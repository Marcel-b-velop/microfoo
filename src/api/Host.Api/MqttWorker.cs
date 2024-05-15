using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Host.Api.Config;
using Host.Application;
using Host.Application.Commands;
using Host.Application.Handler;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace Host.Api;

public class MqttWorker : BackgroundService
{
    private readonly ILogger<MqttWorker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private IManagedMqttClient _client;
    private IEnumerable<string> _subscriptions;

    public MqttWorker(
        ILogger<MqttWorker> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public override async Task StopAsync(
        CancellationToken stoppingToken)
    {
        foreach (var subscription in _subscriptions)
            await _client.UnsubscribeAsync(subscription);

        await _client.StopAsync();
        _client.Dispose();
        _logger.LogInformation("Client stopped.");
        await base.StopAsync(stoppingToken);
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        var log = true;
        var counter = 0;

        do
        {
            try
            {
                var scope = _serviceProvider.CreateScope();
                // var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<CreateMeasurementCommand>>();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var application = GetApplication(configuration);
                _subscriptions = application?.Subscriptions ??
                                 throw new InvalidOperationException("No subscriptions found.");
                var mqttFactory = new MqttFactory();
                _client = mqttFactory.CreateManagedMqttClient();
                _logger.LogInformation($"MQTT Server: {application.Server}");
                _logger.LogInformation($"MQTT Client ID: {application.ClientId}");
                var clientOptions = new MqttClientOptions
                {
                    ClientId = application.ClientId,
                    Credentials = new MqttClientCredentials(application.UserName,
                        Encoding.ASCII.GetBytes(application.Password)),
                    ChannelOptions = new MqttClientTcpOptions
                    {
                        Server = application.Server,
                        Port = int.Parse(application.Port),
                        TlsOptions = new MqttClientTlsOptions
                        {
                            UseTls = true,
                            AllowUntrustedCertificates = false,
                            IgnoreCertificateChainErrors = false,
                            IgnoreCertificateRevocationErrors = false
                        }
                    }
                };
                var managedMqttClientOptions = new ManagedMqttClientOptionsBuilder()
                    .WithClientOptions(clientOptions)
                    .Build();
                await _client.StartAsync(managedMqttClientOptions);

                foreach (var subscription in _subscriptions)
                    await _client.SubscribeAsync(subscription);

                _client.ApplicationMessageReceivedAsync += async e =>
                {
                    using var scope = _serviceProvider.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<ProcessMessageCommand>>();
                    Console.WriteLine("Received application message.");
                    var payload = Encoding.Default.GetString(e.ApplicationMessage.PayloadSegment);
                    var address = e.ApplicationMessage.Topic;
                    var cmd = new ProcessMessageCommand
                    {
                        Payload = payload,
                        Topic = address
                    };
                    await handler.Handle(cmd);
                    Console.WriteLine("Topic: " + address);
                    Console.WriteLine("Value: " + payload);
                    // var command = new CreateMeasurementCommand(address, payload);
                    // await handler.HandleAsync(command, stoppingToken);
                };

                _logger.LogInformation("The managed MQTT client is connected.");

                // Wait until the queue is fully processed.
                SpinWait.SpinUntil(() => _client.PendingApplicationMessagesCount == 0, 10_000);
                _logger.LogInformation("Client connected: {S}", _client.IsConnected.ToString());
                _logger.LogInformation("Pending messages: {ClientPendingApplicationMessagesCount}",
                    _client.PendingApplicationMessagesCount);
                log = false;
            }
            catch (Exception e)
            {
                _logger.LogError($"Fehler beim Starten des Workers. {e.StackTrace}", e);
                await Task.Delay(3_000, stoppingToken);
            }

            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        } while (log);
    }

    private ApplicationConfig? GetApplication(
        IConfiguration configuration)
    {
        try
        {
            var application = configuration
                .GetSection("Application")
                .Get<ApplicationConfig>();
            return application;
        }
        catch (Exception e)
        {
            _logger.LogError("Error getting Application Settings", e.StackTrace);
            throw;
        }
    }
}
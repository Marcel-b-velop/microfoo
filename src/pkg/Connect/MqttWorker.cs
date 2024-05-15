using System.Text;
using System.Text.Json;
using com.b_velop.microfe.connect.Models;
using com.b_velop.microfe.shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace com.b_velop.microfe.connect;

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

    private async Task ApplyConnectAsync(IConfiguration configuration)
    {
        var application = GetApplication(configuration);
        var message = new MqttApplicationMessageBuilder()
            .WithTopic(ApplicationTopic.Connect)
            .WithPayload(JsonSerializer.Serialize(application))
            .WithRetainFlag()
            .Build();
        await _client.EnqueueAsync(message);
        // Wait until the queue is fully processed.
        SpinWait.SpinUntil(() => _client.PendingApplicationMessagesCount == 0, 10_000);
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
                var applicationConfig = GetApplicationConfig(configuration);
                _subscriptions = applicationConfig?.Subscriptions ??
                                 throw new InvalidOperationException("No subscriptions found.");
                var mqttFactory = new MqttFactory();
                _client = mqttFactory.CreateManagedMqttClient();
                _logger.LogInformation($"MQTT Server: {applicationConfig.Server}");
                _logger.LogInformation($"MQTT Client ID: {applicationConfig.ClientId}");
                var clientOptions = new MqttClientOptions
                {
                    ClientId = applicationConfig.ClientId,
                    Credentials = new MqttClientCredentials(applicationConfig.UserName,
                        Encoding.ASCII.GetBytes(applicationConfig.Password)),
                    ChannelOptions = new MqttClientTcpOptions
                    {
                        Server = applicationConfig.Server,
                        Port = int.Parse(applicationConfig.Port),
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
                    var payload = Encoding.Default.GetString(e.ApplicationMessage.PayloadSegment);
                    var address = e.ApplicationMessage.Topic;
                    if (address.Equals(ApplicationTopic.CallForApplications))
                    {
                        await ApplyConnectAsync(configuration);
                    }
                };

                _logger.LogInformation("The managed MQTT client is connected.");

                await ApplyConnectAsync(configuration);
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

    private ApplicationConfig? GetApplicationConfig(
        IConfiguration configuration)
    {
        try
        {
            var application = configuration
                .GetSection("ApplicationConfig")
                .Get<ApplicationConfig>();
            return application;
        }
        catch (Exception e)
        {
            _logger.LogError("Error getting ApplicationConfig Settings", e.StackTrace);
            throw;
        }
    }

    private Application? GetApplication(
        IConfiguration configuration)
    {
        try
        {
            var application = configuration
                .GetSection("Application")
                .Get<Application>();
            return application;
        }
        catch (Exception e)
        {
            _logger.LogError("Error getting Application Settings", e.StackTrace);
            throw;
        }
    }
}
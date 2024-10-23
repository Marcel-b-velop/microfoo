using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using com.b_velop.microfe.shared.Client;
using com.b_velop.microfe.shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using IServiceProvider = System.IServiceProvider;

namespace com.b_velop.microfe.shared.workers;

public class RegisterApplicationWorker(IServiceProvider serviceProvider, IOptions<ApplicationConfiguration> config)
    : BackgroundService
{
    private Timer _timer;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(RegisterApplication, serviceProvider, TimeSpan.FromSeconds(10), Timeout.InfiniteTimeSpan);
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _timer.Dispose();
        Console.WriteLine("Disposed RegisterApplicationWorker.");
        base.Dispose();
    }

    private void RegisterApplication(object? obj)
    {
        var serviceProvider = obj as IServiceProvider;
        var http = serviceProvider.GetRequiredService<IHostClient>();
        http.RegisterApplication(config.Value);
    }
}
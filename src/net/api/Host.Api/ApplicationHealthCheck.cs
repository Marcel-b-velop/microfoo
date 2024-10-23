using com.b_velop.microfe.Models;

namespace com.b_velop.microfe;

public class ApplicationHealthCheck : BackgroundService
{
    private Timer _timer;

    public override void Dispose()
    {
        _timer.Dispose();
        base.Dispose();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(CheckAlive, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        return Task.CompletedTask;
    }

    private void CheckAlive(object obj)
    {
        Console.WriteLine("Test Timer");
        var applications = ApplicationStore.Applications;
        foreach (var application in applications)
        {
            var lastSeen = application.Value.LastSeen;
            if (DateTime.Now - lastSeen > TimeSpan.FromMinutes(1))
            {
                // Application is dead
                applications.TryRemove(application.Key, out _);
                Console.WriteLine($"App {application.Key} is dead.");
            }
        }
    }
}
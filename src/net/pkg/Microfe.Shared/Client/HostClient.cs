using System.Net;
using System.Net.Http.Json;
using com.b_velop.microfe.shared.Models;

namespace com.b_velop.microfe.shared.Client;

public interface IHostClient
{
    Task RegisterApplication(ApplicationConfiguration config);
    Task ApplicationAlive(ApplicationConfiguration config);
}

public class HostClient(HttpClient _httpClient) : IHostClient
{
    private Timer _timer;

    public Task RegisterApplication(ApplicationConfiguration config)
    {
        _timer = new Timer(TryRegisterApplication, config, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        return Task.CompletedTask;
    }

    private void TryRegisterApplication(object? obj)
    {
        if (obj is not ApplicationConfiguration config)
            throw new InvalidOperationException("Could not cast object to ApplicationConfiguration.");
        try
        {
            var result = _httpClient.PostAsJsonAsync("api/host", config).Result;
            var isSuccessful = result.IsSuccessStatusCode;
            if (isSuccessful)
            {
                Console.WriteLine("Application registered.");
                _timer.Dispose();
            }
            else
                Console.WriteLine("Could not register application.");
        }
        catch (WebException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Could not register application.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Could not register application.");
        }
    }

    public async Task ApplicationAlive(ApplicationConfiguration config)
    {
        var response = await _httpClient.GetAsync($"api/host/alive/{config.Name}");
        if (response.StatusCode == HttpStatusCode.NotFound)
            await RegisterApplication(config);
    }
}
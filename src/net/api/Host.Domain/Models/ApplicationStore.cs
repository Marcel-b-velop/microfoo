using System.Collections.Concurrent;

namespace com.b_velop.microfe.Models;

public static class ApplicationStore
{
    public static ConcurrentDictionary<string, Application> Applications { get; } = new();
}
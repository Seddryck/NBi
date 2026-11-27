using System.Collections.Generic;
using System.IO;
using NUnit.Framework.Api;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace NBi.NUnit.Runtime.Embed;

public class Engine
{
    private static readonly object RunLock = new();

    protected string BinPath { get; }

    public Engine()
        : this(string.Empty)
    { }

    public Engine(string binPath)
        => BinPath = binPath;

    public ITestResult Execute(string configFileName)
        => Execute(configFileName, TestFilter.Empty);

    public ITestResult Execute(string configFileName, ITestFilter filter)
    {
        // The runtime keeps suite variables in static properties.
        lock (RunLock)
            return Run(configFileName, filter);
    }

    private ITestResult Run(string configFileName, ITestFilter filter)
    {
        var previousFile = ConfigurationProvider.ExplicitFile.Value;
        ConfigurationProvider.ExplicitFile.Value = Path.GetFullPath(configFileName);
        try
        {
            var assembly = typeof(TestSuite).Assembly;
            var runner = new NUnitTestAssemblyRunner(new DefaultTestAssemblyBuilder());
            var settings = new Dictionary<string, object>();
            if (string.IsNullOrEmpty(BinPath))
                runner.Load(assembly, settings);
            else
                runner.Load(Path.Combine(AppContext.BaseDirectory, BinPath, Path.GetFileName(assembly.Location)), settings);
            return runner.Run(TestListener.NULL, filter);
        }
        finally
        {
            ConfigurationProvider.ExplicitFile.Value = previousFile;
        }
    }
}

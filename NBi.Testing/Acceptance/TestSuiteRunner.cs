using NUnit.Core;
using NUnit.Framework;

namespace NBi.Testing.Acceptance
{
    [TestFixture]
    public class TestSuiteRunner
    {
        [Test]
        protected void Run()
        {
            var assemblyName = @"NBi.Testing.dll";

            CoreExtensions.Host.InitializeService();
            SimpleTestRunner runner = new SimpleTestRunner();
            TestPackage package = new TestPackage( "Test");
            package.TestName = "NBi.Testing.Acceptance.RuntimeOverrider";
            package.Assemblies.Add(assemblyName);
            if( runner.Load(package) )     
            {         
                TestResult result = runner.Run( new NullListener(), TestFilter.Empty, false, LoggingThreshold.Off ); 
                Assert.That(result.IsSuccess, Is.True);    
            }
            else
                Assert.Fail("Unable to load the TestPackage from assembly '{0}'", assemblyName);
        }

    }
}

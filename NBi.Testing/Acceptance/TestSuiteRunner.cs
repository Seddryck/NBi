using NUnit.Core;
using NUnit.Framework;

namespace NBi.Testing.Acceptance
{
    [TestFixture]
    public class TestSuiteRunner
    {
        /// <summary>
        /// Runner to load and execute the Acceptance Tests with the code as close as possible to the true Runtime
        /// <remarks>Yes I know it's a bit tricky</remarks>
        /// </summary>
        [Test]
        [Category ("Acceptance")]
        public void RunAllAcceptanceTests()
        {
            //Define the name of itself
            var assemblyName = @"NBi.Testing.dll";

            //Instantiate a SimpleTestRunner
            CoreExtensions.Host.InitializeService();
            SimpleTestRunner runner = new SimpleTestRunner();

            //Define the test package as all the tests of this assembly in the class RuntimeOverrider
            //The assembly (and so the tests) will be filtered based on TestName
            TestPackage package = new TestPackage( "Test");
            package.TestName = "NBi.Testing.Acceptance.RuntimeOverrider"; //Filter
            package.Assemblies.Add(assemblyName);

            //Load the tests from the filtered package (so we don't need to filter again!)
            if( runner.Load(package) )     
            {         
                //Run all the tests (Have I said I've previsously filtered ? ... No seriously you read this kind of comment?)
                TestResult result = runner.Run( new NullListener(), TestFilter.Empty, false, LoggingThreshold.Off ); 
                //Ensure the acceptance test suite is fully positive!
                Assert.That(result.IsSuccess, Is.True);    
            }
            else
                Assert.Fail("Unable to load the TestPackage from assembly '{0}'", assemblyName);
        }

    }
}

using System;
using System.Linq;
using System.ServiceProcess;
using Moq;
using NBi.Core.Decoration.Process;
using NBi.Core.Scalar.Resolver;
using NUnit.Framework;

namespace NBi.Testing.Integration.Core.WindowsService
{
    [TestFixture]
    [Category("WindowsService")]
    public class RunningConditionTest
    {
        public const string SERVICE_NAME = "SQLWriter";

        [Test]
        public void Validate_OnStoppedService_False()
        {
            //Ensure the service is stopped
            var service = new ServiceController(SERVICE_NAME);
            var timeout = TimeSpan.FromMilliseconds(5000);
            if (service.Status!=ServiceControllerStatus.Stopped)
                service.Stop();
            service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

            //Mock the args and setup command
            var args = Mock.Of<IRunningConditionArgs>(
                isRunning => isRunning.ServiceName == new LiteralScalarResolver<string>(SERVICE_NAME)
                    && isRunning.TimeOut == new LiteralScalarResolver<int>("5000")
                );
            var factory = new ProcessConditionFactory();
            var condition = factory.Instantiate(args);

            //Assert
            Assert.That(condition.Validate(), Is.False);
        }

        [Test]
        public void Validate_OnNotExistingService_False()
        {
            //Mock the args and setup command
            var args = Mock.Of<IRunningConditionArgs>(
                isRunning => isRunning.ServiceName == new LiteralScalarResolver<string>("NOT EXISTING")
                    && isRunning.TimeOut == new LiteralScalarResolver<int>("5000")
                );
            var factory = new ProcessConditionFactory();
            var condition = factory.Instantiate(args);

            //Assert
            Assert.That(condition.Validate(), Is.False);
        }

        [Test]
        public void Validate_OnRunningService_True()
        {
            //Ensure the service is stopped
            var service = new ServiceController(SERVICE_NAME);
            var timeout = TimeSpan.FromMilliseconds(5000);
            if (service.Status != ServiceControllerStatus.Running)
                service.Start();
            service.WaitForStatus(ServiceControllerStatus.Running, timeout);

            //Mock the args and setup command
            var args = Mock.Of<IRunningConditionArgs>(
                isRunning => isRunning.ServiceName == new LiteralScalarResolver<string>(SERVICE_NAME)
                    && isRunning.TimeOut == new LiteralScalarResolver<int>("5000")
                );
            var factory = new ProcessConditionFactory();
            var condition = factory.Instantiate(args);

            //Assert
            Assert.That(condition.Validate(), Is.True);
        }
    }
}

using System;
using System.Linq;
using System.ServiceProcess;
using Moq;
using NBi.Core.Decoration.Process;
using NBi.Core.Decoration.Process.Commands;
using NBi.Core.Scalar.Resolver;
using NUnit.Framework;

namespace NBi.Testing.Integration.Core.Decoration.Process.Commands
{
    [TestFixture]
    [Category("WindowsService")]
    public class StopCommandTest
    {
        public const string SERVICE_NAME = "SQLWriter";

        [Test]
        public void Execute_InitiallyStarted_SericeIsStopped()
        {
            //Ensure the service is stopped
            var service = new ServiceController(SERVICE_NAME);
            var timeout = TimeSpan.FromMilliseconds(5000);
            if (service.Status != ServiceControllerStatus.Running)
                service.Start();
            service.WaitForStatus(ServiceControllerStatus.Running, timeout);

            //Mock the args and setup command
            var args = Mock.Of<IStopCommandArgs>(
                stop => stop.ServiceName == new LiteralScalarResolver<string>(SERVICE_NAME)
                    && stop.TimeOut == new LiteralScalarResolver<int>("5000")
                );
            var command = new StopCommand(args);

            //Apply the test
            command.Execute();

            //Assert
            service.Refresh();
            Assert.That(service.Status, Is.EqualTo(ServiceControllerStatus.Stopped));
        }

        [Test]
        public void Execute_InitiallyStoped_SericeIsStopped()
        {
            //Ensure the service is stopped
            var service = new ServiceController(SERVICE_NAME);
            var timeout = TimeSpan.FromMilliseconds(5000);
            if (service.Status != ServiceControllerStatus.Stopped)
                service.Stop();
            service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

            //Mock the args and setup command
            var args = Mock.Of<IStopCommandArgs>(
                stop => stop.ServiceName == new LiteralScalarResolver<string>(SERVICE_NAME)
                    && stop.TimeOut == new LiteralScalarResolver<int>("5000")
                );
            var command = new StopCommand(args);

            //Apply the test
            command.Execute();

            //Assert
            service.Refresh();
            Assert.That(service.Status, Is.EqualTo(ServiceControllerStatus.Stopped));
        }
    }
}

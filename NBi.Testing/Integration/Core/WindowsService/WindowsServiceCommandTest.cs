using System;
using System.Linq;
using System.ServiceProcess;
using Moq;
using NBi.Core.WindowsService;
using NUnit.Framework;

namespace NBi.Testing.Integration.Core.WindowsService
{
    [TestFixture]
    [Category("Windows Service")]
    public class WindowsServiceCommandTest
    {
        public const string SERVICE_NAME = "SQLWriter";

        [Test]
        public void Execute_StartCommand_ServiceIsRunning()
        {
            //Ensure the service is stopped
            var service = new ServiceController(SERVICE_NAME);
            var timeout = TimeSpan.FromMilliseconds(5000);
            if (service.Status!=ServiceControllerStatus.Stopped)
                service.Stop();
            service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

            //Mock the commandXml
            var info = Mock.Of<IWindowsServiceStartCommand>(
                start => start.ServiceName==SERVICE_NAME
                    && start.TimeOut==5000
                );
            
            //Apply the test
            var cmd = WindowsServiceCommand.Start(info);
            cmd.Execute();

            //Assert
            service.Refresh();
            Assert.That(service.Status, Is.EqualTo(ServiceControllerStatus.Running));
        }

        [Test]
        public void Execute_StartCommandOnStartedService_ServiceIsRunning()
        {
            //Ensure the service is started
            var service = new ServiceController(SERVICE_NAME);
            var timeout = TimeSpan.FromMilliseconds(5000);
            if (service.Status != ServiceControllerStatus.Running)
                service.Start();
            service.WaitForStatus(ServiceControllerStatus.Running, timeout);

            //Mock the commandXml
            var info = Mock.Of<IWindowsServiceStartCommand>(
                start => start.ServiceName == SERVICE_NAME
                    && start.TimeOut == 5000
                );

            //Apply the test
            var cmd = WindowsServiceCommand.Start(info);
            cmd.Execute();

            //Assert
            service.Refresh();
            Assert.That(service.Status, Is.EqualTo(ServiceControllerStatus.Running));
        }

        [Test]
        public void Execute_StopCommand_SericeIsStopped()
        {
            //Ensure the service is stopped
            var service = new ServiceController(SERVICE_NAME);
            var timeout = TimeSpan.FromMilliseconds(5000);
            if (service.Status != ServiceControllerStatus.Running)
                service.Start();
            service.WaitForStatus(ServiceControllerStatus.Running, timeout);

            //Mock the commandXml
            var info = Mock.Of<IWindowsServiceStopCommand>(
                start => start.ServiceName == SERVICE_NAME
                    && start.TimeOut == 5000
                );

            //Apply the test
            var cmd = WindowsServiceCommand.Stop(info);
            cmd.Execute();

            //Assert
            service.Refresh();
            Assert.That(service.Status, Is.EqualTo(ServiceControllerStatus.Stopped));
        }

        [Test]
        public void Execute_StopCommandOnStoppedService_SericeIsStopped()
        {
            //Ensure the service is stopped
            var service = new ServiceController(SERVICE_NAME);
            var timeout = TimeSpan.FromMilliseconds(5000);
            if (service.Status != ServiceControllerStatus.Stopped)
                service.Stop();
            service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

            //Mock the commandXml
            var info = Mock.Of<IWindowsServiceStopCommand>(
                start => start.ServiceName == SERVICE_NAME
                    && start.TimeOut == 5000
                );

            //Apply the test
            var cmd = WindowsServiceCommand.Stop(info);
            cmd.Execute();

            //Assert
            service.Refresh();
            Assert.That(service.Status, Is.EqualTo(ServiceControllerStatus.Stopped));
        }
    }
}

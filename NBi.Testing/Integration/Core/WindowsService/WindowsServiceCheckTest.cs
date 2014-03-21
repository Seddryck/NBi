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
    public class WindowsServiceCheckTest
    {
        public const string SERVICE_NAME = "SQLWriter";

        [Test]
        public void Validate_IsRunningPredicateOnStoppedService_False()
        {
            //Ensure the service is stopped
            var service = new ServiceController(SERVICE_NAME);
            var timeout = TimeSpan.FromMilliseconds(5000);
            if (service.Status!=ServiceControllerStatus.Stopped)
                service.Stop();
            service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

            //Mock the commandXml
            var info = Mock.Of<IWindowsServiceRunningCheck>(
                start => start.ServiceName==SERVICE_NAME
                    && start.TimeOut==5000
                );
            
            //Apply the test
            var predicate = WindowsServiceCheck.IsRunning(info);
            var result = predicate.Validate();

            //Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Execute_IsRunningPredicateOnRunningService_True()
        {
            //Ensure the service is stopped
            var service = new ServiceController(SERVICE_NAME);
            var timeout = TimeSpan.FromMilliseconds(5000);
            if (service.Status != ServiceControllerStatus.Running)
                service.Start();
            service.WaitForStatus(ServiceControllerStatus.Running, timeout);

            //Mock the commandXml
            var info = Mock.Of<IWindowsServiceRunningCheck>(
                start => start.ServiceName == SERVICE_NAME
                    && start.TimeOut == 5000
                );

            //Apply the test
            var predicate = WindowsServiceCheck.IsRunning(info);
            var result = predicate.Validate();

            //Assert
            Assert.That(result, Is.True);
        }
    }
}

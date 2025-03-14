﻿using System;
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
    public class StartCommandTest
    {
        public const string SERVICE_NAME = "SQLWriter";

        [Test]
        public void Execute_InitiallyStopped_ServiceIsRunning()
        {
            //Ensure the service is stopped
            var service = new ServiceController(SERVICE_NAME);
            var timeout = TimeSpan.FromMilliseconds(5000);
            if (service.Status != ServiceControllerStatus.Stopped)
                service.Stop();
            service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

            //Mock the args and setup command
            var args = new ServiceStartCommandArgs
            (
                Guid.NewGuid()
                , new LiteralScalarResolver<string>(SERVICE_NAME)
                , new LiteralScalarResolver<int>("5000")
            );
            var command = new StartCommand(args);

            //Execute command
            command.Execute();

            //Assert
            service.Refresh();
            Assert.That(service.Status, Is.EqualTo(ServiceControllerStatus.Running));
        }

        [Test]
        public void Execute_InitiallyStarted_ServiceIsRunning()
        {
            //Ensure the service is started
            var service = new ServiceController(SERVICE_NAME);
            var timeout = TimeSpan.FromMilliseconds(5000);
            if (service.Status != ServiceControllerStatus.Running)
                service.Start();
            service.WaitForStatus(ServiceControllerStatus.Running, timeout);

            //Mock the args and setup command
            var args = new ServiceStartCommandArgs
            (
                Guid.NewGuid()
                , new LiteralScalarResolver<string>(SERVICE_NAME)
                , new LiteralScalarResolver<int>("5000")
            );
            var command = new StartCommand(args);

            //Apply the test
            command.Execute();

            //Assert
            service.Refresh();
            Assert.That(service.Status, Is.EqualTo(ServiceControllerStatus.Running));
        }
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using Moq;
using NBi.Core.Decoration.DataEngineering;
using NUnit.Framework;
using NBi.Core;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Decoration;
using NBi.Core.Decoration.Grouping;
using NBi.Core.Decoration.Grouping.Commands;
using NBi.Extensibility.Decoration;

namespace NBi.Core.Testing.Decoration.Grouping;

[TestFixture]
public class GroupCommandFactoryTest
{

    #region SetUp & TearDown
    //Called only at instance creation
    [OneTimeSetUp]
    public void SetupMethods()
    {

    }

    //Called only at instance destruction
    [OneTimeTearDown]
    public void TearDownMethods()
    {
    }

    //Called before each test
    [SetUp]
    public void SetupTest()
    {
    }

    //Called after each test
    [TearDown]
    public void TearDownTest()
    {
    }
    #endregion

    [Test]
    public void Get_ParallelCommandArgs_ParallelCommand()
    {
        var group = new GroupParallelCommandArgs(Guid.NewGuid(), true, []);

        var factory = new GroupCommandFactory();
        var impl = factory.Instantiate(group, []);

        Assert.That(impl, Is.TypeOf<ParallelCommand>());
    }

    [Test]
    public void Get_SequentialCommandArgs_SequentialCommand()
    {
        var group = new GroupSequentialCommandArgs(Guid.NewGuid(), true, []);

        var factory = new GroupCommandFactory();
        var impl = factory.Instantiate(group, []);

        Assert.That(impl, Is.TypeOf<SequentialCommand>());
    }

}

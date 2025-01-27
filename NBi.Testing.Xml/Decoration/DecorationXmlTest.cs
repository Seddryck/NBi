using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Decoration;
using NBi.Xml.Decoration.Command;
using NUnit.Framework;

namespace NBi.Xml.Testing.Unit.Decoration;

[TestFixture]
public class DecorationXmlTest : BaseXmlTest
{ 
    
    [Test]
    public void Deserialize_SampleFile_Setup()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Setup, Is.TypeOf<SetupXml>());
    }


    [Test]
    public void Deserialize_SampleFile_SetupCountCommands()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Setup.Commands, Has.Count.EqualTo(4));
    }

    [Test]
    public void Deserialize_SampleFile_Cleanup()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Cleanup, Is.TypeOf<CleanupXml>());
    }

    [Test]
    public void Deserialize_SampleFile_CleanupCountCommands()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Cleanup.Commands, Has.Count.EqualTo(1));
    }

    [Test]
    public void Deserialize_SampleFile_LoadCommand()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Setup.Commands[1], Is.TypeOf<TableLoadXml>());
        var cmd = (TableLoadXml)ts.Tests[testNr].Setup.Commands[1];
        Assert.That(cmd.ConnectionString, Is.EqualTo(@"Data Source=(local)\SQL2017;Initial Catalog=AdventureWorksDW2012;Integrated Security=true"));
        Assert.That(cmd.TableName, Is.EqualTo("Users"));
        Assert.That(cmd.InternalFileName, Is.EqualTo("Users.csv"));
    }


    [Test]
    public void Deserialize_SampleFile_ResetCommand()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Setup.Commands[0], Is.TypeOf<TableResetXml>());
        var cmd = (TableResetXml)ts.Tests[testNr].Setup.Commands[0];
        Assert.That(cmd.ConnectionString, Is.EqualTo(@"Data Source=(local)\SQL2017;Initial Catalog=AdventureWorksDW2012;Integrated Security=true"));
        Assert.That(cmd.TableName, Is.EqualTo("Users"));
    }

    [Test]
    public void Deserialize_SampleFile_SqlRunCommand()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Setup.Commands[2], Is.TypeOf<SqlRunXml>());
        var cmd = (SqlRunXml)ts.Tests[testNr].Setup.Commands[2];
        Assert.That(cmd.ConnectionString, Is.EqualTo(@"Data Source=(local)\SQL2017;Initial Catalog=AdventureWorksDW2012;Integrated Security=true"));
        Assert.That(cmd.Name, Is.EqualTo("MySQLtoRun.sql"));
        Assert.That(cmd.Version, Is.EqualTo("SqlServer2016"));

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Setup.Commands[3], Is.TypeOf<SqlRunXml>());
        var cmd2 = (SqlRunXml)ts.Tests[testNr].Setup.Commands[3];
        Assert.That(cmd2.Version, Is.EqualTo("SqlServer2014"));
    }

    [Test]
    public void Deserialize_SampleFile_ConnectionStringFromDefaults()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Setup.Commands[0], Is.TypeOf<TableResetXml>());
        var cmd = (TableResetXml)ts.Tests[testNr].Setup.Commands[0];
        Assert.That(cmd.ConnectionString, Is.EqualTo(@"Data Source=(local)\SQL2017;Initial Catalog=AdventureWorksDW2012;Integrated Security=true"));
    }

    [Test]
    public void Deserialize_SampleFile_StartAndStopService()
    {
        int testNr = 2;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Setup.Commands[0], Is.TypeOf<ServiceStartXml>());
        var cmd = (ServiceStartXml)ts.Tests[testNr].Setup.Commands[0];
        Assert.That(cmd.TimeOut, Is.EqualTo("5000")); //Default value
        Assert.That(cmd.ServiceName, Is.EqualTo("MyService")); 

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Cleanup.Commands[0], Is.TypeOf<ServiceStopXml>());
        var cmdCleanup = (ServiceStopXml)ts.Tests[testNr].Cleanup.Commands[0];
        Assert.That(cmdCleanup.TimeOut, Is.EqualTo("15000")); //Value Specified
        Assert.That(cmdCleanup.ServiceName, Is.EqualTo("MyService")); 
    }

    [Test]
    public void Deserialize_ThreeCommandsInOneTask_TaskContainsThreeCommands()
    {
        int testNr = 3;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Setup.Commands, Has.Count.EqualTo(1));
        Assert.That(ts.Tests[testNr].Setup.Commands[0], Is.TypeOf<CommandGroupXml>());
        var commandGroup = (CommandGroupXml)ts.Tests[testNr].Setup.Commands[0];
        Assert.That(commandGroup.Commands, Has.Count.EqualTo(3));
    }

    [Test]
    public void Deserialize_OneTaskWithoutParallelAttributeExplicitelySet_TasksAreParallel()
    {
        int testNr = 3;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        var commandGroup = (CommandGroupXml)ts.Tests[testNr].Setup.Commands[0];
        Assert.That(commandGroup.Parallel, Is.True);
    }

    [Test]
    public void Deserialize_OneTaskWithoutRunOnceAttributeExplicitelySet_TasksAreNotRunOnce()
    {
        int testNr = 3;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        var commandGroup = (CommandGroupXml)ts.Tests[testNr].Setup.Commands[0];
        Assert.That(commandGroup.RunOnce, Is.False);
    }

    [Test]
    public void Deserialize_TasksAndCommandsPermutted_AllTasksAndCommandsAreLoaded()
    {
        int testNr = 4;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Setup.Commands, Has.Count.EqualTo(5));
    }

    [Test]
    public void Deserialize_SecondGroupWithRunOnceSetTrue_SecondGroupWithRunOnce()
    {
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        var group = (CommandGroupXml)ts.Groups[1].Setup.Commands[0];
        Assert.That(group.RunOnce, Is.True);
    }

    [Test]
    public void Deserialize_SecondGroupWithExeRun_SecondGroupWithExeRun()
    {
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        var group = (CommandGroupXml)ts.Groups[1].Setup.Commands[0];
        Assert.That(group.Commands[2], Is.TypeOf<ExeRunXml>());
    }

    [Test]
    public void Deserialize_SampleFile_ParentSetup()
    {
        int groupNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Groups[groupNr].Tests, Has.Count.EqualTo(2));
        foreach (var test in ts.Groups[groupNr].Tests)
        {
            Assert.That(test.Setup.Commands, Has.Count.EqualTo(2));
            Assert.That(test.Setup.Commands[0], Is.InstanceOf<ServiceStartXml>());
            Assert.That(test.Setup.Commands[1], Is.InstanceOf<TableLoadXml>());
        }
    }

    [Test]
    public void Deserialize_SampleFile_ProcessRun()
    {
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        int groupNr = 2;

        // Check the properties of the object.
        var command = ts.Groups[groupNr].Tests[0].Setup.Commands[0];

        Assert.That(command, Is.TypeOf<ExeRunXml>());
        var run = (ExeRunXml)command;
        Assert.That(run.Name, Is.EqualTo("clean.exe"));
        Assert.That(run.Path, Is.EqualTo(@"Batches\"));
        Assert.That(run.Argument, Is.EqualTo("-all"));
        Assert.That(run.TimeOut, Is.EqualTo("1000"));
    }

    [Test]
    public void Deserialize_SampleFile_ProcessKill()
    {
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        int groupNr = 2;

        // Check the properties of the object.
        var command = ts.Groups[groupNr].Tests[1].Setup.Commands[0];

        Assert.That(command, Is.TypeOf<ExeKillXml>());
        var kill = (ExeKillXml)command;
        Assert.That(kill.ProcessName, Is.EqualTo(@"PBIDesktop"));
    }

    [Test]
    public void Deserialize_SampleFile_ProcessWait()
    {
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        int groupNr = 2;

        // Check the properties of the object.
        var command = ts.Groups[groupNr].Tests[2].Setup.Commands[0];

        Assert.That(command, Is.TypeOf<WaitXml>());
        var wait = (WaitXml)command;
        Assert.That(wait.MilliSeconds, Is.EqualTo("1000"));
    }

    [Test]
    public void Deserialize_SampleFile_ConnectionWait()
    {
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        int groupNr = 2;

        // Check the properties of the object.
        var command = ts.Groups[groupNr].Tests[3].Setup.Commands[0];

        Assert.That(command, Is.TypeOf<ConnectionWaitXml>());
        var wait = (ConnectionWaitXml)command;
        Assert.That(wait.TimeOut, Is.EqualTo("30000"));
        Assert.That(wait.ConnectionString, Is.EqualTo("pbix = My Solution"));
    }

    [Test]
    public void Deserialize_SampleFile_ProcessRunWithoutOptionalArguments()
    {
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        int groupNr = 2;

        // Check the properties of the object.
        var command = ts.Groups[groupNr].Tests[0].Setup.Commands[1];

        Assert.That(command, Is.TypeOf<ExeRunXml>());
        var run = (ExeRunXml)command;
        Assert.That(run.Name, Is.EqualTo(@"load.exe"));
        Assert.That(run.Path, Is.Null.Or.Empty);
        Assert.That(run.TimeOut, Is.EqualTo("0"));
    }


    [Test]
    public void Deserialize_SampleFile_BatchRun()
    {
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        int groupNr = 3;

        // Check the properties of the object.
        var command = ts.Groups[groupNr].Tests[0].Setup.Commands[0];

        Assert.That(command, Is.TypeOf<SqlRunXml>());
        var batchRun = (SqlRunXml)command;
        Assert.That(batchRun.Name, Is.EqualTo(@"build.sql"));
        Assert.That(batchRun.Path, Is.EqualTo(@"Batches\"));
        Assert.That(batchRun.ConnectionString, Is.EqualTo("Data source=(local);Initial Catalog=MyDB"));
    }

    [Test]
    public void Deserialize_SampleFile_BatchRunWithoutOptional()
    {
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        int groupNr = 3;

        // Check the properties of the object.
        var command = ts.Groups[groupNr].Tests[0].Setup.Commands[1];

        Assert.That(command, Is.TypeOf<SqlRunXml>());
        var batchRun = (SqlRunXml)command;
        Assert.That(batchRun.Name, Is.EqualTo(@"import.sql"));
        Assert.That(batchRun.ConnectionString, Is.EqualTo(@"Data Source=(local)\SQL2017;Initial Catalog=AdventureWorksDW2012;Integrated Security=true"));
    }

    [Test]
    public void Deserialize_SampleFile_FileDelete()
    {
        int groupNr = 4;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        var command = ts.Groups[groupNr].Tests[0].Setup.Commands[0];

        Assert.That(command, Is.TypeOf<FileDeleteXml>());
        var delete = (FileDeleteXml)command;
        Assert.That(delete.FileName, Is.EqualTo(@"toto.xls"));
        Assert.That(delete.Path, Is.EqualTo(@"Temp\"));
    }

    
    [Test]
    public void Deserialize_SampleFile_FileMove()
    {
        int groupNr = 4;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        var command = ts.Groups[groupNr].Tests[1].Setup.Commands[0];

        Assert.That(command, Is.TypeOf<FileCopyXml>());
        var copy = (FileCopyXml)command;
        Assert.That(copy.FileName, Is.EqualTo(@"toto.xls"));
        Assert.That(copy.DestinationPath, Is.EqualTo(@"Temp\"));
        Assert.That(copy.SourcePath, Is.EqualTo(@"Backup\"));
    }
}

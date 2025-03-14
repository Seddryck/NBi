﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Assemblies.Decoration;
using NBi.Core.Decoration;
using NBi.Core.Decoration.DataEngineering;
using NBi.Core.Decoration.Grouping;
using NBi.Core.Decoration.IO;
using NBi.Core.Decoration.Process;
using NBi.Core.Injection;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
using NBi.Extensibility.Resolving;
using NBi.NUnit.Builder.Helper;
using NBi.Xml.Decoration;
using NBi.Xml.Decoration.Command;
using NUnit.Framework;

namespace NBi.Testing.Unit.NUnit.Builder.Helper
{
    public class SetupHelperTest
    {
        [Test]
        public void Execute_UniqueCommand_CorrectInterpretation()
        {
            var xml = new SetupXml()
            {
                Commands = new List<DecorationCommandXml>()
                    { new FileDeleteXml()  { FileName="foo.txt", Path = @"C:\Temp\" } }
            };

            var helper = new SetupHelper(new ServiceLocator(), new Dictionary<string, IVariable>());
            var listCommandArgs = helper.Execute(xml.Commands);
            Assert.That(listCommandArgs.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Execute_MultipleCommands_CorrectInterpretation()
        {
            var xml = new SetupXml()
            {
                Commands = new List<DecorationCommandXml>()
                {
                    new FileDeleteXml()  { FileName="foo.txt", Path = @"C:\Temp\" },
                    new ExeKillXml()  { ProcessName="bar" },
                    new WaitXml()  { MilliSeconds = "2000" }
                }
            };

            var helper = new SetupHelper(new ServiceLocator(), new Dictionary<string, IVariable>());
            var listCommandArgs = helper.Execute(xml.Commands);
            Assert.That(listCommandArgs.Count(), Is.EqualTo(3));
        }

        [Test]
        [TestCase(typeof(SqlRunXml), typeof(SqlBatchRunCommandArgs))]
        [TestCase(typeof(TableLoadXml), typeof(TableLoadCommandArgs))]
        [TestCase(typeof(TableResetXml), typeof(TableTruncateCommandArgs))]
        //[TestCase(typeof(EtlRunXml), typeof(EtlRunCommandArgs))]
        [TestCase(typeof(ConnectionWaitXml), typeof(ConnectionWaitCommandArgs))]
        [TestCase(typeof(FileDeleteXml), typeof(IoDeleteCommandArgs))]
        [TestCase(typeof(FileDeletePatternXml), typeof(IoDeletePatternCommandArgs))]
        [TestCase(typeof(FileDeleteExtensionXml), typeof(IoDeleteExtensionCommandArgs))]
        [TestCase(typeof(FileCopyXml), typeof(IoCopyCommandArgs))]
        [TestCase(typeof(FileCopyPatternXml), typeof(IoCopyPatternCommandArgs))]
        [TestCase(typeof(FileCopyExtensionXml), typeof(IoCopyExtensionCommandArgs))]
        [TestCase(typeof(ExeKillXml), typeof(ProcessKillCommandArgs))]
        [TestCase(typeof(ExeRunXml), typeof(ProcessRunCommandArgs))]
        [TestCase(typeof(ServiceStartXml), typeof(ServiceStartCommandArgs))]
        [TestCase(typeof(ServiceStopXml), typeof(ServiceStopCommandArgs))]
        [TestCase(typeof(WaitXml), typeof(WaitCommandArgs))]
        [TestCase(typeof(CustomCommandXml), typeof(CustomCommandArgs))]
        public void Execute_DecorationCommand_CorrectlyTransformedToArgs(Type xmlType, Type argsType)
        {
            var xmlInstance = Activator.CreateInstance(xmlType);
            Assert.That(xmlInstance, Is.AssignableTo<DecorationCommandXml>());

            var xml = new SetupXml()
            {
                Commands = new List<DecorationCommandXml>()
                    { xmlInstance as DecorationCommandXml }
            };

            var helper = new SetupHelper(new ServiceLocator(), new Dictionary<string, IVariable>());

            var commandArgs = helper.Execute(xml.Commands).ElementAt(0);
            Assert.That(commandArgs, Is.AssignableTo<IDecorationCommandArgs>());
            Assert.That(commandArgs, Is.AssignableTo(argsType));
        }

        [Test]
        [TestCase(true, typeof(GroupParallelCommandArgs))]
        [TestCase(false, typeof(GroupSequentialCommandArgs))]
        public void Execute_GroupCommand_CorrectlyTransformedToArgs(bool isParallel, Type argsType)
        {
            var xml = new SetupXml()
            {
                Commands = new List<DecorationCommandXml>()
                {
                    new CommandGroupXml()
                    {
                        Parallel = isParallel,
                        Commands = new List<DecorationCommandXml>()
                        {
                            new FileDeleteXml()  { FileName="foo.txt", Path = @"C:\Temp\" },
                            new ExeKillXml()  { ProcessName = "bar.exe" }
                        }
                    }
                }
            };

            var helper = new SetupHelper(new ServiceLocator(), new Dictionary<string, IVariable>());

            var commandArgs = helper.Execute(xml.Commands).ElementAt(0);
            Assert.That(commandArgs, Is.AssignableTo(argsType));

            var groupCommandArgs = commandArgs as IGroupCommandArgs;
            Assert.That(groupCommandArgs.Commands.ElementAt(0), Is.AssignableTo<IoDeleteCommandArgs>());
            Assert.That(groupCommandArgs.Commands.ElementAt(1), Is.AssignableTo<ProcessKillCommandArgs>());
        }

        [Test]
        public void Execute_GroupsWithinGroupCommand_CorrectlyTransformedToArgs()
        {
            var xml = new SetupXml()
            {
                Commands = new List<DecorationCommandXml>()
                {
                    new CommandGroupXml()
                    {
                        Parallel = false,
                        Commands = new List<DecorationCommandXml>()
                        {
                            new CommandGroupXml()
                            {
                                Parallel = true,
                                Commands = new List<DecorationCommandXml>()
                                {
                                    new FileDeleteXml()  { FileName="foo.txt", Path = @"C:\Temp\" },
                                    new ExeKillXml()  { ProcessName = "foo.exe" }
                                }
                            },
                            new CommandGroupXml()
                            {
                                Parallel = true,
                                Commands = new List<DecorationCommandXml>()
                                {
                                    new FileDeleteXml()  { FileName="bar.txt", Path = @"C:\Temp\" },
                                    new ExeKillXml()  { ProcessName = "bar.exe" }
                                }
                            }
                        }
                    }
                }
            };

            var helper = new SetupHelper(new ServiceLocator(), new Dictionary<string, IVariable>());

            var commandArgs = helper.Execute(xml.Commands).ElementAt(0);
            Assert.That(commandArgs, Is.AssignableTo<GroupSequentialCommandArgs>());

            var groupCommandArgs = commandArgs as IGroupCommandArgs;
            Assert.That(groupCommandArgs.Commands.ElementAt(0), Is.AssignableTo<GroupParallelCommandArgs>());
            Assert.That(groupCommandArgs.Commands.ElementAt(1), Is.AssignableTo<GroupParallelCommandArgs>());

            foreach (var subGroup in groupCommandArgs.Commands)
            {
                var subGroupCommandArgs = subGroup as IGroupCommandArgs;
                Assert.That(subGroupCommandArgs.Commands.ElementAt(0), Is.AssignableTo<IoDeleteCommandArgs>());
                Assert.That(subGroupCommandArgs.Commands.ElementAt(1), Is.AssignableTo<ProcessKillCommandArgs>());
            }
        }


        [Test]
        public void Execute_CustomCommand_CorrectlyParsed()
        {
            var xml = new SetupXml()
            {
                Commands = new List<DecorationCommandXml>()
                    { new CustomCommandXml()
                        {
                            AssemblyPath ="NBi.Testing"
                            , TypeName = @"CustomCommand"
                            , Parameters = new List<CustomCommandParameterXml>()
                                {
                                    new CustomCommandParameterXml() { Name="foo", StringValue="bar" },
                                    new CustomCommandParameterXml() { Name="quark", StringValue="@myVar" },
                                }
                        }
                    }
            };
            var myVar = new GlobalVariable(new LiteralScalarResolver<object>("bar-foo"));
            var helper = new SetupHelper(new ServiceLocator(), new Dictionary<string, IVariable>() {{ "myVar", myVar } });

            var customCommandArgs = helper.Execute(xml.Commands).ElementAt(0) as CustomCommandArgs;
            Assert.That(customCommandArgs.AssemblyPath, Is.TypeOf<LiteralScalarResolver<string>>());
            Assert.That(customCommandArgs.AssemblyPath.Execute(), Is.EqualTo("NBi.Testing"));
            Assert.That(customCommandArgs.TypeName, Is.TypeOf<LiteralScalarResolver<string>>());
            Assert.That(customCommandArgs.TypeName.Execute(), Is.EqualTo("CustomCommand"));

            Assert.That(customCommandArgs.Parameters, Has.Count.EqualTo(2));
            Assert.That(customCommandArgs.Parameters["foo"], Is.TypeOf<LiteralScalarResolver<string>>());
            var paramValue = customCommandArgs.Parameters["foo"] as LiteralScalarResolver<string>;
            Assert.That(paramValue.Execute(), Is.EqualTo("bar"));

            Assert.That(customCommandArgs.Parameters["quark"], Is.TypeOf<GlobalVariableScalarResolver<string>>());
            var paramValue2 = customCommandArgs.Parameters["quark"] as GlobalVariableScalarResolver<string>;
            Assert.That(paramValue2.Execute(), Is.EqualTo("bar-foo"));
        }

        [Test]
        public void Execute_LiteralArgument_CorrectlyParsed()
        {
            var xml = new SetupXml()
            {
                Commands = new List<DecorationCommandXml>()
                    { new FileDeleteXml()  { FileName="foo.txt", Path = @"C:\Temp\" } }
            };

            var helper = new SetupHelper(new ServiceLocator(), new Dictionary<string, IVariable>());

            var deleteCommandArgs = helper.Execute(xml.Commands).ElementAt(0) as IoDeleteCommandArgs;
            Assert.That(deleteCommandArgs.Name, Is.TypeOf<LiteralScalarResolver<string>>());
            Assert.That(deleteCommandArgs.Name.Execute(), Is.EqualTo("foo.txt"));
        }

        [Test]
        public void Execute_VariableArgument_CorrectlyParsed()
        {
            var xml = new SetupXml()
            {
                Commands = new List<DecorationCommandXml>()
                    { new FileDeleteXml()  { FileName="@myvar", Path = @"C:\Temp\" } }
            };

            var myVar = new GlobalVariable(new LiteralScalarResolver<object>("bar.txt"));
            var helper = new SetupHelper(new ServiceLocator(), new Dictionary<string, IVariable>() { { "myvar", myVar } });

            var deleteCommandArgs = helper.Execute(xml.Commands).ElementAt(0) as IoDeleteCommandArgs;
            Assert.That(deleteCommandArgs.Name, Is.TypeOf<GlobalVariableScalarResolver<string>>());
            Assert.That(deleteCommandArgs.Name.Execute(), Is.EqualTo("bar.txt"));
        }

        [Test]
        public void Execute_FormatArgument_CorrectlyParsed()
        {
            var xml = new SetupXml()
            {
                Commands = new List<DecorationCommandXml>()
                    { new FileDeleteXml()  { FileName="~{@myvar}.csv", Path = @"C:\Temp\" } }
            };

            var myVar = new GlobalVariable(new LiteralScalarResolver<object>("bar"));
            var helper = new SetupHelper(new ServiceLocator(), new Dictionary<string, IVariable>() { { "myvar", myVar } });

            var deleteCommandArgs = helper.Execute(xml.Commands).ElementAt(0) as IoDeleteCommandArgs;
            Assert.That(deleteCommandArgs.Name, Is.TypeOf<FormatScalarResolver>());
            Assert.That(deleteCommandArgs.Name.Execute(), Is.EqualTo("bar.csv"));
        }

        [Test]
        public void Execute_InlineTransformationArgument_CorrectlyParsed()
        {
            var xml = new SetupXml()
            {
                Commands = new List<DecorationCommandXml>()
                    { new FileDeleteXml()  { FileName="@myvar | text-to-upper", Path = @"C:\Temp\" } }
            };

            var myVar = new GlobalVariable(new CSharpScalarResolver<object>(
                        new CSharpScalarResolverArgs("\"foo.txt\"")
                    ));
            var helper = new SetupHelper(new ServiceLocator(), new Dictionary<string, IVariable>() { { "myvar", myVar } });

            var deleteCommandArgs = helper.Execute(xml.Commands).ElementAt(0) as IoDeleteCommandArgs;
            Assert.That(deleteCommandArgs.Name, Is.AssignableTo<IScalarResolver>());
            Assert.That(deleteCommandArgs.Name.Execute(), Is.EqualTo("FOO.TXT"));
            Assert.That(deleteCommandArgs.Name.Execute(), Is.Not.EqualTo("foo.txt"));
        }
    }
}

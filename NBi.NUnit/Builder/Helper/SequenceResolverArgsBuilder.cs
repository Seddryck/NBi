using NBi.Core;
using NBi.Core.Injection;
using NBi.Core.Query;
using NBi.Core.Query.Resolver;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Duration;
using NBi.Core.Sequence.Resolver;
using NBi.Core.Variable;
using NBi.Core.Xml;
using NBi.Xml.Items;
using NBi.Xml.Items.ResultSet;
using NBi.Xml.Items.Xml;
using NBi.Xml.Settings;
using NBi.Xml.Systems;
using NBi.Xml.Variables.Sequence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Builder.Helper
{
    class SequenceResolverArgsBuilder
    {
        private bool isSetup = false;

        private object obj = null;
        private SettingsXml settings = null;
        private IDictionary<string, ITestVariable> globalVariables = new Dictionary<string, ITestVariable>();
        private ISequenceResolverArgs args = null;
        private ColumnType columnType = ColumnType.Numeric;

        private readonly ServiceLocator serviceLocator;

        public SequenceResolverArgsBuilder(ServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public void Setup(object obj)
        {
            this.obj = obj;
            isSetup = true;
        }

        public void Setup(ColumnType columnType)
        {
            this.columnType = columnType;
        }

        public void Setup(SettingsXml settingsXml)
        {
            this.settings = settingsXml;
        }

        public void Setup(IDictionary<string, ITestVariable> globalVariables)
        {
            this.globalVariables = globalVariables;
        }

        public void Build()
        {
            if (!isSetup)
                throw new InvalidOperationException();

            if (obj is SentinelLoopXml)
            {
                var loop = obj as SentinelLoopXml;
                switch (columnType)
                {
                    case ColumnType.Numeric:
                        args = BuildSentinelLoopResolverArgs<int,int>(loop.Seed, loop.Terminal, loop.Step);
                        break;
                    case ColumnType.DateTime:
                        args = BuildSentinelLoopResolverArgs<DateTime,IDuration>(loop.Seed, loop.Terminal, loop.Step);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (args == null)
                throw new ArgumentException();
        }

        public ISequenceResolverArgs GetArgs() => args ?? throw new InvalidOperationException();
        
        private ISequenceResolverArgs BuildSentinelLoopResolverArgs<T, U>(string seed, string terminal, string step)
        {
            var helper = new ScalarHelper(serviceLocator, globalVariables);
            
            var args = new SentinelLoopSequenceResolverArgs<T, U>(
                    helper.InstantiateResolver<T>(seed).Execute(),
                    helper.InstantiateResolver<T>(terminal).Execute(),
                    helper.InstantiateResolver<U>(step).Execute()
                );

            return args;
        }

    }
}

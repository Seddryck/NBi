using NBi.Core.Injection;
using NBi.Core.IO.Filtering;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Duration;
using NBi.Core.Sequence.Resolver;
using NBi.Core.Sequence.Resolver.Loop;
using NBi.Core.Variable;
using NBi.Xml.Items;
using NBi.Xml.Settings;
using NBi.Xml.Variables.Custom;
using NBi.Xml.Variables.Sequence;
using NBi.Extensibility.Resolving;
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
        private IDictionary<string, IVariable> Variables { get; set; } = new Dictionary<string, IVariable>();
        private SettingsXml.DefaultScope Scope { get; } = SettingsXml.DefaultScope.Everywhere;
        private ISequenceResolverArgs Args { get; set; } = null;
        private ColumnType columnType = ColumnType.Numeric;

        private ServiceLocator ServiceLocator { get; }

        public SequenceResolverArgsBuilder(ServiceLocator serviceLocator)
            => ServiceLocator = serviceLocator;

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

        public void Setup(IDictionary<string, IVariable> globalVariables)
        {
            this.Variables = globalVariables;
        }

        public void Build()
        {
            if (!isSetup)
                throw new InvalidOperationException();

            var helper = new ScalarHelper(ServiceLocator, new Context(Variables));
            switch (obj)
            {
                case SentinelLoopXml loop:
                    switch (columnType)
                    {
                        case ColumnType.Numeric:
                            Args = BuildSentinelLoopResolverArgs<decimal, decimal>(loop.Seed, loop.Terminal, loop.Step, loop.IntervalMode);
                            break;
                        case ColumnType.DateTime:
                            Args = BuildSentinelLoopResolverArgs<DateTime, IDuration>(loop.Seed, loop.Terminal, loop.Step, loop.IntervalMode);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case FileLoopXml loop:
                    Args = BuildFileLoopResolverArgs(loop.Path, loop.Pattern);
                    break;
                case QueryXml query:
                    var queryBuilder = new QueryResolverArgsBuilder(ServiceLocator);
                    queryBuilder.Setup(query, settings, Scope, Variables);
                    queryBuilder.Build();
                    Args = new QuerySequenceResolverArgs(queryBuilder.GetArgs());
                    break;
                case CustomXml obj:
                    Args = new CustomSequenceResolverArgs(
                            helper.InstantiateResolver<string>(obj.AssemblyPath),
                            helper.InstantiateResolver<string>(obj.TypeName),
                            obj.Parameters.Select(x => new { x.Name, ScalarResolver = (IScalarResolver)helper.InstantiateResolver<string>(x.StringValue) })
                            .ToDictionary(x => x.Name, y => y.ScalarResolver)
                        );
                    break;
                case List<string> list:
                    var resolvers = new List<IScalarResolver>();
                    foreach (var value in list)
                        resolvers.Add(helper.InstantiateResolver<string>(value));
                    Args = new ListSequenceResolverArgs(resolvers);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public ISequenceResolverArgs GetArgs() => Args ?? throw new InvalidOperationException();
        
        private ISequenceResolverArgs BuildSentinelLoopResolverArgs<T, U>(string seed, string terminal, string step, IntervalMode intervalMode)
        {
            var helper = new ScalarHelper(ServiceLocator, new Context(Variables));
            
            var args = new SentinelLoopSequenceResolverArgs<T, U>(
                    helper.InstantiateResolver<T>(seed).Execute(),
                    helper.InstantiateResolver<T>(terminal).Execute(),
                    helper.InstantiateResolver<U>(step).Execute(),
                    intervalMode
                );

            return args;
        }
        private ISequenceResolverArgs BuildFileLoopResolverArgs(string path, string pattern)
        {
            var helper = new ScalarHelper(ServiceLocator, new Context(Variables));

            var args = new FileLoopSequenceResolverArgs()
            {
                BasePath = settings.BasePath,
                Path = helper.InstantiateResolver<string>(path).Execute()
            };

            if (!string.IsNullOrEmpty(pattern))
                args.Filters.Add(new PatternRootFilter(helper.InstantiateResolver<string>(pattern).Execute()));

            return args;
        }
    }
}

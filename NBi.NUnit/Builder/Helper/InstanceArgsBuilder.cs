using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.Sequence.Resolver;
using NBi.Core.Transformation;
using NBi.Core.Variable;
using NBi.Core.Variable.Instantiation;
using NBi.Xml;
using NBi.Xml.Settings;
using NBi.Xml.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Builder.Helper
{
    public class InstanceArgsBuilder
    {
        private ServiceLocator ServiceLocator { get; }
        private IDictionary<string, ITestVariable> Variables { get; }

        private bool isSetup = false;
        private object obj = null;
        private SettingsXml settings = SettingsXml.Empty;
        private IInstanceArgs args = null;

        public InstanceArgsBuilder(ServiceLocator serviceLocator, IDictionary<string, ITestVariable> variables)
            => (ServiceLocator, Variables) = (serviceLocator, variables);

        public void Setup(SettingsXml settings)
        {
            this.settings = settings;
        }

        public void Setup(InstanceSettlingXml definition)
        {
            obj = definition;
            isSetup = true;
        }

        public void Build()
        {
            if (!isSetup)
                throw new InvalidOperationException();

            if (obj == InstanceSettlingXml.Unique)
                args = new DefaultInstanceArgs();

            else if ((obj as InstanceSettlingXml).Variable != null)
            {
                var variable = (obj as InstanceSettlingXml).Variable;

                var argsBuilder = new SequenceResolverArgsBuilder(ServiceLocator);
                argsBuilder.Setup(settings);
                argsBuilder.Setup(Variables);
                argsBuilder.Setup(variable.Type);

                if (variable.SentinelLoop != null)
                    argsBuilder.Setup(variable.SentinelLoop);
                else if (variable.FileLoop != null)
                    argsBuilder.Setup(variable.FileLoop);
                else if (variable.Items != null)
                    argsBuilder.Setup(variable.Items);
                else
                    throw new ArgumentOutOfRangeException();

                argsBuilder.Build();
                var factory = new SequenceResolverFactory(ServiceLocator);

                if (((obj as InstanceSettlingXml).DerivedVariables?.Count() ?? 0) == 0)
                {
                    args = new SingleVariableInstanceArgs()
                    {
                        Name = variable.Name,
                        Resolver = factory.Instantiate(variable.Type, argsBuilder.GetArgs()),
                        Categories = (obj as InstanceSettlingXml).Categories,
                        Traits = (obj as InstanceSettlingXml).Traits.ToDictionary(x => x.Name, x => x.Value),
                    };
                }
                else
                {
                    var derivationArgs = new Dictionary<string, DerivationArgs>();
                    foreach (var derivation in (obj as InstanceSettlingXml).DerivedVariables)
                    {
                        var transformerArgs = new TransformaterArgs() { Language = derivation.Script.Language, Code = derivation.Script.Code };
                        var transformerFactory = new TransformerFactory(ServiceLocator, new Context(Variables));
                        var transformer = transformerFactory.Instantiate(transformerArgs);
                        transformer.Initialize(derivation.Script.Code);
                        derivationArgs.Add(derivation.Name, new DerivationArgs() { Source = derivation.BasedOn, Transformer = transformer });
                    }

                    args = new DerivedVariableInstanceArgs()
                    {
                        Name = variable.Name,
                        Resolver = factory.Instantiate(variable.Type, argsBuilder.GetArgs()),
                        Derivations = derivationArgs,
                        Categories = (obj as InstanceSettlingXml).Categories,
                        Traits = (obj as InstanceSettlingXml).Traits.ToDictionary(x => x.Name, x => x.Value),
                    };
                }
            }
        }

        public IInstanceArgs GetArgs() => args ?? throw new InvalidOperationException();

        private class TransformaterArgs : ITransformationInfo
        {
            public ColumnType OriginalType { get; set; }
            public LanguageType Language { get; set; }
            public string Code { get; set; }
        }
    }
}

using NBi.Core.Calculation.Predicate;
using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.Sequence.Resolver;
using NBi.Core.Transformation;
using NBi.Core.Transformation.Transformer;
using NBi.Core.Variable;
using NBi.Core.Variable.Instantiation;
using NBi.Xml;
using NBi.Xml.Settings;
using NBi.Xml.Variables;
using NBi.Xml.Variables.Sequence;
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
                else if (variable.Custom != null)
                    argsBuilder.Setup(variable.Custom);
                else if (variable.Query != null)
                    argsBuilder.Setup(variable.Query);
                else if (variable.Items != null && variable.Items.Count>0)
                    argsBuilder.Setup(variable.Items);
                else
                    throw new ArgumentOutOfRangeException();

                argsBuilder.Build();
                var factory = new SequenceResolverFactory(ServiceLocator);
                var innerResolver = factory.Instantiate(variable.Type, argsBuilder.GetArgs());
                var sequenceResolver = BuildFilterSequenceResolver(variable.Type, innerResolver, variable.Filter);

                if (((obj as InstanceSettlingXml).DerivedVariables?.Count() ?? 0) == 0)
                {
                    args = new SingleVariableInstanceArgs()
                    {
                        Name = variable.Name,
                        Resolver = sequenceResolver,
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
                        Resolver = sequenceResolver,
                        Derivations = derivationArgs,
                        Categories = (obj as InstanceSettlingXml).Categories,
                        Traits = (obj as InstanceSettlingXml).Traits.ToDictionary(x => x.Name, x => x.Value),
                    };
                }
            }
        }

        private ISequenceResolver BuildFilterSequenceResolver(ColumnType type, ISequenceResolver resolver, FilterSequenceXml filterXml)
        {
            if (filterXml == null)
                return resolver;

            var predicateBuilder = new PredicateArgsBuilder(ServiceLocator, new Context(Variables));
            var predicateArgs = predicateBuilder.Execute(filterXml.Predication.ColumnType, filterXml.Predication.Predicate);
            var predicate = new PredicateFactory().Instantiate(predicateArgs);

            var operandTransformation = new TransformerFactory(ServiceLocator, new Context(Variables)).Instantiate
                (
                    new OperandTransformation
                    {
                        OriginalType = type,
                        Code = filterXml.Predication.Operand
                    }
                );
            operandTransformation.Initialize(filterXml.Predication.Operand);

            var factory = new SequenceResolverFactory(ServiceLocator);
            return factory.Instantiate(type, new FilterSequenceResolverArgs(resolver, predicate, operandTransformation));
        }

        private class OperandTransformation : ITransformationInfo
        {
            public ColumnType OriginalType { get ; set; }
            public LanguageType Language { get => LanguageType.Native; set => throw new NotImplementedException(); }
            public string Code { get; set; }
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

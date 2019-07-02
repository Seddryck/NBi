using NBi.Core.Calculation;
using NBi.Core.Calculation.Grouping;
using NBi.Core.Calculation.Predicate;
using NBi.Core.Calculation.Ranking;
using NBi.Core.Evaluate;
using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Alteration;
using NBi.Core.ResultSet.Alteration.Renaming;
using NBi.Core.ResultSet.Conversion;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Conversion;
using NBi.Core.Transformation;
using NBi.Core.Variable;
using NBi.Xml.Items.Calculation.Ranking;
using NBi.Xml.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Builder.Helper
{
    class ResultSetSystemHelper
    {
        private readonly ServiceLocator serviceLocator;
        private readonly IDictionary<string, ITestVariable> variables;

        public ResultSetSystemHelper(ServiceLocator serviceLocator, IDictionary<string, ITestVariable> variables)
        {
            this.serviceLocator = serviceLocator;
            this.variables = variables;
        }

        public IResultSetResolver InstantiateResolver(ResultSetSystemXml resultSetXml)
        {
            var argsBuilder = new ResultSetResolverArgsBuilder(serviceLocator);
            argsBuilder.Setup(resultSetXml);
            argsBuilder.Setup(resultSetXml.Settings);
            argsBuilder.Setup(variables);
            argsBuilder.Build();

            var factory = serviceLocator.GetResultSetResolverFactory();
            var resolver = factory.Instantiate(argsBuilder.GetArgs());
            return resolver;
        }


        public IEnumerable<Alter> InstantiateAlterations(ResultSetSystemXml resultSetXml)
        {
            if (resultSetXml.Alteration == null)
                yield break;

            if (resultSetXml.Alteration.Filters != null)
            {
                var factory = new ResultSetFilterFactory(variables);
                foreach (var filterXml in resultSetXml.Alteration.Filters)
                {
                    if (filterXml.Ranking == null)
                    {
                        var expressions = new List<IColumnExpression>();
                        if (filterXml.Expression != null)
                            expressions.Add(filterXml.Expression);
                        
                        if (filterXml.Predication != null)
                        {
                            var helper = new PredicateArgsBuilder(serviceLocator, variables);
                            var args = helper.Execute(filterXml.Predication.ColumnType, filterXml.Predication.Predicate);

                            yield return factory.Instantiate
                                        (
                                            filterXml.Aliases
                                            , expressions
                                            , new PredicationArgs(filterXml.Predication.Operand, args)
                                        ).Apply;
                        }
                        if (filterXml.Combination != null)
                        {
                            var helper = new PredicateArgsBuilder(serviceLocator, variables);
                            var predicationArgs = new List<PredicationArgs>();
                            foreach (var predication in filterXml.Combination.Predications)
                            {
                                var args = helper.Execute(predication.ColumnType, predication.Predicate);
                                predicationArgs.Add(new PredicationArgs(predication.Operand, args));
                            }

                            yield return factory.Instantiate
                                        (
                                            filterXml.Aliases
                                            , expressions
                                            , filterXml.Combination.Operator
                                            , predicationArgs
                                        ).Apply;
                        }
                    }
                    else
                    {
                        yield return factory.Instantiate(
                            filterXml.Ranking, 
                            filterXml.Ranking?.GroupBy?.Columns
                            ).Apply;
                    }
                }
            }

            if (resultSetXml.Alteration.Conversions != null)
            {
                foreach (var conversionXml in resultSetXml.Alteration.Conversions)
                {
                    var factory = new ConverterFactory();
                    var converter = factory.Instantiate(conversionXml.Converter.From, conversionXml.Converter.To, conversionXml.Converter.DefaultValue, conversionXml.Converter.Culture);
                    var engine = new ConverterEngine(conversionXml.Column, converter);
                    yield return engine.Execute;
                }
            }

            if (resultSetXml.Alteration.Transformations != null)
            {
                var identifierFactory = new ColumnIdentifierFactory();

                var provider = new TransformationProvider();
                foreach (var transformationXml in resultSetXml.Alteration.Transformations)
                    provider.Add(transformationXml.Identifier, transformationXml);
                yield return provider.Transform;
            }

            if (resultSetXml.Alteration.Renamings != null)
            {
                foreach (var renameXml in resultSetXml.Alteration.Renamings)
                {
                    var helper = new ScalarHelper(serviceLocator, variables);
                    var newName = helper.InstantiateResolver<string>(renameXml.NewName);

                    var factory = new RenamingFactory();
                    var renamer = factory.Instantiate(new NewNameRenamingArgs(renameXml.Identifier, newName));
                    yield return renamer.Execute;
                }
            }
        }
    }
}

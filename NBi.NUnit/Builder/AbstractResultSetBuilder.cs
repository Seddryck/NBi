using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Loading;
using NBi.Core;
using System.Diagnostics;
using System.IO;
using NBi.Core.ResultSet.Resolver.Query;
using NBi.Core.ResultSet.Alteration;
using NBi.Core.Evaluate;
using NBi.Core.Calculation;

namespace NBi.NUnit.Builder
{
    abstract class AbstractResultSetBuilder : AbstractTestCaseBuilder
    {
        protected AbstractSystemUnderTestXml SystemUnderTestXml { get; set; }

        protected override void BaseSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(sutXml is ExecutionXml || sutXml is ResultSetSystemXml))
                throw new ArgumentException("System-under-test must be a 'ExecutionXml' or 'ResultSetXml'");

            SystemUnderTestXml = sutXml;
        }

        protected override void BaseBuild()
        {
            if (SystemUnderTestXml is ExecutionXml)
                SystemUnderTest = InstantiateSystemUnderTest((ExecutionXml)SystemUnderTestXml);
            else
                SystemUnderTest = InstantiateSystemUnderTest((ResultSetSystemXml)SystemUnderTestXml);
        }

        protected virtual IResultSetService InstantiateSystemUnderTest(ExecutionXml executionXml)
        {
            var commandBuilder = new CommandBuilder();

            var connectionString = executionXml.Item.GetConnectionString();
            var commandText = (executionXml.Item as QueryableXml).GetQuery();

            IEnumerable<IQueryParameter> parameters=null;
            IEnumerable<IQueryTemplateVariable> variables = null;
            int timeout = 0;
            if (executionXml.BaseItem is QueryXml)
            { 
                parameters = ((QueryXml)executionXml.BaseItem).GetParameters();
                variables = ((QueryXml)executionXml.BaseItem).GetVariables();
                timeout = ((QueryXml)executionXml.BaseItem).Timeout;
            }
            if (executionXml.BaseItem is ReportXml)
            {
                parameters = ((ReportXml)executionXml.BaseItem).GetParameters();
            }
            var cmd = commandBuilder.Build(connectionString, commandText, parameters, variables, timeout);

            if (executionXml.BaseItem is ReportXml)
            {
                cmd.CommandType = ((ReportXml)executionXml.BaseItem).GetCommandType();
            }

            var args = new DbCommandQueryResolverArgs(cmd);

            var factory = new ResultSetLoaderFactory();
            var loader = factory.Instantiate(args);

            var builder = new ResultSetServiceBuilder();
            builder.Setup(loader);
            var service = builder.GetService();

            return service;
        }

        protected virtual object InstantiateSystemUnderTest(ResultSetSystemXml resultSetXml)
        {
            var builder = new ResultSetServiceBuilder();
            builder.Setup(InstantiateLoader(resultSetXml));
            builder.Setup(InstantiateAlterations(resultSetXml));
            return builder.GetService();
        }

        protected virtual IResultSetLoader InstantiateLoader(ResultSetSystemXml resultSetXml)
        {
            var factory = new ResultSetLoaderFactory();
            IResultSetLoader loader;

            //ResultSet (external file)
            if (!string.IsNullOrEmpty(resultSetXml.File))
            {
                Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, "ResultSet defined in external file!");
                var file = string.Empty;
                if (Path.IsPathRooted(resultSetXml.File))
                    file = resultSetXml.File;
                else
                    file = resultSetXml.Settings?.BasePath + resultSetXml.File;

                factory.Using(resultSetXml?.Settings?.CsvProfile);
                loader = factory.Instantiate(file);
            }
            //Query
            else if (resultSetXml.Query != null )
            {
                var argsBuilder = new Helper.QueryResolverArgsBuilder();
                argsBuilder.Setup(resultSetXml.Query);
                argsBuilder.Setup(resultSetXml.Settings);
                argsBuilder.Build();
                var args = argsBuilder.GetArgs();
                
                loader = factory.Instantiate(args);
            }
            //ResultSet (embedded)
            else if (resultSetXml.Rows != null)
            {
                Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, "ResultSet defined in embedded resultSet!");
                loader = factory.Instantiate(resultSetXml.Content);
            }
            else
                throw new ArgumentException();

            return loader;
        }

        private IEnumerable<Alter> InstantiateAlterations(ResultSetSystemXml resultSetXml)
        {
            if (resultSetXml.Alteration == null)
                yield break;

            if (resultSetXml.Alteration.Filters != null)
            {
                foreach (var filterXml in resultSetXml.Alteration.Filters)
                {
                    var expressions = new List<IColumnExpression>();
                    if (filterXml.Expression != null)
                        expressions.Add(filterXml.Expression);

                    var factory = new PredicateFilterFactory();
                    if (filterXml.Predicate!=null)
                        yield return factory.Instantiate
                                    (
                                        filterXml.Aliases
                                        , expressions
                                        , filterXml.Predicate
                                    ).Apply;
                    if (filterXml.Combination != null)
                        yield return factory.Instantiate
                                    (
                                        filterXml.Aliases
                                        , expressions
                                        , filterXml.Combination.Operator
                                        , filterXml.Combination.Predicates
                                    ).Apply;
                }
            }
        }

    }
}

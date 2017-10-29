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

            var factory = new ResultSetLoaderFactory();
            var loader = factory.Instantiate(cmd);

            var builder = new ResultSetServiceBuilder
            {
                Loader = loader
            };
            var service = builder.GetService();

            return service;
        }

        protected virtual object InstantiateSystemUnderTest(ResultSetSystemXml resultSetXml)
        {
            var factory = new ResultSetLoaderFactory();
            IResultSetLoader loader;

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
            else if (resultSetXml.Rows != null)
            {
                Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, "ResultSet defined in embedded resultSet!");
                loader = factory.Instantiate(resultSetXml.Content);
            }
            else
                throw new ArgumentException();

            var builder = new ResultSetServiceBuilder() { Loader = loader };
            return builder.GetService();
        }

    }
}

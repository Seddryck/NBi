using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NBi.Core;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NBi.Core.ResultSet.Loading;
using System.IO;
using NBi.Core.ResultSet;

namespace NBi.NUnit.Builder
{
    class ResultSetSystemEqualToBuilder : ResultSetEqualToBuilder
    {
        public ResultSetSystemEqualToBuilder()
        {

        }

        protected override void BaseSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(sutXml is ResultSetSystemXml))
                throw new ArgumentException("System-under-test must be a 'ResultSetSystemXml'");

            SystemUnderTestXml = (ResultSetSystemXml)sutXml;
        }
        protected override void BaseBuild()
        {
            SystemUnderTest = InstantiateSystemUnderTest((ResultSetSystemXml)SystemUnderTestXml);
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

                factory.Using(resultSetXml.Settings.CsvProfile);
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

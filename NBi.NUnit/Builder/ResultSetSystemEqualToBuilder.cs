using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NBi.Core;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NBi.Core.ResultSet.Loading;
using System.IO;

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
            var factory = new ResultSetServiceFactory();

            if (!string.IsNullOrEmpty(resultSetXml.File))
            {
                Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, "ResultSet defined in external file!");
                var file = string.Empty;
                if (Path.IsPathRooted(resultSetXml.File))
                    file = resultSetXml.File;
                else
                    file = resultSetXml.Settings?.BasePath + resultSetXml.File;

                if (resultSetXml.Settings.CsvProfile != null)
                    return factory.Instantiate(file, resultSetXml?.Settings?.CsvProfile);
            }
            else if (resultSetXml.Rows != null)
            {
                Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, "ResultSet defined in embedded resultSet!");
                return factory.Instantiate(resultSetXml.Content, null);
            }
            throw new ArgumentException();
        }

    }
}

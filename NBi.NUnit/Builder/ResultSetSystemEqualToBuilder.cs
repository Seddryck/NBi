using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NBi.Core;
using NBi.Core.Query;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Comparer;
using NBi.NUnit.ResultSetComparison;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NBi.Core.Xml;
using NBi.Core.Transformation;
using System.Data;

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
            if (!string.IsNullOrEmpty(resultSetXml.File))
            {
                Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, "ResultSet defined in external file!");
                return resultSetXml.GetFile();
                //if (resultSetXml.Settings.CsvProfile != null)
                    //ctr = ctr.CsvProfile(resultSetXml.Settings.CsvProfile);
            }
            else if (resultSetXml.Rows != null)
            {
                Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, "ResultSet defined in embedded resultSet!");
                return resultSetXml.Content;
            }
            throw new ArgumentException();
        }

    }
}

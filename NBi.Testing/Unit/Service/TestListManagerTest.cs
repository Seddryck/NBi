using NBi.Service;
using NBi.Xml.Constraints;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Service
{
    public class TestListManagerTest
    {
        [Test]
        public void Build_NewRow_CorrectTemplate()
        {
            var manager = new TestListManager();
            
            //Setup template
            var template = "<test name=\"testName\"><system-under-test><execution><query>select top 1 [$field$] from [table]</query></execution></system-under-test><assert><matchPattern><numeric-format decimal-digits=\"5\"/></matchPattern></assert></test>";
            var variables = new[] { "field" };
            var caseSet = new DataTable();
            caseSet.Columns.Add(new DataColumn("field"));
            var row = caseSet.NewRow();
            row[0] = "myColumn";
            caseSet.Rows.Add(row);

            manager.Build(template, variables, caseSet, false);

            var test = manager.Tests[0];

            Assert.That(test.Constraints[0], Is.TypeOf<MatchPatternXml>());
            var matchPattern = test.Constraints[0] as MatchPatternXml;
            Assert.That(matchPattern.NumericFormat.DecimalDigits, Is.EqualTo(5));
            Assert.That(test.Content, Is.StringContaining("<numeric-format"));
        }
    }
}

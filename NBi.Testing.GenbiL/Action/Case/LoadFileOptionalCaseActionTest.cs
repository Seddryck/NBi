using NBi.GenbiL;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Stateful;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Testing.Action.Case;

public class LoadFileOptionalCaseActionTest
{
    public class LoadOptionalCaseFromFileActionTestable : LoadOptionalCaseFromFileAction
    {
        public LoadOptionalCaseFromFileActionTestable(string filename, IEnumerable<string> columnNames)
            : base(filename, columnNames) { }

        protected override bool IsExistingFile() => false;
    }
        
    [Test]
    public void Execute_FileMissing_EmptyDataSetWithExpectedColumns()
    {
        var state = new GenerationState();
        var action = new LoadOptionalCaseFromFileActionTestable("file.csv", ["foo", "bar"]);
        action.Execute(state);

        var caseSet = state.CaseCollection.First().Value;
        Assert.That(caseSet.Content.Rows, Has.Count.EqualTo(0));
        Assert.That(caseSet.Content.Columns, Has.Count.EqualTo(2));
        Assert.That(caseSet.Content.Columns.Cast<DataColumn>().Select(x => x.ColumnName), Does.Contain("foo"));
        Assert.That(caseSet.Content.Columns.Cast<DataColumn>().Select(x => x.ColumnName), Does.Contain("bar"));
    }
}

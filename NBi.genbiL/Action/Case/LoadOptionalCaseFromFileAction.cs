using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using NBi.Core.FlatFile;
using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Case;

public class LoadOptionalCaseFromFileAction : LoadCaseFromFileAction
{
    public IEnumerable<string> ColumnNames { get; set; }

    public LoadOptionalCaseFromFileAction(string filename, IEnumerable<string> columnNames)
        : base(filename) => ColumnNames = columnNames;

    public override void Execute(CaseSet testCases)
    {
        if (IsExistingFile())
            base.Execute(testCases);
        else
        {
            testCases.Content = new DataTable();
            foreach (var columnName in ColumnNames)
                testCases.Content.Columns.Add(new DataColumn(columnName, typeof(object)));
            testCases.Content.AcceptChanges();
        }
    }

    protected virtual bool IsExistingFile() => File.Exists(Filename);

    public override string Display => $"Loading test-cases from CSV file '{Filename}', if missing create empty test-case with following columns {string.Join(", ", ColumnNames)}.";
}

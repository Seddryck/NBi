using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Case
{
    public class FilterDistinctCaseAction: ICaseAction
    {

        public FilterDistinctCaseAction()
        { }

        public void Execute(GenerationState state)
        {
            DataTableReader dataReader = null;

            var content = state.TestCaseCollection.Scope.Content;
            var distinctRows = content.AsEnumerable().Distinct(DataRowComparer.Default);

            if (distinctRows.Count() > 0)
            {
                var distinctTable = distinctRows.CopyToDataTable();
                dataReader = distinctTable.CreateDataReader();
            }
            content.Clear();

            if (dataReader != null)
                content.Load(dataReader, LoadOption.PreserveChanges);
            content.AcceptChanges();
        }

        public virtual string Display { get => $"Filtering distinct cases."; }
    }
}

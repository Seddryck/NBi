using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Case
{
    class FilterDistinctCaseAction: ICaseAction
    {

        public FilterDistinctCaseAction()
        {
        }

        public void Execute(GenerationState state)
        {
            var content = state.TestCaseSetCollection.Scope.Content;
            DataTableReader dataReader = null;
            var distinctRows = content.AsEnumerable().Distinct(System.Data.DataRowComparer.Default);

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

        public virtual string Display
        {
            get
            {
                return string.Format("Filtering distinct cases.");
            }
        }
    }
}

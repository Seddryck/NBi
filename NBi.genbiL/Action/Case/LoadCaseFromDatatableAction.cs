using System;
using System.Linq;
using NBi.GenbiL.Stateful;
using System.Data;

namespace NBi.GenbiL.Action.Case
{
    public class LoadCaseFromDatatableAction : ICaseAction
    {
        public DataTable Datatable { get; set; }
        public LoadCaseFromDatatableAction(DataTable datatable)
        {
            Datatable = datatable;
        }

        public virtual void Execute(GenerationState state)
        {
            state.TestCaseCollection.Scope.ReadFromDataTable(Datatable);
            state.TestCaseCollection.Scope.Content.AcceptChanges();
        }

        public string Display
        {
            get
            {
                return "Loading TestCases from datatable. ";
            }
        }
    }
}
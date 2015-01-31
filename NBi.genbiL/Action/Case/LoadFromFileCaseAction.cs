using System;
using System.Linq;
using NBi.Service;
using NBi.GenbiL.Stateful;
using NBi.Core;
using System.Data;

namespace NBi.GenbiL.Action.Case
{
    public class LoadFromFileCaseAction : ICaseAction
    {
        public string Filename { get; set; }
        public LoadFromFileCaseAction(string filename)
        {
            Filename = filename;
        }

        public virtual void Execute(GenerationState state)
        {
            var csvReader = new CsvReader();
            var dataTable = csvReader.Read(Filename, true);
            var dataReader = dataTable.CreateDataReader();

            state.TestCaseSetCollection.Scope.Content.Reset();
            state.TestCaseSetCollection.Scope.Content.Load(dataReader, LoadOption.PreserveChanges);
            state.TestCaseSetCollection.Scope.Content.AcceptChanges();
        }

        public string Display
        {
            get
            {
                return string.Format("Loading TestCases from CSV file '{0}'"
                    , Filename);
            }
        }
       

    }
}

using System;
using System.Linq;
using NBi.Service;
using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Case
{
    public class LoadCaseFromQueryFileAction : ICaseAction
    {
        public string Filename { get; set; }
        public string ConnectionString { get; set; }

        public LoadCaseFromQueryFileAction(string filename, string connectionString)
        {
            Filename = filename;
            ConnectionString = connectionString;
        }

        public virtual void Execute(GenerationState state)
        {
            state.TestCaseCollection.Scope.ReadFromQueryFile(Filename, ConnectionString);
        }

        public string Display
        {
            get
            {
                return string.Format("Loading TestCases from query written in '{0}'"
                    , Filename);
            }
        }
       

    }
}

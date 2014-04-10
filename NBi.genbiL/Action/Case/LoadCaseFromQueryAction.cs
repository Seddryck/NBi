using System;
using System.Linq;
using NBi.Service;

namespace NBi.GenbiL.Action.Case
{
    public class LoadCaseFromQueryAction : ICaseAction
    {
        public string Filename { get; set; }
        public string ConnectionString { get; set; }

        public LoadCaseFromQueryAction(string filename, string connectionString)
        {
            Filename = filename;
            ConnectionString = connectionString;
        }

        public virtual void Execute(GenerationState state)
        {
            state.TestCases.ReadFromQueryFile(Filename, ConnectionString);
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

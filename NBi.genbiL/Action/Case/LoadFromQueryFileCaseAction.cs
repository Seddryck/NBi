using System;
using System.Linq;
using NBi.Service;
using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Case
{
    public class LoadFromQueryFileCaseAction : LoadFromQueryCaseAction
    {
        public string Filename { get; set; }

        public LoadFromQueryFileCaseAction(string filename, string connectionString)
            : base(string.Empty, connectionString)
        {
            Filename = filename;
        }

        protected virtual string GetQuery()
        {
            return System.IO.File.ReadAllText(Filename);
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

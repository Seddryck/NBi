using System;
using System.IO;
using NBi.Core.Query;
using NBi.Core.ResultSet;

namespace NBi.Core
{
    public class QuerySetManager : IProgressStatusAware
    {
        public event ProgressStatusHandler ProgressStatusChanged;

        public string DirectoryQueries { get; private set; }
        public string DirectoryResultSets { get; private set; }
        public string ConnectionString { get; private set; }
        public string Pattern { get; private set; }

        public QueryOleDbEngine Executor { get; private set; }
        public ResultSetAbstractWriter ResultSetWriter { get; private set; }

        public static QuerySetManager BuildDefault(string directoryQueries, string directoryResultSets, string connectionString)
        {
            var qsm = new QuerySetManager(directoryQueries, "*.mdx", directoryResultSets, connectionString);
            qsm.Executor = new QueryOleDbEngine(connectionString);
            qsm.ResultSetWriter = new ResultSetCsvWriter(directoryResultSets);
            return qsm;
        }

        protected internal QuerySetManager(string directoryQueries, string pattern, string directoryResultSets, string connectionString)
        {
            DirectoryQueries = directoryQueries;
            Pattern = pattern;
            DirectoryResultSets = directoryResultSets;
            ConnectionString = connectionString;
        }

        public void PersistResultSets()
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs(string.Format("Parsing directory {0}", DirectoryQueries)));

            var files = Directory.GetFiles(DirectoryQueries, Pattern);
            var i = 1;

            foreach (var file in files)
	        {
                i++;
                
                if (ProgressStatusChanged != null)
                    ProgressStatusChanged(this, new ProgressStatusEventArgs(String.Format("Executing query {0} of {1}", i, files.Length+1), i, files.Length+1));
                
                var query = File.ReadAllText(file);

                var ds = Executor.Execute(query);

                if (ProgressStatusChanged != null)
                    ProgressStatusChanged(this, new ProgressStatusEventArgs(String.Format("Persisting results set for {0} of {1}", i, files.Length+1), i, files.Length+1));
                ResultSetWriter.Write(Path.GetFileNameWithoutExtension(file) + String.Format(".csv"), ds);
	        }

            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs(string.Format("Directory {0} parsed", DirectoryQueries)));
            
        }


    }
}

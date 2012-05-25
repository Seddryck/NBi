using System.IO;
using NBi.Core.Query;
using NBi.Core.ResultSet;

namespace NBi.Core
{
    public class ResultSetManager
    {
        protected IResultSetWriter _resultSetWriter;

        public string ConnectionString { get; private set; }

        protected internal ResultSetManager(IResultSetWriter resultSetWriter, string connectionString)
        {
            _resultSetWriter = resultSetWriter;
            ConnectionString = connectionString;
        }

        public static ResultSetManager Instantiate(string resultSetDirectory, string connectionString)
        {
            return new ResultSetManager(new ResultSetCsvWriter(resultSetDirectory), connectionString);
        }
        
        public void CreateResultSet(string queriesDirectory)
        {
            var queryFiles = System.IO.Directory.EnumerateFiles(queriesDirectory);

            if(!Directory.Exists(_resultSetWriter.PersistencePath))
                Directory.CreateDirectory(_resultSetWriter.PersistencePath);

            foreach (var queryFile in queryFiles)
            {
                string query;
                using (StreamReader infile = new StreamReader(Path.Combine(queriesDirectory, queryFile)))
                {
                    query=infile.ReadToEnd();
                }

                var qe = new QueryEngineFactory().GetExecutor(query, ConnectionString);

                var ds = qe.Execute();

                var resultFile = Path.GetFileName(Path.ChangeExtension(queryFile,"csv"));
                _resultSetWriter.Write(resultFile, ds);
            }
        }
    }
}

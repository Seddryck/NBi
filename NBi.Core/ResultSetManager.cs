using System.IO;
using NBi.Core.Query;
using NBi.Core.ResultSet;

namespace NBi.Core
{
    public class ResultSetManager
    {
        protected IResultSetWriter _resultSetWriter;
        protected IQueryExecutor _queryExecutor;

        protected internal ResultSetManager(IResultSetWriter resultSetWriter, IQueryExecutor queryExecutor)
        {
            _resultSetWriter = resultSetWriter;
            _queryExecutor= queryExecutor;
        }

        public static ResultSetManager Instantiate(string resultSetDirectory, string connectionString)
        {
            return new ResultSetManager(
                new ResultSetCsvWriter(resultSetDirectory),
                new QueryOleDbEngine(connectionString));
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

                var ds = _queryExecutor.Execute(query);

                var resultFile = Path.GetFileName(Path.ChangeExtension(queryFile,"csv"));
                _resultSetWriter.Write(resultFile, ds);
            }
        }
    }
}

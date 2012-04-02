using System.IO;
using NBi.Core.Analysis.Query;

namespace NBi.QueryGenerator
{
    public class BatchManager
    {
        public void CreateResultSet(string queriesDirectory, string resultSetDirectory, string connectionString)
        {
            var queryFiles = System.IO.Directory.EnumerateFiles(queriesDirectory);

            if(!Directory.Exists(resultSetDirectory))
                Directory.CreateDirectory(resultSetDirectory);

            var writer = new CsvResultSetWriter(resultSetDirectory);
            var exec = new OleDbExecutor(connectionString);

            foreach (var queryFile in queryFiles)
            {
                string query;
                using (StreamReader infile = new StreamReader(Path.Combine(queriesDirectory, queryFile)))
                {
                    query=infile.ReadToEnd();
                }

                var ds = exec.Execute(query);

                var resultFile = Path.Combine(resultSetDirectory, Path.GetFileName(Path.ChangeExtension(queryFile,"csv")));
                writer.Write(resultFile, ds);
                

            }
        }
    }
}

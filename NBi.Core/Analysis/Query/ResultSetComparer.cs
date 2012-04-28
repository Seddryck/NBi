using System.Data;
using System.IO;

namespace NBi.Core.Analysis.Query
{
    public class ResultSetComparer: IResultSetComparer
    {
        protected string _expectedResultSetPath;

        public ResultSetComparer(string expectedResultSetPath)
        {
            _expectedResultSetPath = expectedResultSetPath;
        }

        public Result Validate(IDbCommand cmd)
        {
            string actual =null;
            string expected = null;

            var exec = new OleDbExecutor(cmd.Connection.ConnectionString);
            
            var ds = exec.Execute(cmd.CommandText);
            var csvWriter = new ResultSetCsvWriter("");
            actual = csvWriter.BuildContent(ds.Tables[0]);

            var csvReader = new ResultSetCsvReader(Path.GetDirectoryName(_expectedResultSetPath));
            expected = csvReader.Read(Path.GetFileName(_expectedResultSetPath));              
            

            if (actual == expected)
                return Result.Success();
            else
                return Result.Failed();
        }

        
    }
}

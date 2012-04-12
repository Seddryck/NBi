using System;
using System.IO;

namespace NBi.Core.Analysis.Query
{
    public class ResultSetComparer: IResultSetComparer
    {
        protected string _connectionString;
        protected string _expectedResultSetPath;

        public ResultSetComparer(string connectionString, string expectedResultSetPath)
        {
            _connectionString = connectionString;
            _expectedResultSetPath = expectedResultSetPath;
        }
        
        public Result Validate(string mdxQuery)
        {
            string actual =null;
            string expected = null;

            var exec = new OleDbExecutor(_connectionString);
            
            var ds = exec.Execute(mdxQuery);
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

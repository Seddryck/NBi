using System.Data;
using System.IO;
using NBi.Core.Query;

namespace NBi.Core.ResultSet
{
    public class ResultSetComparer: IResultSetComparer
    {
        protected string _expectedResultSetPath;
        protected IDbCommand _expectedResultSetCommand;
        protected string _expectedResultSetPathPersistance;
        protected string _expectedResultSetFilenamePersistance;

        public delegate string GetExpectedResultSetMethod();
        protected internal GetExpectedResultSetMethod _getExpectedResultSet;

        public ResultSetComparer(string expectedResultSetPath)
        {
            _expectedResultSetPath = expectedResultSetPath;
            //set the moethod to call
            _getExpectedResultSet = GetExpectedResultSetWithFile; 
        }

        public ResultSetComparer(IDbCommand expectedResultSetCommand, string expectedResultSetPathPersistance, string expectedResultSetFilenamePersistance)
        {
            _expectedResultSetCommand = expectedResultSetCommand;
            _expectedResultSetPathPersistance = expectedResultSetPathPersistance;
            _expectedResultSetFilenamePersistance = expectedResultSetFilenamePersistance;
            //set the moethod to call
            _getExpectedResultSet = GetExpectedResultSetWithCommand;
        }

        public Result Validate(IDbCommand cmd)
        {
            string actual =null;
            string expected = null;

            actual = GetActualResultSet(cmd);

            expected = _getExpectedResultSet.Invoke();              
            
            if (actual == expected)
                return Result.Success();
            else
                return Result.Failed();
        }

        protected internal virtual string GetActualResultSet(IDbCommand cmd)
        {
            var exec = (IQueryExecutor) QueryEngineFactory.Get(cmd);
              
            var ds = exec.Execute(cmd.CommandText);
            var csvWriter = new ResultSetCsvWriter("");
            var actual = csvWriter.BuildContent(ds.Tables[0]);
            return actual;
        }

        protected internal virtual string GetExpectedResultSetWithCommand()
        {
            var exec = (IQueryExecutor) QueryEngineFactory.Get(_expectedResultSetCommand);

            var ds = exec.Execute(_expectedResultSetCommand.CommandText);
            var csvWriter = new ResultSetCsvWriter(_expectedResultSetPathPersistance);
            var expected = csvWriter.BuildContent(ds.Tables[0]);
            if (!string.IsNullOrEmpty(_expectedResultSetFilenamePersistance))
                csvWriter.Write(_expectedResultSetFilenamePersistance, ds);
            return expected;
        }

        protected internal virtual string GetExpectedResultSetWithFile()
        {
            var csvReader = new ResultSetCsvReader(Path.GetDirectoryName(_expectedResultSetPath));
            var expected = csvReader.Read(Path.GetFileName(_expectedResultSetPath));
            return expected;
        }

        
    }
}

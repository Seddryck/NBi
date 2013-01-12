using System;
using System.Data;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Core.Query
{
    public class QueryAdomdEngine:IQueryExecutor, IQueryEnginable, IQueryParser, IQueryPerformance
    {
        protected readonly AdomdCommand _command;


        public QueryAdomdEngine(AdomdCommand cmd)
        {
            _command = cmd;
        }

        public void CleanCache()
        {
            throw new NotImplementedException("Hé man what's the goal to clean the cache for an MDX query?");
        }

        public virtual PerformanceResult CheckPerformance()
        {
            DateTime tsStart, tsStop;

            if (_command.Connection.State == ConnectionState.Closed)
                _command.Connection.Open();

            tsStart = DateTime.Now;
            _command.ExecuteNonQuery();
            tsStop = DateTime.Now;

            if (_command.Connection.State == ConnectionState.Open)
                _command.Connection.Close();

            return new PerformanceResult(tsStop.Subtract(tsStart));
        }

        public virtual DataSet Execute()
        {
            int i;
            return Execute(out i);
        }

        public virtual DataSet Execute(out int elapsedSec)
        {
            // Open the connection
            using (var connection = new AdomdConnection())
            {
                var connectionString = _command.Connection.ConnectionString;
                try
                    { connection.ConnectionString = connectionString; }
                catch (ArgumentException ex)
                { throw new ConnectionException(ex, connectionString); }
                //TODO
                //try
                //    {connection.Open();}
                //catch (AdomdException ex)
                //    {throw new ConnectionException(ex);}

                // capture time before execution
                long ticksBefore = DateTime.Now.Ticks;
                var adapter = new AdomdDataAdapter(_command.CommandText, connection);
                var ds = new DataSet();
                
                adapter.SelectCommand.CommandTimeout = 0;
                try
                {
                    adapter.Fill(ds);
                }
                catch (AdomdConnectionException ex)
                {
                    throw new ConnectionException(ex, connectionString);
                }

                // capture time after execution
                long ticksAfter = DateTime.Now.Ticks;

                // setting query runtime
                elapsedSec = Convert.ToInt32((ticksAfter - ticksBefore) / 1000 / 1000);

                return ds;
            }
        }

        public virtual ParserResult Parse()
        {
            ParserResult res = null;

            using (var connection = new AdomdConnection())
            {
                var connectionString = _command.Connection.ConnectionString;
                try
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                }
                catch (ArgumentException ex)
                { throw new ConnectionException(ex, connectionString); }
                
                using (AdomdCommand cmdIn = new AdomdCommand(_command.CommandText, connection))
                {
                    try
                    {
                        cmdIn.ExecuteReader(CommandBehavior.SchemaOnly);
                        res = ParserResult.NoParsingError();
                    }
                    catch (AdomdException ex)
                    {
                        res = new ParserResult(ex.Message.Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries));
                    }

                }

                if (connection.State != System.Data.ConnectionState.Closed)
                    connection.Close();
            }

            return res;
        }
    }
}

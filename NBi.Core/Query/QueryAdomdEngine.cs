using System;
using System.Data;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Core.Query
{
    public class QueryAdomdEngine:IQueryExecutor, IQueryEnginable
    {
        protected readonly AdomdCommand _command;


        public QueryAdomdEngine(AdomdCommand cmd)
        {
            _command = cmd;
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
                try
                    { connection.ConnectionString = _command.Connection.ConnectionString; }
                catch (ArgumentException ex)
                    {throw new ConnectionException(ex);}
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
                adapter.Fill(ds);

                // capture time after execution
                long ticksAfter = DateTime.Now.Ticks;

                // setting query runtime
                elapsedSec = Convert.ToInt32((ticksAfter - ticksBefore) / 1000 / 1000);

                return ds;
            }
        }
    }
}

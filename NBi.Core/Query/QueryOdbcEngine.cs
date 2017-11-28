using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;

namespace NBi.Core.Query
{
    /// <summary>
    /// Engine wrapping the System.Data.Odbc namespace for execution of NBi tests
    /// <remarks>Instances of this class are built by the means of the <see>QueryEngineFactory</see></remarks>
    /// </summary>
    internal class QueryOdbcEngine: IQueryEnginable, IQueryPerformance, IQueryParser
    {
        protected readonly OdbcCommand command;


        protected internal QueryOdbcEngine(OdbcCommand command)
        {
            this.command = command;
        }

        /// <summary>
        /// Method exposed by the interface IQueryPerformance to execute a test of syntax
        /// </summary>
        /// <remarks>This method makes usage the set statement named SET FMTONLY to not effectively execute the query but check the validity of this query</remarks>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public virtual ParserResult Parse()
        {
            ParserResult res=null;
            
            using(var conn = new OdbcConnection(command.Connection.ConnectionString))
            {
                var fullSql = string.Format(@"SET FMTONLY ON {0} SET FMTONLY OFF", command.CommandText);
                
                conn.Open();

                using (var cmdIn = new OdbcCommand(fullSql, conn))
                {
                    try 
	                {
                        cmdIn.ExecuteNonQuery();
                        res = ParserResult.NoParsingError();
	                }
	                catch (OdbcException ex)
	                {
                        var exList = new List<string>(ex.Message.Split(new string[] { "[SQL Server]", "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries));
                        var exArray = exList.Where(s => !s.StartsWith("ERROR [")).ToArray();
                        res = new ParserResult(exArray);
	                }
                    
                }

                if (conn.State != System.Data.ConnectionState.Closed)
                conn.Close();
            }

            return res;
        }

        /// <summary>
        /// Method exposed by the interface IQueryPerformance to clean the cache of a SQL Database
        /// <remarks>Makes usage of the command dbcc freeproccache and dbcc dropcleanbuffers</remarks>
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public virtual void CleanCache()
        {
            using (var conn = new OdbcConnection(command.Connection.ConnectionString))
            {
                var clearSql = new string[] { "dbcc freeproccache", "dbcc dropcleanbuffers" };

                conn.Open();

                foreach (var sql in clearSql)
                {
                    using (var cleanCmd = new OdbcCommand(sql, conn))
                    {
                        cleanCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public PerformanceResult CheckPerformance()
        {
            return CheckPerformance(0);
        }

        public PerformanceResult CheckPerformance(int timeout)
        {
            bool isTimeout = false;
            DateTime tsStart, tsStop = DateTime.Now;

            if (command.Connection.State == ConnectionState.Closed)
                command.Connection.Open();

            Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, command.CommandText);
            foreach (OdbcParameter param in command.Parameters)
                Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, string.Format("{0} => {1}", param.ParameterName, param.Value));

            command.CommandTimeout = timeout / 1000;

            tsStart = DateTime.Now;
            try
            {
                command.ExecuteNonQuery();
                tsStop = DateTime.Now;
            }
            catch (OdbcException e)
            {
                if (!e.Message.EndsWith("Query timeout expired"))
                    throw;
                isTimeout = true;
            }

            if (command.Connection.State == ConnectionState.Open)
                command.Connection.Close();

            if (isTimeout)
                return PerformanceResult.Timeout(timeout);
            else
                return new PerformanceResult(tsStop.Subtract(tsStart));
        }
    }
}

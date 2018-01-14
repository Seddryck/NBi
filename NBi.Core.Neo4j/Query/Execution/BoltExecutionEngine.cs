using NBi.Core.Neo4j.Query.Client;
using NBi.Core.Query;
using NBi.Core.Query.Execution;
using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NBi.Core.Neo4j.Query.Execution
{
    /// <summary>
    /// Engine wrapping the Neo4j.Driver.V1 namespace for execution of NBi tests
    /// </summary>
    [SupportedCommandType(typeof(Statement))]
    internal class BoltExecutionEngine : IExecutionEngine
    {
        protected Statement Statement { get; }
        protected ISession Session { get; }

        private readonly Stopwatch stopWatch = new Stopwatch();

        protected internal BoltExecutionEngine(ISession session, Statement statement)
        {
            Session = session;
            Statement = statement;
        }

        public DataSet Execute()
        {
            using (var session = Session)
            {
                DataSet ds = null;
                StartWatch();
                Task.Run(async () =>
                {
                    ds = await OnExecuteDataSet(session, Statement);
                }).GetAwaiter().GetResult();
                StopWatch();
                return ds;
            };
        }

        protected async Task<DataSet> OnExecuteDataSet(ISession session, Statement statement)
        {
            var ds = new DataSet();
            var dt = new DataTable();
            ds.Tables.Add(dt);

            var reader = await session.RunAsync(statement);
            while (await reader.FetchAsync())
            {
                foreach (var key in reader.Current.Keys)
                    if (!dt.Columns.Contains(key))
                        dt.Columns.Add(new DataColumn(key, typeof(object)));

                var dr = dt.NewRow();
                foreach (var key in reader.Current.Keys)
                    dr[key] = reader.Current[key];
                dt.Rows.Add(dr);
            }
            await session.CloseAsync();
            dt.AcceptChanges();

            return ds;
        }

        public object ExecuteScalar()
        {
            using (var session = Session)
            {
                object obj = null;
                StartWatch();
                Task.Run(async () =>
                {
                    obj = await OnExecuteScalar(session, Statement);
                }).GetAwaiter().GetResult();
                StopWatch();
                return obj;
            };
        }

        public async Task<object> OnExecuteScalar(ISession session, Statement statement)
        {
            var reader = await session.RunAsync(statement);
            var containsRow = await reader.FetchAsync();
            if (containsRow)
            {
                if (reader.Keys.Count == 0)
                    return null;
                var result = reader.Current;
                return result.Values[result.Keys[0]];
            }
            return null;
        }

        public IEnumerable<T> ExecuteList<T>()
        {
            DataSet ds = null;
            StartWatch();
            Task.Run(async () =>
            {
                ds = await OnExecuteDataSet(Session, Statement);
            }).GetAwaiter().GetResult();

            var list = new List<T>();
            foreach (DataRow row in ds.Tables[0].Rows)
                list.Add((T)row.ItemArray[0]);
            StopWatch();
            return list;
        }


        protected void StartWatch()
        {
            stopWatch.Restart();
        }

        protected void StopWatch()
        {
            stopWatch.Stop();
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Time needed to execute query [Neo4j/Bolt]: {stopWatch.Elapsed:d'.'hh':'mm':'ss'.'fff'ms'}");
        }

        protected internal TimeSpan Elapsed { get => stopWatch.Elapsed; }

    }
}

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Graphs.Elements;
using NBi.Core.CosmosDb.Query.Command;
using NBi.Core.CosmosDb.Query.Session;
using NBi.Core.Query;
using NBi.Core.Query.Execution;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace NBi.Core.CosmosDb.Query.Execution
{
    [SupportedCommandType(typeof(GremlinQuery))]
    internal class GraphExecutionEngine : IExecutionEngine
    {
        protected GremlinQuery Query { get; }
        protected GremlinSession Session { get; }

        private readonly Stopwatch stopWatch = new Stopwatch();

        protected internal GraphExecutionEngine(GremlinSession session, GremlinQuery query)
        {
            Session = session;
            Query = query;
        }

        public DataSet Execute()
        {
            var query = Query.Create();
            DataSet ds = null;
            StartWatch();
            Task.Run(async () =>
            {
                ds = await OnExecuteDataSet(query);
            }).GetAwaiter().GetResult();
            StopWatch();
            return ds;
        }

        protected async Task<DataSet> OnExecuteDataSet(IDocumentQuery<dynamic> query)
        {
            var ds = new DataSet();
            var dt = new DataTable();
            ds.Tables.Add(dt);

            while (query.HasMoreResults)
            {
                foreach (dynamic result in await query.ExecuteNextAsync())
                {
                    var dico = (IDictionary<string, JToken>)result;

                    if (dico.ContainsKey("type"))
                        AddVertexOrEdgeToDataTable(dico, ref dt);
                    else
                        AddObjectToDataTable(dico, ref dt);
                }
            }
            dt.AcceptChanges();

            return ds;
        }

        private void AddVertexOrEdgeToDataTable(IDictionary<string, JToken> dico, ref DataTable dt)
        {
            var dr = dt.NewRow();
            foreach (var attribute in dico.Keys)
            {
                if (attribute != "properties")
                {
                    if (!dt.Columns.Contains(attribute))
                        dt.Columns.Add(new DataColumn(attribute, typeof(object)) { DefaultValue = DBNull.Value });
                    dr[attribute] = dico[attribute].ToObject<object>();
                }
                else
                {
                    var properties = (IDictionary<string, JToken>)dico["properties"];
                    foreach (var propertyKey in properties.Keys)
                    {
                        if (!dt.Columns.Contains(propertyKey))
                            dt.Columns.Add(new DataColumn(propertyKey, typeof(object)));
                        var values = properties[propertyKey].Values().Cast<JProperty>().ToList();
                        var value = (JValue)(values.Single(x => x.Name == "value").Value);
                        dr[propertyKey] = value.Value;
                    }
                }
            }
            dt.Rows.Add(dr);
        }

        private void AddObjectToDataTable(IDictionary<string, JToken> dico, ref DataTable dt)
        {
            var dr = dt.NewRow();
            foreach (var attribute in dico.Keys)
            {
                if (!dt.Columns.Contains(attribute))
                    dt.Columns.Add(new DataColumn(attribute, typeof(object)) { DefaultValue = DBNull.Value });
                dr[attribute] = dico[attribute].ToObject<object>();
            }
            dt.Rows.Add(dr);
        }

        public object ExecuteScalar()
        {
            var query = Query.Create();
            object result = null;
            StartWatch();
            Task.Run(async () =>
            {
                result = await OnExecuteScalar(query);
            }).GetAwaiter().GetResult();
            StopWatch();
            return result;
        }

        public async Task<object> OnExecuteScalar(IDocumentQuery<dynamic> query)
        {
            if (query.HasMoreResults)
            {
                var buffer = await query.ExecuteNextAsync();
                var obj = buffer.FirstOrDefault();
                return obj;
            }
            return null;
        }

        public IEnumerable<T> ExecuteList<T>()
        {
            var query = Query.Create();
            List<T> result = null;
            StartWatch();
            Task.Run(async () =>
            {
                result = await OnExecuteList<T>(query);
            }).GetAwaiter().GetResult();
            StopWatch();
            return result;
        }

        public async Task<List<T>> OnExecuteList<T>(IDocumentQuery<dynamic> query)
        {
            var list = new List<T>();
            while (query.HasMoreResults)
            {
                var buffer = await query.ExecuteNextAsync();
                foreach (var obj in buffer)
                    list.Add(obj);
            }
            return list;
        }

        protected void StartWatch()
        {
            stopWatch.Restart();
        }

        protected void StopWatch()
        {
            stopWatch.Stop();
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Time needed to execute query [Cosmos DB/Gremlin]: {stopWatch.Elapsed:d'.'hh':'mm':'ss'.'fff'ms'}");
        }

        protected internal TimeSpan Elapsed { get => stopWatch.Elapsed; }


    }
}

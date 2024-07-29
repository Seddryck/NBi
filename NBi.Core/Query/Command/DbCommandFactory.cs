using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Query.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;
using NBi.Extensibility;

namespace NBi.Core.Query.Command
{
    abstract class DbCommandFactory : ICommandFactory
    {
        public abstract bool CanHandle(IClient client);

        public ICommand Instantiate(IClient client, IQuery query, ITemplateEngine? engine)
        {
            if (!CanHandle(client))
                throw new ArgumentException();
            var connection = (IDbConnection)client.CreateNew();
            var cmd = Instantiate(connection, query, engine);
            return new Command(connection, cmd);
        }

        protected IDbCommand Instantiate(IDbConnection connection, IQuery query, ITemplateEngine? engine)
        {
            var cmd = connection.CreateCommand();
            if (query.TemplateTokens is not null && query.TemplateTokens.Any() && engine is not null)
                cmd.CommandText = ApplyVariablesToTemplate(engine, query.Statement, query.TemplateTokens);
            else
                cmd.CommandText = query.Statement;

            if (query.Parameters != null && query.Parameters.Any())
            {
                foreach (var p in query.Parameters)
                {
                    var param = cmd.CreateParameter();
                    param.ParameterName = RenameParameter(p.Name);

                    param.Value = p.GetValue();
                    var dbType = new DbTypeBuilder().Build(p.SqlType);
                    if (dbType != null)
                    {
                        param.Direction = ParameterDirection.Input;
                        param.DbType = dbType.Value;
                        param.Size = dbType.Size;
                        param.Precision = dbType.Precision;
                    }
                    cmd.Parameters.Add(param);
                }
            }

            if (query.CommandType>0)
                cmd.CommandType = query.CommandType;

            var commandTimeout = Convert.ToInt32(Math.Max(0, query.Timeout.TotalSeconds));
            cmd.CommandTimeout = commandTimeout;

            return cmd;
        }

        protected virtual string RenameParameter(string originalName) => originalName;

        private string ApplyVariablesToTemplate(ITemplateEngine engine, string template, IEnumerable<IQueryTemplateVariable> variables)
        {
            var valuePairs = new List<KeyValuePair<string, object>>();
            foreach (var variable in variables)
                valuePairs.Add(new KeyValuePair<string, object>(variable.Name, variable.Value));

            return engine.Render(template, valuePairs);
        }

    }
}


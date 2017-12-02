using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Core.Query
{
    public class DbCommandFactory
    {
        public IDbCommand Instantiate(IDbConnection connection, IQuery query)
        {
            return Build(connection, query.Statement, query.CommandType, query.Parameters, query.TemplateTokens, Convert.ToInt32(query.Timeout.TotalSeconds));
        }

        public IDbCommand Build(string connectionString, string query, IEnumerable<IQueryParameter> parameters)
        {
            var factory = new ConnectionFactory();
            var connection = factory.Instantiate(connectionString);
            return Build(connection, query, parameters);
        }

        public IDbCommand Build(string connectionString, string query, IEnumerable<IQueryParameter> parameters, IEnumerable<IQueryTemplateVariable> variables, TimeSpan timeout)
        {
            var factory = new ConnectionFactory();
            var connection = factory.Instantiate(connectionString);
            return Build(connection, query, parameters, variables, Convert.ToInt32(timeout.TotalSeconds));
        }

        public IDbCommand Build(IDbConnection connection, string query, IEnumerable<IQueryParameter> parameters)
        {
            return Build(connection, query, parameters, null);
        }

        public IDbCommand Build(IDbConnection connection, string query, IEnumerable<IQueryTemplateVariable> variables)
        {
            return Build(connection, query, null, variables);
        }

        public IDbCommand Build(IDbConnection connection, string query, IEnumerable<IQueryParameter> parameters, IEnumerable<IQueryTemplateVariable> variables)
        {
            var cmd = connection.CreateCommand();

            if (variables != null && variables.Count() > 0)
                query = ApplyVariablesToTemplate(query, variables);

            cmd.CommandText = query;

            if (parameters != null && parameters.Count() > 0)
            {
                foreach (var p in parameters)
                {
                    var param = cmd.CreateParameter();

                    if (cmd is AdomdCommand && p.Name.StartsWith("@"))
                        param.ParameterName = p.Name.Substring(1, p.Name.Length - 1);
                    else if (cmd is SqlCommand && !p.Name.StartsWith("@") && char.IsLetter(p.Name[0]))
                        param.ParameterName = "@" + p.Name;
                    else
                        param.ParameterName = p.Name;

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

            return cmd;
        }

        public IDbCommand Build(IDbConnection connection, string text, CommandType commandType, IEnumerable<IQueryParameter> parameters, IEnumerable<IQueryTemplateVariable> variables, int timeout)
        {
            var cmd = Build(connection, text, parameters, variables, timeout);
            cmd.CommandType = commandType;
            return cmd;
        }

        public IDbCommand Build(IDbConnection connection, string query, IEnumerable<IQueryParameter> parameters, IEnumerable<IQueryTemplateVariable> variables, int timeout)
        {
            var cmd = Build(connection, query, parameters, variables);
            var commandTimeout = Math.Max(0, timeout);
            cmd.CommandTimeout = commandTimeout;
            return cmd;
        }

        private string ApplyVariablesToTemplate(string template, IEnumerable<IQueryTemplateVariable> variables)
        {
            var templateEngine = new StringTemplateEngine(template, variables);
            return templateEngine.Build();
        }

    }
}

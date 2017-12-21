using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Query.Connection;

namespace NBi.Core.Query
{
    public class DbCommandFactory
    {
        public IDbCommand Instantiate(IDbConnection connection, IQuery query)
        {
            return Build(connection, query.Statement, query.CommandType, query.Parameters, query.TemplateTokens, Convert.ToInt32(query.Timeout.TotalSeconds));
        }
        
        protected IDbCommand Build(IDbConnection connection, string query, CommandType commandType, IEnumerable<IQueryParameter> parameters, IEnumerable<IQueryTemplateVariable> variables, int timeout)
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

            cmd.CommandType = commandType;

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

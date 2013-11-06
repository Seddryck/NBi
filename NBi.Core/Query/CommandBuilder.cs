using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Core.Query
{
    public class CommandBuilder
    {
        public IDbCommand Build(string connectionString, string query, IEnumerable<IQueryParameter> parameters)
        {
            return Build(connectionString, query, parameters, null);
        }

        public IDbCommand Build(string connectionString, string query, IEnumerable<IQueryVariable> variables)
        {
            return Build(connectionString, query, null, variables);
        }

        public IDbCommand Build(string connectionString, string query, IEnumerable<IQueryParameter> parameters, IEnumerable<IQueryVariable> variables)
        {
            var conn = new ConnectionFactory().Get(connectionString);
            var cmd = conn.CreateCommand();

            if (variables != null && variables.Count() > 0)
                query = ApplyVariablesToTemplate(query, variables);
                       
            cmd.CommandText = query;

            if (parameters!=null && parameters.Count()>0)
            {
                foreach (var p in parameters)
                {
                    var param = cmd.CreateParameter();

                    if (cmd is AdomdCommand && p.Name.StartsWith("@"))
                        p.Name = p.Name.Substring(1, p.Name.Length - 1);
                    if (cmd is SqlCommand && !p.Name.StartsWith("@") && char.IsLetter(p.Name[0]))
                        p.Name = "@" + p.Name;

                    param.ParameterName = p.Name;
                    param.Value = p.StringValue;
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

        private string ApplyVariablesToTemplate(string template, IEnumerable<IQueryVariable> variables)
        {
            var templateEngine = new StringTemplateEngine(template, variables);
            return templateEngine.Build();
        }

    }
}

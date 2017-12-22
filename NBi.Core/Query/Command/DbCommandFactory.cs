using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Query.Session;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Command
{
    abstract class DbCommandFactory : ICommandFactory
    {
        public abstract bool CanHandle(ISession session);

        public ICommand Instantiate(ISession session, IQuery query)
        {
            if (!CanHandle(session))
                throw new ArgumentException();
            var cmd = Instantiate(session.CreateNew() as IDbConnection, query);
            return new Command(cmd);
        }

        protected IDbCommand Instantiate(IDbConnection connection, IQuery query)
        {
            var cmd = connection.CreateCommand();
            if (query.TemplateTokens != null && query.TemplateTokens.Count() > 0)
                cmd.CommandText = ApplyVariablesToTemplate(query.Statement, query.TemplateTokens);
            else
                cmd.CommandText = query.Statement;

            if (query.Parameters != null && query.Parameters.Count() > 0)
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

        private string ApplyVariablesToTemplate(string template, IEnumerable<IQueryTemplateVariable> variables)
        {
            var templateEngine = new StringTemplateEngine(template, variables);
            return templateEngine.Build();
        }

    }
}


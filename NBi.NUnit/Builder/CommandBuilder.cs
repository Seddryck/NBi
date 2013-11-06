using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core;
using NBi.Core.Query;
using NBi.Xml.Items;

namespace NBi.NUnit.Builder
{
    internal class CommandBuilder
    {
        public IDbCommand Build(string connectionString, string query, IEnumerable<QueryParameterXml> parameters)
        {
            var conn = new ConnectionFactory().Get(connectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = query;

            if (parameters!=null && parameters.Count()>0)
            {
                foreach (var p in parameters)
                {
                    var param = cmd.CreateParameter();
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
    }
}

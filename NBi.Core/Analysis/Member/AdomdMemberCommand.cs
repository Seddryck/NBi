using System;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Core.Analysis.Member
{
    public class AdomdMemberCommand
    {
        public string ConnectionString { get; set; }
        public string Perspective { get; set; }
        public string Path { get; set; }

        public AdomdMemberCommand(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected AdomdCommand CreateCommand()
        {
            var conn = new AdomdConnection();
            conn.ConnectionString = ConnectionString;
            try
            {
                conn.Open();
            }
            catch (AdomdConnectionException ex)
            {

                throw new ConnectionException(ex);
            }


            var cmd = new AdomdCommand();
            cmd.Connection = conn;
            return cmd;
        }

        public AdomdCommand BuildCommand()
        {
            var cmd = CreateCommand();

            string whereClause = "";
            whereClause += string.Format(" where CUBE_NAME='{0}'", Perspective);

            var pathParser = PathParser.Build(Perspective, Path);
            whereClause += string.Format(" and [{0}_UNIQUE_NAME]='{1}'", pathParser.Position.Current.ToUpper(), Path);
                                    
            cmd.CommandText = string.Format("select * from $system.mdschema_members{0}", whereClause);

            return cmd;
           
        }
    }
}

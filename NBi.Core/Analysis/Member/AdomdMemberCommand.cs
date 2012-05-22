using System;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Core.Analysis.Member
{
    public class AdomdMemberCommand
    {
        public enum PlaceHolderType
        {
            Hierarchy,
            Level
        }


        public string ConnectionString { get; set; }
        public string Perspective { get; set; }
        public string PlaceHolderUniqueName { get; set; }
        public PlaceHolderType PlaceHolder { get; set; }

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

            switch (PlaceHolder)
            {
                case PlaceHolderType.Hierarchy:
                    whereClause += string.Format(" and [{0}_UNIQUE_NAME]='{1}'", "HIERARCHY", PlaceHolderUniqueName);
                    break;
                case PlaceHolderType.Level:
                    whereClause += string.Format(" and [{0}_UNIQUE_NAME]='{1}'", "LEVEL", PlaceHolderUniqueName);
                    break;
                default:
                    throw new Exception();
            }
                      
            cmd.CommandText = string.Format("select * from $system.mdschema_members{0}", whereClause);

            return cmd;
           
        }
    }
}

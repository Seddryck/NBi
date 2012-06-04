using System;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Analysis.Metadata;

namespace NBi.Core.Analysis.Member
{
    public class SchemaRowsetAdomdEngine : IDiscoverMemberEngine
    {
        public event ProgressStatusHandler ProgressStatusChanged;
        
        public SchemaRowsetAdomdEngine() 
        {
        }

        public MemberResult Execute(DiscoverMemberCommand cmd)
        {
            var list = new MemberResult();

            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Starting discovery ..."));

            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs(string.Format("Discovering {0} in {1}", cmd.Path, cmd.Perspective)));

            var rdr = ExecuteReader(BuildCommand(cmd));
            // Traverse the response and 
            // read column 6, "LEVEL_NUMBER"
            // read column 7, "MEMBER_ORDINAL"
            // read column 9, "MEMBER_UNIQUE_NAME"
            // read column 12, "MEMBER_CAPTION"
            while (rdr.Read())
            {
                var uniqueName = (string)rdr.GetValue(9);
                var caption = (string)rdr.GetValue(12);
                var ordinal = Convert.ToInt32((uint)rdr.GetValue(7));
                var levelNumber = Convert.ToInt32((uint)rdr.GetValue(6));

                list.Add(new Member(uniqueName, caption, ordinal, levelNumber));
            }

            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Discovery completed"));

            return list;
        }


        protected AdomdDataReader ExecuteReader(AdomdCommand cmd)
        {
            AdomdDataReader rdr = null;
            try
            {
                rdr = cmd.ExecuteReader();
                return rdr;
            }
            catch (AdomdConnectionException ex)
            { throw new ConnectionException(ex); }
            catch (AdomdErrorResponseException ex)
            { throw new ConnectionException(ex); }
        }

        protected AdomdCommand CreateCommand(string connectionString)
        {
            var conn = new AdomdConnection();
            conn.ConnectionString = connectionString;
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

        public AdomdCommand BuildCommand(DiscoverMemberCommand disco)
        {
            var cmd = CreateCommand(disco.ConnectionString);

            string whereClause = "";
            whereClause += string.Format(" where CUBE_NAME='{0}'", disco.Perspective);

            var pathParser = PathParser.Build(disco.Perspective, disco.Path);
            whereClause += string.Format(" and [{0}_UNIQUE_NAME]='{1}'", pathParser.Position.Current.ToUpper(), disco.Path);

            cmd.CommandText = string.Format("select * from $system.mdschema_members{0}", whereClause);

            return cmd;

        }
    }
}

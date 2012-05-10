using System;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Analysis.Metadata;

namespace NBi.Core.Analysis
{
    public class MemberAdomdExtractor
    {
        public event ProgressStatusHandler ProgressStatusChanged;
        
        public string ConnectionString { get; private set; }

        public MemberAdomdExtractor(string connectionString) 
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

        public MemberList GetMembersByHierarchy(string perspectiveName, string hierarchyUniqueName)
        {
            return GetMembers(perspectiveName, "HIERARCHY", hierarchyUniqueName);
        }

        public MemberList GetMembersByLevel(string perspectiveName, string levelUniqueName)
        {
            return GetMembers(perspectiveName, "LEVEL", levelUniqueName);
        }

        public MemberList GetMembers(string perspectiveName, string filterMember, string filterValue)
        {
            var list = new MemberList();
            
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Starting investigation ..."));

            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs(string.Format("Investigating {0} {1}", filterMember.ToLower(), filterValue)));
            using (var cmd = CreateCommand())
            {
                string whereClause = "";
                whereClause += string.Format(" where CUBE_NAME='{0}'", perspectiveName);
                whereClause += string.Format(" and [{0}_UNIQUE_NAME]='{1}'", filterMember, filterValue);
                cmd.CommandText = string.Format("select * from $system.mdschema_members{0}", whereClause);
                var rdr = ExecuteReader(cmd);
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
            }

            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Cube investigated"));

            return list;
        }
    }
}

using System;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Analysis.Metadata;

namespace NBi.Core.Analysis.Member
{
    public class MemberAdomdEngine : IMemberEngine
    {
        public event ProgressStatusHandler ProgressStatusChanged;
        
        public MemberAdomdEngine() 
        {
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

        public MemberResult Execute(AdomdMemberCommand cmd)
        {
            var list = new MemberResult();

            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Starting investigation ..."));

            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs(string.Format("Investigating {0} in {1}", cmd.Path, cmd.Perspective)));

            var rdr = ExecuteReader(cmd.BuildCommand());
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
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Investigation completed"));

            return list;
        }
    }
}

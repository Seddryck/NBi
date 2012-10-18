using System;
using System.Linq;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Analysis.Discovery;

namespace NBi.Core.Analysis.Member
{
    public class CubeDimensionAdomdEngine : IDiscoverMemberEngine
    {
        public event ProgressStatusHandler ProgressStatusChanged;

        public CubeDimensionAdomdEngine() 
        {
        }

        protected CellSet ExecuteCellSet(AdomdCommand cmd)
        {
            CellSet cs = null;
            try
            {
                cs = cmd.ExecuteCellSet();
                return cs;
            }
            catch (AdomdConnectionException ex)
            { throw new ConnectionException(ex); }
            catch (AdomdErrorResponseException ex)
            { throw new ConnectionException(ex); }
        }

        public MemberResult Execute(MembersDiscoveryCommand cmd)
        {
            var list = new MemberResult();

            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Starting discovery ..."));

            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs(string.Format("Discovering {0} on {1}", cmd.Path, cmd.PerspectiveName)));

            var cs = ExecuteCellSet(BuildCommand(cmd));
            // Traverse the response (The response is on first line!!!) 
            var i = 0;
            foreach (var position in cs.Axes[1].Positions)
	        {
                var member = position.Members[0];
                var uniqueName = member.UniqueName;
                var caption = member.Caption;
                var ordinal = ++i;
                var levelNumber = member.LevelDepth;

                list.Add(new Member(uniqueName, caption, ordinal, levelNumber));
	        }    

            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Discovery completed"));

            return list;
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

        public AdomdCommand BuildCommand(MembersDiscoveryCommand disco)
        {
            var cmd = CreateCommand(disco.ConnectionString);
            if (string.IsNullOrEmpty(disco.MemberCaption))
                cmd.CommandText = string.Format("select {0} on 0, {1}.{2} on 1 from [{3}]", "{}" , disco.Path, disco.Function, disco.PerspectiveName);
            else
                cmd.CommandText = string.Format("select {0} on 0, {1}.[{4}].{2} on 1 from [{3}]", "{}", disco.Path, disco.Function, disco.PerspectiveName, disco.MemberCaption);
            Console.Out.WriteLine(cmd.CommandText);
            return cmd;
        }

    }
}

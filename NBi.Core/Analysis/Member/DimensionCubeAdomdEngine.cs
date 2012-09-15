using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Analysis;
using NBi.Core.Analysis.Discovery;

namespace NBi.Core.Analysis.Member
{
    public class DimensionCubeAdomdEngine : IDiscoverMemberEngine
    {
        public event ProgressStatusHandler ProgressStatusChanged;

        public DimensionCubeAdomdEngine() 
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
                ProgressStatusChanged(this, new ProgressStatusEventArgs(string.Format("Discovering {0}", cmd.Path)));

            var cs = ExecuteCellSet(BuildCommand(cmd));
            // Traverse the response (The response is on first line!!!) 
            var i = 0;
            foreach (var position in cs.Axes[0].Positions)
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

            string dimensionCube = GetDimensionCube(disco.Path);

            if (string.IsNullOrEmpty(disco.MemberCaption))
                cmd.CommandText = string.Format("select {0}.{1} on 0 from {2}", disco.Path, disco.Function, dimensionCube);
            else
                cmd.CommandText = string.Format("select {0}.[{3}].{1} on 0 from {2}", disco.Path, disco.Function, dimensionCube, disco.MemberCaption);
            Console.Out.WriteLine(cmd.CommandText);
            return cmd;
        }

        protected string GetDimensionCube(string path)
        {
            var dimension = path.Substring(0, (path.IndexOf("]") )).Replace("[", "").Replace("]", "");
            return string.Format("[${0}]", dimension);
        }
    }
}

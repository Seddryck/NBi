using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Analysis.Discovery;

namespace NBi.Core.Analysis.Member
{
    public class MembersCommand
    {
        public event ProgressStatusHandler ProgressStatusChanged;

        public string ConnectionString { get; private set; }
        public string Function { get; private set; }
        public string MemberCaption { get; private set; }

        public MembersCommand(string connectionString, string function, string memberCaption)
        {
            ConnectionString = connectionString;
            Function = function;
            MemberCaption = memberCaption;
        }

        protected void Inform(string text)
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs(text));
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


        public virtual MemberResult List(IEnumerable<IFilter> filters)
        {
            var list = new MemberResult();

            Inform("Investigating members");

            using (var cmd = CreateCommand())
            {
                var path = BuildPath(filters);
                var perspective = GetPerspective(filters);
                var commandText = Build(perspective.Value, path, Function, MemberCaption);
                cmd.CommandText = commandText;
                var cs = ExecuteCellSet(cmd);
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
            }

            Inform("Members investigated");

            return list;
        }
  
        private CaptionFilter GetPerspective(IEnumerable<IFilter> filters)
        {
            var perFilter = FindFilterOrNull(filters, DiscoveryTarget.Perspectives);
            return perFilter;
        }

        private string BuildPath(IEnumerable<IFilter> filters)
        {
            var dimFilter = FindFilterOrNull(filters, DiscoveryTarget.Dimensions);

            var hieFilter = FindFilterOrNull(filters, DiscoveryTarget.Hierarchies);
            if (hieFilter == null)
                return string.Format("[{0}]", dimFilter.Value);
            
            var levFilter = FindFilterOrNull(filters, DiscoveryTarget.Levels);
            if (levFilter == null)
                return string.Format("[{0}].[{1}]", dimFilter.Value, hieFilter.Value);
            else
                return string.Format("[{0}].[{1}].[{2}]", dimFilter.Value, hieFilter.Value, levFilter.Value);
        }

        protected CaptionFilter FindFilterOrNull(IEnumerable<IFilter> filters, DiscoveryTarget target)
        {
            var filter = filters.FirstOrDefault(f => f is CaptionFilter && ((CaptionFilter)f).Target == target);
            return (CaptionFilter)filter;
        }

        public string Build(string perspective, string path, string function, string memberCaption)
        {
            var commandText = string.Empty;
            if (string.IsNullOrEmpty(memberCaption))
                commandText = string.Format("select {0} on 0, {1}.{2} on 1 from [{3}]", "{}", path, function, perspective);
            else
                commandText = string.Format("select {0} on 0, {1}.[{4}].{2} on 1 from [{3}]", "{}", path, function, perspective, memberCaption);
            Console.Out.WriteLine(commandText);
            return commandText;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Analysis.Request;

namespace NBi.Core.Analysis.Member
{
    public class MembersCommand
    {
        public event ProgressStatusHandler ProgressStatusChanged;

        public string ConnectionString { get; private set; }
        public string Function { get; private set; }
        public string MemberCaption { get; private set; }
        public IEnumerable<string> ExcludedMembers { get; private set; }

        public MembersCommand(string connectionString, string function, string memberCaption)
            : this(connectionString,function,memberCaption,null)
        {}

        public MembersCommand(string connectionString, string function, string memberCaption, IEnumerable<string> excludedMembers)
        {
            ConnectionString = connectionString;
            Function = function;
            MemberCaption = memberCaption;
            ExcludedMembers = excludedMembers;
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

                throw new ConnectionException(ex, conn.ConnectionString);
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
            { throw new ConnectionException(ex, cmd.Connection.ConnectionString); }
            catch (AdomdErrorResponseException ex)
            { throw new ConnectionException(ex, cmd.Connection.ConnectionString); }
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
            { throw new ConnectionException(ex, cmd.Connection.ConnectionString); }
            catch (AdomdErrorResponseException ex)
            { throw new ConnectionException(ex, cmd.Connection.ConnectionString); }
        }


        public virtual MemberResult List(IEnumerable<IFilter> filters)
        {
            var list = new MemberResult();

            Inform("Investigating members");

            using (var cmd = CreateCommand())
            {
                var path = BuildPath(filters);
                var perspective = GetPerspective(filters);
                var commandText = Build(perspective.Value, path, Function, MemberCaption, ExcludedMembers);
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
            return Build(perspective, path, function,memberCaption, null);
        }

        public string Build(string perspective, string path, string function, string memberCaption, IEnumerable<string> exludedMembers)
        {
            var members = string.Empty;
            if (string.IsNullOrEmpty(memberCaption))
                members = string.Format("{0}.{1}", path, function);
            else
                members = string.Format("{0}.[{2}].{1}", path, function, memberCaption);

            if (exludedMembers!=null && exludedMembers.Count()>0)
            {
                foreach (var excl in exludedMembers)
                    members = string.Format("{0}-{1}.[{2}]", members, path, excl);
                members = string.Format("{0}{1}{2}", "{", members, "}");
            }
                            
            var commandText = string.Empty;
            commandText = string.Format("select {0} on 0, {1} on 1 from [{2}]", "{}", members, perspective);

            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, commandText);
            return commandText;
        }
    }
}

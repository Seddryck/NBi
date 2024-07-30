using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Analysis.Request;
using System.Data;
using NBi.Core.Query.Client;
using NBi.Extensibility;

namespace NBi.Core.Analysis.Member
{
    public class MembersCommand
    {
        public event ProgressStatusHandler? ProgressStatusChanged;

        public string ConnectionString { get; private set; }
        public string Function { get; private set; }
        public string MemberCaption { get; private set; }
        public IEnumerable<string> ExcludedMembers { get; private set; }
        public IEnumerable<PatternValue> ExcludedPatterns { get; private set; }

        public MembersCommand(string connectionString, string function, string memberCaption)
            : this(connectionString,function,memberCaption, [], [])
        {}

        public MembersCommand(string connectionString, string function, string memberCaption, IEnumerable<string> excludedMembers, IEnumerable<PatternValue> excludedPatterns)
        {
            ConnectionString = connectionString;
            Function = function;
            MemberCaption = memberCaption;
            ExcludedMembers = excludedMembers;
            ExcludedPatterns = excludedPatterns;
        }

        protected void Inform(string text)
        {
            ProgressStatusChanged?.Invoke(this, new ProgressStatusEventArgs(text));
        }

        protected IDbCommand CreateCommand()
        {
            var factory = new ClientProvider();
            var conn = factory.Instantiate(ConnectionString).CreateNew() as IDbConnection
                        ?? throw new NotSupportedException();
            
            var cmd = conn.CreateCommand();
            return cmd;
        }

        protected virtual AdomdDataReader ExecuteReader(AdomdCommand cmd)
        {
            try
            {
                var rdr = cmd.ExecuteReader();
                return rdr;
            }
            catch (AdomdConnectionException ex)
            { throw new ConnectionException(ex, cmd.Connection.ConnectionString); }
            catch (AdomdErrorResponseException ex)
            { throw new ConnectionException(ex, cmd.Connection.ConnectionString); }
        }

        protected CellSet ExecuteCellSet(AdomdCommand cmd)
        {
            try
            {
                cmd.Connection.Open();
                var cs = cmd.ExecuteCellSet();
                cmd.Connection.Close();
                return cs;
            }
            catch (AdomdConnectionException ex)
            { throw new ConnectionException(ex, cmd.Connection.ConnectionString); }
            catch (AdomdErrorResponseException ex)
            { throw new ConnectionException(ex, cmd.Connection.ConnectionString); }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public virtual MemberResult List(IEnumerable<IFilter> filters)
        {
            var list = new MemberResult();

            Inform("Investigating members");

            using (var cmd = CreateCommand())
            {
                var path = BuildPath(filters);
                var perspective = GetPerspective(filters) ?? throw new NotSupportedException();
                var commandText = Build(perspective.Value, path, Function, MemberCaption, ExcludedMembers, ExcludedPatterns);
                cmd.CommandText = commandText;
                if (cmd is not AdomdCommand)
                    throw new NotImplementedException();
                var cs = ExecuteCellSet((cmd as AdomdCommand)!);
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
  
        private CaptionFilter? GetPerspective(IEnumerable<IFilter> filters)
        {
            var perFilter = FindFilterOrNull(filters, DiscoveryTarget.Perspectives);
            return perFilter;
        }

        private string BuildPath(IEnumerable<IFilter> filters)
        {
            var setFilter = FindFilterOrNull(filters, DiscoveryTarget.Sets);
            if (setFilter != null)
                return string.Format("[{0}]", setFilter.Value);

            var dimFilter = FindFilterOrNull(filters, DiscoveryTarget.Dimensions) 
                ?? throw new NotSupportedException();

            var hierarchyFilter = FindFilterOrNull(filters, DiscoveryTarget.Hierarchies);
            if (hierarchyFilter == null)
                return $"[{dimFilter.Value}]";
            
            var levFilter = FindFilterOrNull(filters, DiscoveryTarget.Levels);
            if (levFilter == null)
                return $"[{dimFilter.Value}].[{hierarchyFilter.Value}]";
            else
                return $"[{dimFilter.Value}].[{hierarchyFilter.Value}].[{levFilter.Value}]";
        }

        protected virtual CaptionFilter? FindFilterOrNull(IEnumerable<IFilter> filters, DiscoveryTarget target)
        {
            var filter = filters.FirstOrDefault(f => f is CaptionFilter && ((CaptionFilter)f).Target == target);
            return (CaptionFilter?)filter;
        }

        public string Build(string perspective, string path, string function, string memberCaption)
        {
            return Build(perspective, path, function,memberCaption, [], []);
        }

        public string Build(string perspective, string path, string function, string memberCaption, IEnumerable<string> exludedMembers, IEnumerable<PatternValue> excludedPatterns)
        {
            var members = string.Empty;
            if (string.IsNullOrEmpty(function))
                members = $"{path}";
            else if (string.IsNullOrEmpty(memberCaption))
                members = $"{path}.{function}";
            else
                members = $"{path}.[{function}].{memberCaption}";

            if (exludedMembers!=null && exludedMembers.Any())
            {
                foreach (var excl in exludedMembers)
                    members = $"{members}-{path}.[{excl}]";
                members = $"{"{"}{members}{"}"}";
            }

            if (ExcludedPatterns != null && ExcludedPatterns.Any())
            {
                var hierarchyPath = BuildHierarchyPath(path);
                var exclPattern = BuildExcludedPatterns(hierarchyPath, excludedPatterns);
                members = $"filter({members}, {exclPattern})";                
            }
                            
            var commandText = string.Empty;
            commandText = $"select {"{}"} on 0, {members} on 1 from [{perspective}]";

            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, commandText);
            return commandText;
        }

        private string BuildHierarchyPath(string path)
        {
            if (path.Count(c => c == '.') == 1)
                return path;
            else
                return path.Substring(0,path.IndexOf('.', path.IndexOf('.') + 1));
        }

        internal protected string BuildExcludedPatterns(string hierarchyPath, IEnumerable<PatternValue> excludedPatterns)
        {
            if (excludedPatterns == null || !excludedPatterns.Any())
                return string.Empty;

            var exclusions = new System.Text.StringBuilder();
            foreach (var excl in excludedPatterns)
            {
                var exclPattern = string.Empty;
                switch (excl.Pattern)
                {
                    case Pattern.StartWith:
                        exclPattern = "left({0}, len('{1}'))<>'{1}'";
                        break;
                    case Pattern.EndWith:
                        exclPattern = "right({0}, len('{1}'))<>'{1}'";
                        break;
                    case Pattern.Exact:
                        exclPattern = "{0}<>'{1}'";
                        break;
                    case Pattern.Contain:
                        exclPattern = "instr({0}, '{1}') = 0";
                        break;
                    default:
                        throw new NotImplementedException();
                }
                exclusions.AppendFormat(" and " + exclPattern, hierarchyPath + ".CurrentMember.Member_Name", excl.Text);
            }
            exclusions.Remove(0, 4);
            return exclusions.ToString();
        }
    }
}

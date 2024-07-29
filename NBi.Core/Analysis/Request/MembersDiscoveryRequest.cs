using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Member;
using NBi.Extensibility;

namespace NBi.Core.Analysis.Request
{
    public class MembersDiscoveryRequest : BaseDiscoveryRequest
    {
        public string MemberCaption { get; set; }
        public IEnumerable<string> ExcludedMembers { get; set; }
        public IEnumerable<PatternValue> ExcludedPatterns { get; set; }
        public string Function { get; set; }

        protected internal MembersDiscoveryRequest(string connectionString, string function, string memberCaption
            , IEnumerable<string> excludedMembers, IEnumerable<PatternValue> excludedPatterns)
            : base(connectionString)
            => (Function, MemberCaption, ExcludedMembers, ExcludedPatterns) = (function, memberCaption, excludedMembers, excludedPatterns);

        public string Perspective
            => GetFilter(DiscoveryTarget.Perspectives)?.Value ?? throw new NBiException("Perspective doesn't exist");

        public override string Path
        {
            get
            {
                string path = base.Path;

                if (!string.IsNullOrEmpty(MemberCaption))
                    path = string.Format("{0}.[{1}]", path, MemberCaption);

                return path;
            }
        }


        
    }
}

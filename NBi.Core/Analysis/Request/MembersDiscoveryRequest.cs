using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Core.Analysis.Request
{
    public class MembersDiscoveryRequest : BaseDiscoveryRequest
    {
        public string MemberCaption { get; set; }
        public IEnumerable<string> Exlusions { get; set; }
        public string Function { get; set; }

        protected internal MembersDiscoveryRequest()
            : base()
        {
        }

        public string Perspective
        {
            get
            {
                return GetFilter(DiscoveryTarget.Perspectives).Value;
            }
        }


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

using System;
using System.Linq;

namespace NBi.Core.Analysis.Discovery
{
    public class MembersDiscoveryCommand : BaseDiscoveryCommand
    {
        public string MemberCaption { get; set; }
        public string Function { get; set; }

        protected internal MembersDiscoveryCommand()
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

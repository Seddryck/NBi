using System;
using System.Linq;

namespace NBi.Core.Analysis.Discovery
{
    public class MembersDiscoveryCommand : BaseDiscoveryCommand
    {
        public string MemberCaption { get; set; }
        public string Function { get; set; }

        public MembersDiscoveryCommand()
            : base()
        {
        }

    }
}

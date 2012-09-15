using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Discovery
{
    public class MembersDiscoveryCommand : PathDiscoveryCommand
    {
        public string MemberCaption { get; private set; }
        public string Function { get; private set; }

        public MembersDiscoveryCommand(string connectionString, string perspectiveName, string path, string memberCaption, string function)
            : base(connectionString, perspectiveName, path)
        {
            Target = DiscoveryTarget.Members;
            MemberCaption = memberCaption;
            Function = function;
        }

    }
}

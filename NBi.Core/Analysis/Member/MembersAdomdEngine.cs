using System;
using System.Linq;
using NBi.Core.Analysis.Discovery;

namespace NBi.Core.Analysis.Member
{
    public class MembersAdomdEngine
    {
        public virtual MemberResult GetMembers(MembersDiscoveryCommand command)
        {
            var cmd = new MembersCommand(command.ConnectionString, command.Function, command.MemberCaption);
            return cmd.List(command.GetAllFilters());
        }
    }
}

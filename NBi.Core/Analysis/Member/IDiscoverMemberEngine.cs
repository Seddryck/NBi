using System;
using NBi.Core.Analysis.Discovery;

namespace NBi.Core.Analysis.Member
{
    public interface IDiscoverMemberEngine
    {
        MemberResult Execute(MembersDiscoveryCommand cmd);
    }
}
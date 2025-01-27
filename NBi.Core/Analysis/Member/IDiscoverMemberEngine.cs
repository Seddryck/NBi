using System;
using NBi.Core.Analysis.Request;

namespace NBi.Core.Analysis.Member;

public interface IDiscoverMemberEngine
{
    MemberResult Execute(MembersDiscoveryRequest cmd);
}
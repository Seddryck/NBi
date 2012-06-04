using System;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Analysis.Metadata;

namespace NBi.Core.Analysis.Member
{
    public interface IDiscoverMemberEngine
    {
        MemberResult Execute(DiscoverMemberCommand cmd);
    }
}
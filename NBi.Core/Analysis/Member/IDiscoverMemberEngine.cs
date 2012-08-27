using System;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Analysis.Metadata;
using NBi.Core.Analysis;

namespace NBi.Core.Analysis.Member
{
    public interface IDiscoverMemberEngine
    {
        MemberResult Execute(DiscoverCommand cmd);
    }
}
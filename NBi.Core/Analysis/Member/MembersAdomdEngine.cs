using System;
using System.Linq;
using NBi.Core.Analysis.Request;

namespace NBi.Core.Analysis.Member;

public class MembersAdomdEngine
{
    public virtual MemberResult GetMembers(MembersDiscoveryRequest command)
    {
        var cmd = new MembersCommand(command.ConnectionString, command.Function, command.MemberCaption, command.ExcludedMembers, command.ExcludedPatterns);
        var filters = command.GetAllFilters();
        return cmd.List(filters);
    }
}

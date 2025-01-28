using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Member;

public class MemberResult : List<Member>
{
    public void Add(string caption)
    {
        Add(new Member(caption, caption, 0, 0));
    }

    public IEnumerable<string> ToCaptions()
    {
        var list = new List<string>();
        this.ForEach(item => list.Add(item.Caption));

        return list;
    }
}

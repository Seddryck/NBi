using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Member;

public class PatternValue(Pattern pattern, string text)
{
    public Pattern Pattern { get; set; } = pattern;
    public string Text { get; set; } =  text;
}

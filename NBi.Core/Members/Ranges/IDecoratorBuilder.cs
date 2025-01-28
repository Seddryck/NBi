using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Members.Ranges;

interface IDecoratorBuilder
{
    void Apply(IEnumerable<string> values);
    void Setup(IRange range);
    IEnumerable<string> GetResult();
}

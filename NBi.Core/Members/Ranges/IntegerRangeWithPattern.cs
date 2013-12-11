using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Members.Ranges
{
    public class IntegerRangeWithPattern : IntegerRange
    {
        public string Pattern { get; set; }
        public PositionValue Position { get; set; }
        public enum PositionValue
        {
            Suffix,
            Prefix
        }
    }
}

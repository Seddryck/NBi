using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBiMember = NBi.NUnit.Member;
using NBiStructure = NBi.NUnit.Structure;
using NF = NUnit.Framework;

namespace NBi.NUnit.FluentInterface
{
    public class Has : NF.Has
    {
        public Has()
        {
        }

        public static NBiMember.ContainsConstraint Member(string value)
        {
            var ctr = new NBiMember.ContainsConstraint(value);
            return ctr;
        }

        public static NBiMember.ContainsConstraint Members(IEnumerable<string> values)
        {
            var ctr = new NBiMember.ContainsConstraint(values);
            return ctr;
        }

        public static NBiMember.CountConstraint Exactly(int count)
        {
            var ctr = new NBiMember.CountConstraint();
            ctr.Exactly(count);
            return ctr;
        }

        public static NBiMember.CountConstraint MoreThan(int count)
        {
            var ctr = new NBiMember.CountConstraint();
            ctr.MoreThan(count);
            return ctr;
        }

        public static NBiMember.CountConstraint LessThan(int count)
        {
            var ctr = new NBiMember.CountConstraint();
            ctr.LessThan(count);
            return ctr;
        }

        public static NBiStructure.ContainsConstraint Structure(string value)
        {
            var ctr = new NBiStructure.ContainsConstraint(value);
            return ctr;
        }

        public static NBiStructure.ContainsConstraint Structure(NBi.Core.Analysis.Metadata.IFieldWithDisplayFolder folderAndField)
        {
            var ctr = new NBiStructure.ContainsConstraint(folderAndField);
            return ctr;
        }
    }
}

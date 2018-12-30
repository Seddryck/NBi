using NUnit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Runtime.Embed.Filter
{
    [Serializable]
    public class PropertyFilter : TestFilter
    {
        protected string Name { get; }
        protected string Value { get; }

        public PropertyFilter(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Check whether the filter matches a test
        /// </summary>
        /// <param name="test">The test to be matched</param>
        /// <returns></returns>
        public override bool Match(ITest test)
        {
            if (test.Properties == null)
                return false;
            if (!test.Properties.Contains(Name))
                return false;
            if (test.Properties[Name] as string == Value)
                return true;

            return false;
        }

        /// <summary>
        /// Return the string representation of a property filter
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{Name}::{Value}";

    }
}
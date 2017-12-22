using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query
{
    [AttributeUsage(AttributeTargets.Class)]

    class SupportedCommandTypeAttribute : Attribute
    {
        public Type Value { get; }

        public SupportedCommandTypeAttribute(Type type)
        {
            Value = type;
        }
    }
}

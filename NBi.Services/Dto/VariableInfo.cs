using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Service.Dto
{
    class VariableInfo
    {
        public string Name { get; set; }
        public VariableType Type { get; set; }

        public VariableInfo(string name)
        {
            Name = name;
            Type = VariableType.Text;
        }

        public VariableInfo(string name, VariableType type)
        {
            Name = name;
            Type = type;
        }
    }
}

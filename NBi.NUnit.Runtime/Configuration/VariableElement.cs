using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Runtime.Configuration
{
    public class VariableElement : ConfigurationElement
    {
        public VariableElement()
        { }

        public VariableElement(string name)
        : this(name, null, ColumnType.Text) { }

        public VariableElement(string name, string value)
        : this (name, value, ColumnType.Text) {}

        public VariableElement(string name, string value, ColumnType type)
        {
            Name = name;
            Value = value;
            Type = type;
        }


        [ConfigurationProperty("name",
            IsRequired = true,
            IsKey = true)]
        public string Name
        {
            get => (string)this["name"];
            set =>this["name"] = value;
        }

        [ConfigurationProperty("value",
            IsRequired = true )]
        public string Value
        {
            get => (string)this["value"];
            set => this["value"] = value;
        }

        [ConfigurationProperty("type",
            IsRequired = false)]
        public ColumnType Type
        {
            get => (ColumnType)this["type"];
            set => this["type"] = value;
        }
    }
}

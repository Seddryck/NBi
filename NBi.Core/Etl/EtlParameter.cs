using System;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Core.Etl
{
    public class EtlParameter
    {
        [XmlIgnore]
        public virtual string StringValue { get; set; }
        [XmlIgnore]
        public virtual string Name { get; set; }

        public EtlParameter()
        { }

        public EtlParameter(string name, object value)
            : this()
        {
            Name = name;
            StringValue = value.ToString();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EtlParameter))
                return false;
            
            var objParam = obj as EtlParameter;
            return (objParam.Name == this.Name && objParam.StringValue == this.StringValue);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() * StringValue.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Etl parameter name={0} ; value={1}", Name, StringValue);
        }
    }
}

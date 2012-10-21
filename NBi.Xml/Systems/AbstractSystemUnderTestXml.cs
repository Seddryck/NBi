using System;
using System.Xml.Serialization;
using NBi.Xml.Settings;

namespace NBi.Xml.Systems
{
    public abstract class AbstractSystemUnderTestXml : IAbstractSystemUnderTestXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        public virtual bool IsStructure()
        {
            return false;
        }

        public virtual bool IsQuery()
        {
            return false;
        }

        public virtual bool IsMembers()
        {
            return false;
        }
        

        public abstract object Instantiate();

        public DefaultXml Default { get; set; }
        public SettingsXml Settings { get; set; }

        public AbstractSystemUnderTestXml()
        {
            Default = new DefaultXml();
        }
    }
}

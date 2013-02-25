using System;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Items
{
    public abstract class QueryableXml : BaseItem
    {
        [XmlAttribute("connectionString")]
        public string ConnectionString { get; set; }
        
        public abstract string GetQuery();

        public virtual string GetConnectionString()
        {
            //if ConnectionString is specified then return it
            if (!string.IsNullOrEmpty(ConnectionString))
                return ConnectionString;

            //Else get the reference ConnectionString 
            //if (!string.IsNullOrEmpty(ConnectionStringReference))
            //return Settings.GetReference(ConnectionStringReference).ConnectionString;

            //Else get the default ConnectionString 
            if (Default != null && !string.IsNullOrEmpty(Default.ConnectionString))
                return Default.ConnectionString;
            return null;
        }
        

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Items
{
    public abstract class QueryableXml : ExecutableXml
    {       
        public abstract string GetQuery();

        public QueryableXml()
        {
            Parameters = new List<QueryParameterXml>();
        }

        [XmlElement("parameter")]
        public List<QueryParameterXml> Parameters { get; set; }

        [XmlElement("variable")]
        public List<QueryTemplateVariableXml> Variables { get; set; }

        public virtual List<QueryParameterXml> GetParameters()
        {
            var list = Parameters;
            foreach (var param in Default.Parameters)
                if (!Parameters.Exists(p => p.Name == param.Name))
                    list.Add(param);

            return list;
        }

        public virtual List<QueryTemplateVariableXml> GetVariables()
        {
            var list = Variables;
            foreach (var variable in Default.Variables)
                if (!Variables.Exists(p => p.Name == variable.Name))
                    list.Add(variable);

            return list;
        }
    }
}

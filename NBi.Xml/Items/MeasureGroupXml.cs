using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Items
{
    public class MeasureGroupXml : PerspectiveXml
    {
        [XmlAttribute("perspective")]
        public string Perspective { get; set; }

        public override object Instantiate()
        {
            //TODO here?
            return null;
        }


        [XmlIgnore]
        public override string TypeName
        {
            get { return "measure-group"; }
        }

        internal override Dictionary<string, string> GetRegexMatch()
        {
            var dico = base.GetRegexMatch();
            dico.Add("sut:perspective", Perspective);
            return dico;
        }
    }
}

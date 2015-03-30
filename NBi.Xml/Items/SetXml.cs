﻿using NBi.Xml.Items.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Items
{
    public class SetXml : AbstractMembersItem, IPerspectiveFilter
    {
        [XmlAttribute("perspective")]
        public string Perspective { get; set; }

        [XmlIgnore]
        protected virtual string Path { get { return string.Format("[{0}]", Caption); } }

        public override string TypeName
        {
            get { return "set"; }
        }

        internal override Dictionary<string, string> GetRegexMatch()
        {
            var dico = base.GetRegexMatch();
            dico.Add("sut:perspective", Perspective);
            return dico;
        }

        internal override ICollection<string> GetAutoCategories()
        {
            var values = new List<string>();
            if (!string.IsNullOrEmpty(Perspective))
                values.Add(string.Format("Perspective '{0}'", Perspective));
            values.Add(string.Format("Set '{0}'", Caption));
            return values;
        }
    }
}

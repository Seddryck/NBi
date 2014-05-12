﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Items
{
    public class PropertyXml : LevelXml
    {
        [XmlAttribute("level")]
        public string Level { get; set; }

        [XmlIgnore]
        protected override string ParentPath { get { return string.Format("[{0}].[{1}].[{2}]", Dimension, Hierarchy, Level); } }

        [XmlIgnore]
        protected override string Path { get { return string.Format("{0}.[{1}]", ParentPath, Caption); } }

        [XmlIgnore]
        public override string TypeName
        {
            get { return "property"; }
        }

        internal override Dictionary<string, string> GetRegexMatch()
        {
            var dico = base.GetRegexMatch();
            dico.Add("sut:level", Level);
            return dico;
        }

        internal override ICollection<string> GetAutoCategories()
        {
            var values = new List<string>();
            if (!string.IsNullOrEmpty(Perspective))
                values.Add(string.Format("Perspective '{0}'", Perspective));
            if (!string.IsNullOrEmpty(Dimension))
                values.Add(string.Format("Dimension '{0}'", Dimension));
            if (!string.IsNullOrEmpty(Hierarchy))
                values.Add(string.Format("Hierarchy '{0}'", Hierarchy));
            if (!string.IsNullOrEmpty(Level))
                values.Add(string.Format("Level '{0}'", Level));
            values.Add(string.Format("Property '{0}'", Caption));
            values.Add("Properties");
            return values;
        }
    }
}

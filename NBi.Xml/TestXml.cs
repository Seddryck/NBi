using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using NBi.Xml.Constraints;
using NBi.Xml.Settings;
using NBi.Xml.Systems;

namespace NBi.Xml
{
    public class TestXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        
        [XmlAttribute("uid")]
        public string UniqueIdentifier { get; set; }

        [XmlElement("ignore")]
        public IgnoreXml IgnoreElement { get; set; }
        [XmlAttribute("ignore")]
        [DefaultValue(false)]
        public bool Ignore
        {
            get
            {
                return (IgnoreElement != null);
            }
            set
            {
                if (value)
                {
                    if (IgnoreElement == null)
                        IgnoreElement = new IgnoreXml();
                }
                else
                {
                    IgnoreElement = null;
                }
            }
        }

        [XmlElement("description")]
        public DescriptionXml DescriptionElement { get; set; }
        [XmlAttribute("description")]
        [DefaultValue("")]
        public string Description
        {
            get
            {
                return DescriptionElement == null ? string.Empty : DescriptionElement.Value;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    DescriptionElement = null;
                }
                else
                {
                    if (DescriptionElement == null)
                        DescriptionElement = new DescriptionXml();
                    DescriptionElement.Value = value;
                }
            }
        }

        [XmlElement("category")]
        public List<string> Categories;

        [XmlArray("system-under-test"),
        XmlArrayItem(Type = typeof(ExecutionXml), ElementName = "execution"),
        XmlArrayItem(Type = typeof(MembersXml), ElementName = "members"),
        XmlArrayItem(Type = typeof(StructureXml), ElementName = "structure"),
        ]
        public List<AbstractSystemUnderTestXml> Systems;

        [XmlArray("assert"),
        XmlArrayItem(Type = typeof(SyntacticallyCorrectXml), ElementName = "syntacticallyCorrect"),
        XmlArrayItem(Type = typeof(FasterThanXml), ElementName = "fasterThan"),
        XmlArrayItem(Type = typeof(EqualToXml), ElementName = "equalTo"),
        XmlArrayItem(Type = typeof(CountXml), ElementName = "count"),
        XmlArrayItem(Type = typeof(ContainsXml), ElementName = "contains"),
        XmlArrayItem(Type = typeof(ExistsXml), ElementName = "exists"),
        XmlArrayItem(Type = typeof(OrderedXml), ElementName = "ordered"),
        ]
        public List<AbstractConstraintXml> Constraints;

        public TestXml()
        {
            Constraints = new List<AbstractConstraintXml>();
            Systems = new List<AbstractSystemUnderTestXml>();
            Categories = new List<string>();
        }

        public TestXml(TestStandaloneXml standalone)
        {
            this.Name = standalone.Name;
            this.DescriptionElement = standalone.DescriptionElement;
            this.IgnoreElement = standalone.IgnoreElement;
            this.Categories = standalone.Categories;
            this.Constraints = standalone.Constraints;
            this.Systems = standalone.Systems;
            this.UniqueIdentifier = standalone.UniqueIdentifier;
        }

        public string GetName()
        {
            string newName = Name;
            if (Systems[0] != null)
            {
                var vals = Systems[0].GetRegexMatch();

                Regex re = new Regex(@"\{(sut:([a-z\-])*?)\}", RegexOptions.Compiled);
                string key = string.Empty;
                try
                {

                    newName = re.Replace(Name, delegate(Match match)
                                {
                                    key = match.Groups[1].Value;
                                    return vals[key];
                                });
                }
                catch (KeyNotFoundException)
                {
                    Console.WriteLine(string.Format("Unknown tag '{0}' in test name has stopped the replacement of tag in test name", key));
                }
            }
            return newName;
        }

        [XmlIgnore()]
        public string Content { get; set; }

        [XmlIgnore()]
        public string IgnoreReason
        {
            get
            {
                if (IgnoreElement == null)
                    return string.Empty;
                else
                    return IgnoreElement.Reason;
            }
            set
            {
                if (IgnoreElement == null)
                    Ignore = true;

                IgnoreElement.Reason = value;
            }
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.Name))
                return base.ToString();
            else
                return Name.ToString();
        }

    }
}
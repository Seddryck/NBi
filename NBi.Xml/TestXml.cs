using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using NBi.Xml.Constraints;
using NBi.Xml.Decoration;
using NBi.Xml.Decoration.Command;
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

        [XmlElement("ignore", Order=1)]
        public IgnoreXml IgnoreElement { get; set; }
        [XmlIgnore]
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

        [XmlElement("description", Order = 2)]
        public DescriptionXml DescriptionElement { get; set; }
        [XmlIgnore]
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

        [XmlElement("edition", Order = 3)]
        public EditionXml Edition;

        [XmlElement("category", Order = 4)]
        public List<string> Categories;

        [XmlAttribute("timeout")]
        [DefaultValue(0)]
        public int Timeout { get; set; }

        [XmlElement("condition", Order = 5)]
        public ConditionXml Condition;

        [XmlElement("setup", Order = 6)]
        public SetupXml Setup;

        [XmlArray("system-under-test", Order = 7),
        XmlArrayItem(Type = typeof(ExecutionXml), ElementName = "execution"),
        XmlArrayItem(Type = typeof(MembersXml), ElementName = "members"),
        XmlArrayItem(Type = typeof(StructureXml), ElementName = "structure"),
        ]
        public List<AbstractSystemUnderTestXml> Systems;

        [XmlArray("assert", Order = 8),
        XmlArrayItem(Type = typeof(SyntacticallyCorrectXml), ElementName = "syntacticallyCorrect"),
        XmlArrayItem(Type = typeof(FasterThanXml), ElementName = "fasterThan"),
        XmlArrayItem(Type = typeof(EqualToXml), ElementName = "equalTo"),
        XmlArrayItem(Type = typeof(CountXml), ElementName = "count"),
        XmlArrayItem(Type = typeof(ContainXml), ElementName = "contain"),
        XmlArrayItem(Type = typeof(ExistsXml), ElementName = "exists"),
        XmlArrayItem(Type = typeof(OrderedXml), ElementName = "ordered"),
        XmlArrayItem(Type = typeof(LinkedToXml), ElementName = "linkedTo"),
        XmlArrayItem(Type = typeof(SubsetOfXml), ElementName = "subsetOf"),
        XmlArrayItem(Type = typeof(EquivalentToXml), ElementName = "equivalentTo"),
        XmlArrayItem(Type = typeof(MatchPatternXml), ElementName = "matchPattern"),
        XmlArrayItem(Type = typeof(EvaluateRowsXml), ElementName = "evaluate-rows"),
        XmlArrayItem(Type = typeof(SuccessfulXml), ElementName = "successful"),
        ]
        public List<AbstractConstraintXml> Constraints;

        [XmlElement("cleanup", Order = 9)]
        public CleanupXml Cleanup;

        public TestXml()
        {
            Constraints = new List<AbstractConstraintXml>();
            Systems = new List<AbstractSystemUnderTestXml>();
            Categories = new List<string>();
            Setup = new SetupXml();
            Cleanup = new CleanupXml();
            Condition = new ConditionXml();
            GroupNames = new List<string>();
        }

        public TestXml(TestStandaloneXml standalone)
        {
            this.Name = standalone.Name;
            this.DescriptionElement = standalone.DescriptionElement;
            this.IgnoreElement = standalone.IgnoreElement;
            this.Categories = standalone.Categories;
            this.Constraints = standalone.Constraints;
            this.Setup = standalone.Setup;
            this.Cleanup = standalone.Cleanup;
            this.Systems = standalone.Systems;
            this.UniqueIdentifier = standalone.UniqueIdentifier;
            this.Edition = standalone.Edition;
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


        internal void AddInheritedCategories(List<string> categories)
        {
            Categories.AddRange(categories);
        }

        [XmlIgnore()]
        public IList<string> GroupNames { get; private set; }
    }
}
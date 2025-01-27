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
using System.Xml;
using NBi.Xml.Variables;
using NBi.Core.Variable;

namespace NBi.Xml;

public class TestXml : InheritanceTestXml
{

    [XmlAttribute("name")]
    [DefaultValue("")]
    public string Name { get; set; } = string.Empty;

    [XmlAttribute("uid")]
    [DefaultValue("")]
    public string UniqueIdentifier { get; set; } = string.Empty;

    [XmlElement("ignore", Order = 1)]
    public IgnoreXml? IgnoreElement { get; set; }
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

    [XmlElement("instance-settling", Order = 2)]
    public InstanceSettlingXml InstanceSettling { get; set; } = InstanceSettlingXml.Unique;

    [XmlElement("description", Order = 3)]
    public DescriptionXml? DescriptionElement { get; set; }
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

    [XmlElement("edition", Order = 4)]
    public EditionXml? Edition;

    [XmlElement("category", Order = 5)]
    public List<string> Categories
    {
        get { return categories; }
        set { categories = value; }
    }

    [XmlElement("trait", Order = 6)]
    public List<TraitXml> Traits
    {
        get { return traits; }
        set { traits = value; }
    }

    [XmlAttribute("timeout")]
    [DefaultValue(0)]
    public int Timeout { get; set; }

    [XmlElement("condition", Order = 7)]
    public ConditionXml Condition = new();

    [XmlElement("setup", Order = 8)]
    public SetupXml Setup
    {
        get { return setup; }
        set { setup = value; }
    }

    [XmlArray("system-under-test", Order = 9),
    XmlArrayItem(Type = typeof(ExecutionXml), ElementName = "execution"),
    XmlArrayItem(Type = typeof(ResultSetSystemXml), ElementName = "result-set"),
    XmlArrayItem(Type = typeof(ScalarXml), ElementName = "scalar"),
    XmlArrayItem(Type = typeof(MembersXml), ElementName = "members"),
    XmlArrayItem(Type = typeof(StructureXml), ElementName = "structure"),
    XmlArrayItem(Type = typeof(DataTypeXml), ElementName = "data-type"),
    ]
    public List<AbstractSystemUnderTestXml> Systems { get; set; } = [];

    [XmlArray("assert", Order = 10),
    XmlArrayItem(Type = typeof(SyntacticallyCorrectXml), ElementName = "syntactically-correct"),
    XmlArrayItem(Type = typeof(FasterThanXml), ElementName = "faster-than"),
    XmlArrayItem(Type = typeof(EqualToXml), ElementName = "equal-to"),
    XmlArrayItem(Type = typeof(SupersetOfXml), ElementName = "superset-of"),
    XmlArrayItem(Type = typeof(SubsetOfXml), ElementName = "subset-of"),
     XmlArrayItem(Type = typeof(IntersectionOfXml), ElementName = "intersection-of"),
    XmlArrayItem(Type = typeof(CountXml), ElementName = "count"),
    XmlArrayItem(Type = typeof(ContainXml), ElementName = "contain"),
    XmlArrayItem(Type = typeof(ExistsXml), ElementName = "exists"),
    XmlArrayItem(Type = typeof(OrderedXml), ElementName = "ordered"),
    XmlArrayItem(Type = typeof(LinkedToXml), ElementName = "linkedTo"),
    XmlArrayItem(Type = typeof(ContainedInXml), ElementName = "contained-in"),
    XmlArrayItem(Type = typeof(EquivalentToXml), ElementName = "equivalent-to"),
    XmlArrayItem(Type = typeof(MatchPatternXml), ElementName = "matchPattern"),
    XmlArrayItem(Type = typeof(EvaluateRowsXml), ElementName = "evaluate-rows"),
    XmlArrayItem(Type = typeof(SuccessfulXml), ElementName = "successful"),
    XmlArrayItem(Type = typeof(RowCountXml), ElementName = "row-count"),
    XmlArrayItem(Type = typeof(AllRowsXml), ElementName = "all-rows"),
    XmlArrayItem(Type = typeof(SomeRowsXml), ElementName = "some-rows"),
    XmlArrayItem(Type = typeof(SingleRowXml), ElementName = "single-row"),
    XmlArrayItem(Type = typeof(NoRowsXml), ElementName = "no-rows"),
    XmlArrayItem(Type = typeof(IsXml), ElementName = "is"),
    XmlArrayItem(Type = typeof(UniqueRowsXml), ElementName = "unique-rows"),
    XmlArrayItem(Type = typeof(LookupExistsXml), ElementName = "lookup-exists"),
    XmlArrayItem(Type = typeof(LookupMatchesXml), ElementName = "lookup-matches"),
    XmlArrayItem(Type = typeof(ScoreXml), ElementName = "score"),
    ]
    public List<AbstractConstraintXml> Constraints { get; set; } = [];

    [XmlElement("cleanup", Order = 11)]
    public CleanupXml Cleanup
    {
        get { return cleanup; }
        set { cleanup = value; }
    }

    [XmlElement("not-implemented", Order = 12)]
    public IgnoreXml? NotImplemented { get; set; }

    [XmlAnyElement(Order = 13)]
    public List<XmlElement> Drafts { get; set; } = [];

    public TestXml()
    { }
    public TestXml(TestStandaloneXml standalone)
    {
        Categories = standalone.Categories;
        Cleanup = standalone.Cleanup;
        Condition = standalone.Condition;
        Constraints = standalone.Constraints;
        DescriptionElement = standalone.DescriptionElement;
        Drafts = standalone.Drafts;
        Edition = standalone.Edition;
        IgnoreElement = standalone.IgnoreElement;
        InstanceSettling = standalone.InstanceSettling;
        Name = standalone.Name;
        Setup = standalone.Setup;
        Systems = standalone.Systems;
        Traits = standalone.Traits;
        UniqueIdentifier = standalone.UniqueIdentifier;
        NotImplemented = standalone.NotImplemented;
        Content = standalone.Content;
    }

    public string GetName()
    {
        string newName = Name;
        if (Systems.Count > 0 && Systems[0] != null)
        {
            var vals = Systems[0].GetRegexMatch();

            var re = new Regex(@"\{(sut:([a-z\-])*?)\}", RegexOptions.Compiled);
            string key = string.Empty;
            try
            {

                newName = re.Replace(Name, delegate (Match match)
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

    public string GetName(IDictionary<string, IVariable> dico)
    {
        var newName = GetName() + ".";
        foreach (var token in dico)
            newName = $"{newName}{token.Key}={token.Value};";
        if (newName.EndsWith(";"))
            newName = newName.PadLeft(newName.Length - 1);

        return newName;
    }

    [XmlIgnore()]
    public string Content { get; set; } = string.Empty;

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

            IgnoreElement!.Reason = value;
        }
    }

    public override string ToString()
    {
        if (string.IsNullOrEmpty(Name))
            return base.ToString() ?? string.Empty;
        else
            return Name.ToString();
    }

    [XmlIgnore()]
    public IList<string> GroupNames { get; private set; } = [];

    [XmlIgnore]
    public bool ConditionSpecified
    {
        get
        {
            return !(
                        Condition == null
                        || (
                                (Condition.Predicates == null || Condition.Predicates.Count == 0)
                           )
                     );
        }
        set { return; }
    }

    [XmlIgnore]
    public bool InstanceSettlingSpecified { get => !(InstanceSettling==InstanceSettlingXml.Unique); }
    [XmlIgnore]
    public bool SystemsSpecified { get => !(Systems == null || Systems.Count == 0); }
    [XmlIgnore]
    public bool ConstraintsSpecified { get => !(Constraints == null || Constraints.Count == 0); }

    [XmlIgnore]
    public bool SetupSpecified
    {
        get => (Setup?.Commands?.Count ?? 0) != 0;
        set { return; }
    }

    [XmlIgnore]
    public bool CleanupSpecified
    {
        get => (Cleanup?.Commands?.Count ?? 0) != 0;
        set { return; }
    }

    [XmlIgnore]
    public bool IsNotImplemented { get => NotImplemented != null; }

}

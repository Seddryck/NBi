using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NBi.Model.Constraints;
using NBi.Model.Decoration;
using NBi.Model.Decoration.Command;
using NBi.Model.Settings;
using NBi.Model.Systems;
using NBi.Model.Variables;
using NBi.Core.Variable;

namespace NBi.Model;

public class TestDefinition : InheritanceTest
{
    public string Name { get; set; } = string.Empty;

    public string UniqueIdentifier { get; set; } = string.Empty;

    public Ignore? IgnoreElement { get; set; }
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
                    IgnoreElement = new Ignore();
            }
            else
            {
                IgnoreElement = null;
            }
        }
    }

    public InstanceSettling InstanceSettling { get; set; } = InstanceSettling.Unique;

    public Description? DescriptionElement { get; set; }
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
                    DescriptionElement = new Description();
                DescriptionElement.Value = value;
            }
        }
    }

    public Edition? Edition;

    public List<string> Categories
    {
        get { return categories; }
        set { categories = value; }
    }

    public List<Trait> Traits
    {
        get { return traits; }
        set { traits = value; }
    }

    public int Timeout { get; set; }

    public TestCondition Condition = new();

    public Setup Setup
    {
        get { return setup; }
        set { setup = value; }
    }

    public List<AbstractSystemUnderTest> Systems { get; set; } = [];

    public List<AbstractConstraint> Constraints { get; set; } = [];

    public Cleanup Cleanup
    {
        get { return cleanup; }
        set { cleanup = value; }
    }

    public Ignore? NotImplemented { get; set; }

    public List<string> Drafts { get; set; } = [];

    public TestDefinition()
    { }
    public TestDefinition(StandaloneTest standalone)
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
                newName = re.Replace(Name,  match =>                             {
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

    public string Content { get; set; } = string.Empty;

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

    public IList<string> GroupNames { get; private set; } = [];

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

    public bool InstanceSettlingSpecified { get => !(InstanceSettling==InstanceSettling.Unique); }
    public bool SystemsSpecified { get => !(Systems == null || Systems.Count == 0); }
    public bool ConstraintsSpecified { get => !(Constraints == null || Constraints.Count == 0); }

    public bool SetupSpecified
    {
        get => (Setup?.Commands?.Count ?? 0) != 0;
        set { return; }
    }

    public bool CleanupSpecified
    {
        get => (Cleanup?.Commands?.Count ?? 0) != 0;
        set { return; }
    }

    public bool IsNotImplemented { get => NotImplemented != null; }
}

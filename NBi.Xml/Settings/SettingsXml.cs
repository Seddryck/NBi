using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Serialization;

namespace NBi.Xml.Settings;

public class SettingsXml
{
    [XmlIgnore]
    public string? BasePath { get; set; }

    [XmlElement("default")]
    public List<DefaultXml> Defaults { get; set; }
    [XmlElement("reference")]
    public List<ReferenceXml> References { get; set; }

    [XmlElement("parallelize-queries")]
    [DefaultValue(true)]
    public bool ParallelizeQueries { get; set; } = true;

    [XmlElement("csv-profile")]
    public CsvProfileXml CsvProfile { get; set; }

    public bool ShouldSerializeCsvProfile()
    {
        var value = 
            CsvProfile.InternalFieldSeparator != GetDefaultValue<string>(x => x.InternalFieldSeparator)
            || CsvProfile.InternalRecordSeparator != GetDefaultValue<string>(x => x.InternalRecordSeparator)
            || CsvProfile.InternalTextQualifier != GetDefaultValue<string>(x => x.InternalTextQualifier)
            || CsvProfile.InternalEmptyCell != GetDefaultValue<string>(x => x.InternalEmptyCell)
            || CsvProfile.InternalMissingCell != GetDefaultValue<string>(x => x.InternalMissingCell)
            || CsvProfile.InternalFirstRowHeader != GetDefaultValue<bool>(x => x.InternalFirstRowHeader);

        return value;
    }

    public enum DefaultScope
    {
        [XmlEnum(Name = "everywhere")]
        Everywhere,
        [XmlEnum(Name = "variable")]
        Variable,
        [XmlEnum(Name = "system-under-test")]
        SystemUnderTest,
        [XmlEnum(Name = "assert")]
        Assert,
        [XmlEnum(Name = "setup-cleanup")]
        Decoration
    }

    public DefaultXml GetDefault(DefaultScope scope)
    {
        var def = Defaults.SingleOrDefault(d => d.ApplyTo == scope);
        //Don't throw exception ... generating lot of issues!
        //in place create it (and also add it to the collection)
        if (def == null)
        { 
            def=new DefaultXml(scope);
            Defaults.Add(def);
        }

        //Add the defaults from "everywhere", if they are not overriden in the scope
        var defEverywhere = Defaults.SingleOrDefault(d => d.ApplyTo == DefaultScope.Everywhere);
        if (defEverywhere != null)
        {
            if (!def.ConnectionStringSpecified)
                def.ConnectionString = defEverywhere.ConnectionString;

            foreach (var param in defEverywhere.Parameters)
                if (def.Parameters.SingleOrDefault(p => p.Name == param.Name) == null)
                    def.Parameters.Add(param);

            foreach (var variable in defEverywhere.Variables)
                if (def.Variables.SingleOrDefault(v => v.Name == variable.Name) == null)
                    def.Variables.Add(variable);
        }

        return def;           
    }

    public ReferenceXml GetReference(string name)
    {
        try
        {
            var reference = References.SingleOrDefault(d => d.Name == name);
            if (reference == null)
                throw new ArgumentOutOfRangeException(string.Format("No reference named '{0}' existing in test suite's settings.", name));
            return reference;
        }
        catch (System.InvalidOperationException)
        {
            throw new InvalidOperationException(string.Format("All references'name must be unique in settings. The name '{0}' exists more than once.", name));
        }
        
    }

    public SettingsXml()
    {
        Defaults = new List<DefaultXml>();
        References = new List<ReferenceXml>();
        ParallelizeQueries = true;
        CsvProfile = new CsvProfileXml
        (
            GetDefaultValue<string>(x => x.InternalFieldSeparator)[0]
            , '\"'
            , GetDefaultValue<string>(x => x.InternalRecordSeparator)
        );
    }

    internal void GetValuesFromConfig(NameValueCollection connectionStrings)
    {
        foreach (var def in Defaults)
        {
            if (!string.IsNullOrEmpty(def.ConnectionString.Inline) && def.ConnectionString.Inline.StartsWith("@"))
            {
                if (connectionStrings.Count == 0)
                    throw new ArgumentOutOfRangeException($"No connection-string is provided through the config file. The default connection string stipulated in the nbits file is trying to reference a connection string named '{def.ConnectionString.Inline}'");

                var key = connectionStrings.AllKeys.SingleOrDefault(k => k == def.ConnectionString.Inline.Substring(1) || k == def.ConnectionString.Inline);
                if (string.IsNullOrEmpty(key))
                    throw new ArgumentOutOfRangeException($"Some connection-strings are provided through the config file but the default connection string is trying to reference a connection string named '{def.ConnectionString.Inline}' which has not been found.");

                def.ConnectionString.Inline = connectionStrings.Get(key) ?? string.Empty;
            }
        }

        foreach (var reference in References)
        {
            if (!string.IsNullOrEmpty(reference.ConnectionString.Inline) && reference.ConnectionString.Inline.StartsWith("@"))
            {
                if (connectionStrings.Count == 0)
                    throw new ArgumentOutOfRangeException(string.Format("No connectionString is provided through the config file. The connection string named '{0}' has not been found and cannot be created as a reference.", reference.ConnectionString));

                var key = connectionStrings.AllKeys.SingleOrDefault(k => k == reference.ConnectionString.Inline.Substring(1) || k == reference.ConnectionString.Inline);
                if (string.IsNullOrEmpty(key))
                    throw new ArgumentOutOfRangeException(string.Format("Some connectionStrings are provided through the config file but a reference connection string is trying to reference a connection string named '{0}' which has not been found.", reference.ConnectionString));

                reference.ConnectionString.Inline = connectionStrings.Get(key) ?? string.Empty;
            }
        }
    }

    public static SettingsXml Empty
    {
        get
        {
            return new SettingsXml();
        }
    }

    public static T GetDefaultValue<T>(Expression<Func<CsvProfileXml, T>> propertySelector)
    {
        MemberExpression? memberExpression = null;

        switch (propertySelector.Body.NodeType)
        {
            case ExpressionType.MemberAccess:
                // This is the default case where the 
                // expression is simply member access.
                memberExpression
                    = propertySelector.Body as MemberExpression;
                break;

            case ExpressionType.Convert:
                // This case deals with conversions that 
                // may have occured due to typing.

                if (propertySelector.Body is UnaryExpression unaryExpression)
                {
                    memberExpression
                        = unaryExpression.Operand as MemberExpression;
                }
                break;
        }


        var member = memberExpression!.Member;

        // Check for field and property types. 
        // All other types are not supported by attribute model.
        switch (member.MemberType)
        {
            case MemberTypes.Property:
                break;
            default:
                throw new Exception("Member is not property");
        }

        var property = (PropertyInfo)member;

        return property.GetCustomAttribute(typeof(DefaultValueAttribute)) switch
        {
            DefaultValueAttribute attribute => (T)(attribute.Value ?? throw new NullReferenceException()),
            _ => throw new ArgumentException(),
        };
    }
}

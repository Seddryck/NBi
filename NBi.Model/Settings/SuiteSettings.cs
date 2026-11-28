using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NBi.Model.Settings;

public class SuiteSettings
{
    public string? BasePath { get; set; }

    public List<Default> Defaults { get; set; }
    public List<Reference> References { get; set; }

    public bool ParallelizeQueries { get; set; } = true;

    public CsvProfile CsvProfile { get; set; }

    public enum DefaultScope
    {
        Everywhere,
        Variable,
        SystemUnderTest,
        Assert,
        Decoration
    }

    public Default GetDefault(DefaultScope scope)
    {
        var def = Defaults.SingleOrDefault(d => d.ApplyTo == scope);
        //Don't throw exception ... generating lot of issues!
        //in place create it (and also add it to the collection)
        if (def == null)
        {
            def=new Default(scope);
            Defaults.Add(def);
        }

        //Add the defaults from "everywhere", if they are not overriden in the scope
        var defEverywhere = Defaults.SingleOrDefault(d => d.ApplyTo == DefaultScope.Everywhere);
        if (defEverywhere != null)
        {
            if (!def.ConnectionStringSpecified)
                def.ConnectionString = defEverywhere.ConnectionString;

            foreach (var param in defEverywhere.Parameters)
            {
                if (def.Parameters.SingleOrDefault(p => p.Name == param.Name) == null)
                    def.Parameters.Add(param);
            }

            foreach (var variable in defEverywhere.Variables)
            {
                if (def.Variables.SingleOrDefault(v => v.Name == variable.Name) == null)
                    def.Variables.Add(variable);
            }
        }

        return def;
    }

    public Reference GetReference(string name)
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

    public SuiteSettings()
    {
        Defaults = new List<Default>();
        References = new List<Reference>();
        ParallelizeQueries = true;
        CsvProfile = new CsvProfile();
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

    public static SuiteSettings Empty
    {
        get
        {
            return new SuiteSettings();
        }
    }
}

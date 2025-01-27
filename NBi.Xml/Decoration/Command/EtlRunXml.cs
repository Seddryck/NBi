using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Etl;
using NBi.Xml.Items;
using System.ComponentModel;
using NBi.Xml.Settings;
using System.Reflection;
using NBi.Xml.Constraints;

namespace NBi.Xml.Decoration.Command;

public class EtlRunXml : DecorationCommandXml, IReferenceFriendly
{
    protected const int DEFAULT_TIMEOUT = 30;
    protected const string DEFAULT_VERSION = "SqlServer2014";

    [XmlAttribute("version")]
    public string? Version { get; set; }
    
    [XmlAttribute("server")]
    public string? Server { get; set; }

    [XmlAttribute("path")]
    public string? Path { get; set; }

    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("username")]
    public string? UserName { get; set; }

    [XmlAttribute("password")]
    public string? Password { get; set; }

    [XmlAttribute("catalog")]
    public string? Catalog { get; set; }

    [XmlAttribute("folder")]
    public string? Folder { get; set; }

    [XmlAttribute("project")]
    public string? Project { get; set; }

    [XmlAttribute("environment")]
    public string? Environment { get; set; }

    [XmlAttribute("bits-32")]
    public bool Is32Bits { get; set; } = false;

    [DefaultValue(DEFAULT_TIMEOUT)]
    [XmlAttribute("timeout")]
    public int Timeout { get; set; } = DEFAULT_TIMEOUT;

    [XmlElement("parameter")]
    public List<EtlParameterXml> Parameters { get; set; } = [];

    public EtlRunXml()
    {
        Version = "SqlServer2014";
        Timeout = DEFAULT_TIMEOUT;
    }

    public void AssignReferences(IEnumerable<ReferenceXml> references)
    {
        var properties = typeof(EtlBaseXml).GetProperties(BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance).Where(p => p.PropertyType == typeof(string));

        foreach (var property in properties)
            AssignDefaultOrReference(property.Name, references);
    }

    private void AssignDefaultOrReference(string propertyName, IEnumerable<ReferenceXml> references)
    {
        var property = GetType().GetProperty(propertyName) ?? throw new NullReferenceException(propertyName);

        if (property.PropertyType == typeof(string))
        {
            var currentValue = (string?)property.GetValue(this, null);
            var etl = typeof(EtlBaseXml).GetProperty(propertyName) ?? throw new NullReferenceException(propertyName);

            if (string.IsNullOrEmpty(currentValue))
            {
                var defaultValue = etl?.GetValue(Default.Etl, null);
                property.SetValue(this, defaultValue);
            }
            else if (currentValue.StartsWith("@"))
            {
                var refName = currentValue[1..];
                var refChoice = GetReference(references, refName) ?? throw new NullReferenceException(string.Format("A reference named '{0}' has been found, but no element 'etl' has been defined", refName));

                var referenceValue = etl.GetValue(refChoice.Etl, null);
                property.SetValue(this, referenceValue);
            }
        }
    }

    protected ReferenceXml GetReference(IEnumerable<ReferenceXml> references, string value)
    {
        if (!references.Any())
            throw new InvalidOperationException("No reference has been defined for this constraint");

        return references.FirstOrDefault(r => r.Name == value) ?? throw new IndexOutOfRangeException($"No reference named '{value}' has been defined.");
    }
}

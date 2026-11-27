using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Model.Constraints;
using NBi.Model.Settings;
using System.Reflection;

namespace NBi.Model.Items;

public class Etl: Executable, IReferenceFriendly
{
    protected const int DEFAULT_TIMEOUT = 30;
    protected const string DEFAULT_VERSION = "SqlServer2014";

    public string? Version { get; set; }

    public string? Server { get; set; }

    public string? Path { get; set; }

    public string? Name { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? Catalog { get; set; }

    public string? Folder { get; set; }

    public string? Project { get; set; }

    public string? Environment { get; set; }

    public bool Is32Bits { get; set; }

    public int Timeout { get; set; }

    public List<EtlParameter> Parameters { get; set; } = new List<EtlParameter>();

    public Etl()
    {
        Version = DEFAULT_VERSION;
    }

    public void AssignReferences(IEnumerable<Reference> references)
    {
        var properties = typeof(EtlBase).GetProperties(BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance).Where(p => p.PropertyType == typeof(string));

        foreach (var property in properties)
            AssignDefaultOrReference(property.Name, references);
    }

    private void AssignDefaultOrReference(string propertyName, IEnumerable<Reference> references)
    {
        var property = GetType().GetProperty(propertyName) ?? throw new NullReferenceException(propertyName);
        if (property.PropertyType==typeof(string))
        {
            var currentValue = (string?)property.GetValue(this, null);
            var etl = typeof(EtlBase).GetProperty(propertyName) ?? throw new NullReferenceException(propertyName);

            if (string.IsNullOrEmpty(currentValue))
            {
                var defaultValue = etl.GetValue(Default!.Etl, null);
                property.SetValue(this, defaultValue);
            }
            else if (currentValue.StartsWith("@"))
            {
                var refName = currentValue[1..];
                var refChoice = GetReference(references, refName);
                if (refChoice.Etl == null)
                    throw new NullReferenceException(string.Format("A reference named '{0}' has been found, but no element 'etl' has been defined", refName));

                var referenceValue = etl.GetValue(refChoice.Etl, null);
                property.SetValue(this, referenceValue);
            }
        }
    }

    protected Reference GetReference(IEnumerable<Reference> references, string value)
    {
        if (!references.Any())
            throw new InvalidOperationException("No reference has been defined for this constraint");

        return references.FirstOrDefault(r => r.Name == value) ?? throw new IndexOutOfRangeException($"No reference named '{value}' has been defined.");
    }
}

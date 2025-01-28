using NBi.Core;
using NBi.Core.FlatFile;
using NBi.Extensibility.FlatFile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Settings;

public class CsvProfileXml : IFlatFileProfile
{
    public CsvProfileXml()
        : this(';', '\"', "CrLf")
    { }

    public CsvProfileXml(char fieldSeparator, char textQualifier, string recordSeparator)
        : this(fieldSeparator, textQualifier, recordSeparator, false, "(empty)", "(null)")
    { }

    protected CsvProfileXml(char fieldSeparator, char textQualifier, string recordSeparator, bool firstRowHeader, string emptyCell, string missingCell)
        => (FieldSeparator, TextQualifier, RecordSeparator, FirstRowHeader, EmptyCell, MissingCell)
            = (fieldSeparator, textQualifier, recordSeparator, firstRowHeader, emptyCell, missingCell);

    [XmlAttribute("field-separator")]
    [DefaultValue(";")]
    public string InternalFieldSeparator { get; set; } = string.Empty;

    [XmlIgnore]
    public char FieldSeparator
    {
        get => (InternalFieldSeparator.Replace("Tab", "\t").Length <= 1 ? InternalFieldSeparator.Replace("Tab", "\t")[0] : throw new ArgumentOutOfRangeException());
        set => InternalFieldSeparator = value.ToString().Replace("\t", "Tab");
    }

    [XmlAttribute("text-qualifier")]
    [DefaultValue("Double-quote")]
    public string InternalTextQualifier { get; set; } = string.Empty;

    [XmlIgnore]
    public char TextQualifier
    {
        get => (InternalTextQualifier.Replace("Double-quote", "\"").Replace("Single-quote", "\'").Length <= 1 ? InternalTextQualifier.Replace("Double-quote", "\"").Replace("Single-quote", "\'")[0] : throw new ArgumentOutOfRangeException());
        set => InternalTextQualifier = value.ToString().Replace("\"", "Double-quote").Replace("\'", "Single-quote");
    }

    [XmlAttribute("record-separator")]
    [DefaultValue("CrLf")]
    public string InternalRecordSeparator { get; set; } = string.Empty ;

    [XmlIgnore]
    public string RecordSeparator 
    {
        get => InternalRecordSeparator.Replace("Cr", "\r").Replace("Lf", "\n");
        set => InternalRecordSeparator = value.Replace("\r", "Cr").Replace("\n", "Lf");
    }

    [XmlAttribute("first-row-header")]
    [DefaultValue(false)]
    public bool InternalFirstRowHeader { get; set; }

    [XmlIgnore]
    public bool FirstRowHeader
    {
        get => InternalFirstRowHeader;
        set => InternalFirstRowHeader = value;
    }

    [XmlAttribute("empty-cell")]
    [DefaultValue("(empty)")]
    public string InternalEmptyCell { get; set; } = string.Empty;

    [XmlIgnore]
    public string EmptyCell
    {
        get => string.IsNullOrEmpty(InternalEmptyCell) ? "(empty)" : InternalEmptyCell;
        set => InternalEmptyCell = value;
    }

    [XmlAttribute("missing-cell")]
    [DefaultValue("(null)")]
    public string InternalMissingCell { get; set; } = string.Empty;

    [XmlIgnore]
    public string MissingCell
    {
        get => string.IsNullOrEmpty(InternalMissingCell) ? "(null)" : InternalMissingCell;
        set => InternalMissingCell = value;
    }

    [XmlIgnore]
    public IDictionary<string, object> Attributes
    {
        get => new Dictionary<string, object>()
            {
                { GetXmlSerialization(x => x.InternalFieldSeparator), FieldSeparator },
                { GetXmlSerialization(x => x.InternalTextQualifier), TextQualifier },
                { GetXmlSerialization(x => x.InternalRecordSeparator), RecordSeparator },
                { GetXmlSerialization(x => x.InternalFirstRowHeader), FirstRowHeader },
                { GetXmlSerialization(x => x.InternalMissingCell), MissingCell },
                { GetXmlSerialization(x => x.InternalEmptyCell), EmptyCell },
            };
    }

    private static string GetXmlSerialization<T>(Expression<Func<CsvProfileXml, T>> propertySelector)
    {
        var memberExpression = propertySelector.Body.NodeType switch
        {
            // This is the default case where the expression is simply member access.
            ExpressionType.MemberAccess => propertySelector.Body as MemberExpression,
            // This case deals with conversions that may have occured due to typing.
            ExpressionType.Convert => (propertySelector.Body as UnaryExpression)?.Operand as MemberExpression,
            _ => throw new NotImplementedException()
        };

        var member = memberExpression!.Member;

        // Check for field and property types. 
        // All other types are not supported by attribute model.
        if (member.MemberType != MemberTypes.Property)
            throw new Exception("Member '{member}' is not property. Only properties are supported.");

        var property = (PropertyInfo)member;

        var attribute = (XmlAttributeAttribute)(property
            .GetCustomAttribute(typeof(XmlAttributeAttribute))
            ?? throw new ArgumentException());

        return attribute.AttributeName;
    }
}

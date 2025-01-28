using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace NBi.Core.DataSerialization.Flattening.Xml;

class XmlWrappingReader : XmlReader, IXmlLineInfo
{
    protected XmlReader Reader { get; }

    public XmlWrappingReader(XmlReader baseReader)
        => Reader = baseReader;

    public override XmlReaderSettings? Settings { get => Reader.Settings; }
    public override XmlNodeType NodeType { get => Reader.NodeType; }
    public override string Name { get => Reader.Name; }
    public override string LocalName { get => Reader.LocalName; }
    public override string NamespaceURI { get => Reader.NamespaceURI; }
    public override string Prefix { get => Reader.Prefix; }
    public override bool HasValue { get => Reader.HasValue; }
    public override string Value { get => Reader.Value; }
    public override int Depth { get => Reader.Depth; }
    public override string BaseURI { get => Reader.BaseURI; }
    public override bool IsEmptyElement { get => Reader.IsEmptyElement; }
    public override bool IsDefault { get => Reader.IsDefault; }
    public override XmlSpace XmlSpace { get => Reader.XmlSpace; }
    public override string XmlLang { get => Reader.XmlLang; }
    public override Type ValueType { get => Reader.ValueType; }
    public override int AttributeCount { get => Reader.AttributeCount; }
    public override bool EOF { get => Reader.EOF; }
    public override ReadState ReadState { get => Reader.ReadState; }
    public override bool HasAttributes { get => Reader.HasAttributes; }
    public override XmlNameTable NameTable { get => Reader.NameTable; }
    public override bool CanResolveEntity { get => Reader.CanResolveEntity; }
    public override IXmlSchemaInfo? SchemaInfo { get => Reader.SchemaInfo; }
    public override char QuoteChar { get => Reader.QuoteChar; }
    public override string? GetAttribute(string name)
        => Reader.GetAttribute(name);

    public override string? GetAttribute(string name, string? namespaceURI)
        => Reader.GetAttribute(name, namespaceURI);

    public override string GetAttribute(int i)
        => Reader.GetAttribute(i);

    public override bool MoveToAttribute(string name)
        => Reader.MoveToAttribute(name);

    public override bool MoveToAttribute(string name, string? ns)
        => Reader.MoveToAttribute(name, ns);

    public override void MoveToAttribute(int i)
        => Reader.MoveToAttribute(i);

    public override bool MoveToFirstAttribute()
        => Reader.MoveToFirstAttribute();

    public override bool MoveToNextAttribute()
        => Reader.MoveToNextAttribute();

    public override bool MoveToElement()
        => Reader.MoveToElement();

    public override bool Read()
        => Reader.Read();

    public override void Close()
        => Reader.Close();

    public override void Skip()
        => Reader.Skip();

    public override string? LookupNamespace(string prefix)
        => Reader.LookupNamespace(prefix);

    public override void ResolveEntity()
        => Reader.ResolveEntity();

    public override bool ReadAttributeValue()
        => Reader.ReadAttributeValue();

    public virtual bool HasLineInfo()
        => (Reader as IXmlLineInfo)?.HasLineInfo() ?? false;

    public virtual int LineNumber 
    { 
        get => (Reader as IXmlLineInfo)?.LineNumber ?? 0; 
    }

    public virtual int LinePosition
    {
        get => (Reader as IXmlLineInfo)?.LinePosition ?? 0;
    }
}

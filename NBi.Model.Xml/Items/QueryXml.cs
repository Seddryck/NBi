using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Core.Query.Client;
using NBi.Model.Xml.SerializationOption;
using NBi.Model.Xml.Settings;

namespace NBi.Model.Xml.Items;

public class QueryXml : QueryableXml
{
    [XmlIgnore()]
    private SettingsXml? settings;

    [XmlIgnore()]
    public override SettingsXml? Settings
    {
        get => settings;
        set
        {
            settings = value;
            if (Assembly != null)
                Assembly.Settings = value;
            if (Report != null)
                Report.Settings = value;
            if (SharedDataset != null)
                SharedDataset.Settings = value;
        }
    }


    [XmlAttribute("file")]
    public string File { get; set; } = string.Empty;

    [XmlAttribute("connectionString-ref")]
    public string ConnectionStringReference { get; set; } = string.Empty;

    public bool ShouldSerializeConnectionStringReference() => !string.IsNullOrEmpty(ConnectionStringReference);

    [XmlIgnore]
    private string inlineQuery = string.Empty;

    [XmlIgnore]
    public CData InlineQueryWrite
    {
        get { return inlineQuery; }
        set { inlineQuery = value; }
    }

    [XmlText]
    public virtual string InlineQuery
    {
        get { return inlineQuery; }
        set { inlineQuery = value; }
    }

    [XmlElement("assembly")]
    public AssemblyXml? Assembly { get; set; }

    [XmlElement("report")]
    public virtual ReportXml? Report { get; set; }

    [XmlElement("shared-dataset")]
    public virtual SharedDatasetXml? SharedDataset { get; set; }

}

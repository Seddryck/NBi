using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using NBi.Xml.Items;

namespace NBi.Xml.Settings;

public class DefaultXml
{
    [XmlAttribute("apply-to")]
    public SettingsXml.DefaultScope ApplyTo { get; set; }

    [XmlElement ("connection-string")]
    public ConnectionStringXml ConnectionString { get; set; }

    [Obsolete("Replaced by connection-string")]
    [XmlIgnore]
    public ConnectionStringXml ConnectionStringOld
    {
        get => ConnectionString;
        set { ConnectionString = value; }
    }

    [XmlIgnore]
    public bool ConnectionStringSpecified
    {
        get { return !string.IsNullOrEmpty(ConnectionString.Inline) || ConnectionString.Environment!=null; }
        set { return; }
    }

    [XmlElement("parameter")]
    public List<QueryParameterXml> Parameters { get; set; }

    [XmlElement("variable")]
    public List<QueryTemplateVariableXml> Variables { get; set; }

    [XmlElement("report")]
    public ReportBaseXml Report { get; set; }

    [XmlIgnore]
    public bool ReportSpecified
    {
        get { return !string.IsNullOrEmpty(Report.Path) || !string.IsNullOrEmpty(Report.Source); }
        set { return; }
    }

    [XmlElement("etl")]
    public EtlBaseXml Etl { get; set; }

    [XmlIgnore]
    public bool EtlSpecified
    {
        get { return !Etl.IsEmpty(); }
        set { return; }
    }

    public DefaultXml(SettingsXml.DefaultScope applyTo) : this()
    {
        ApplyTo = applyTo;
    }

    public DefaultXml()
    {
        ConnectionString = new ConnectionStringXml();
        Parameters = new List<QueryParameterXml>();
        Variables = new List<QueryTemplateVariableXml>();
        Report = new ReportBaseXml();
        Etl = new EtlBaseXml();
    }



}

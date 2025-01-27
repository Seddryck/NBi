using NBi.Xml.Decoration.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using NBi.Core;

namespace NBi.Xml.Decoration.Command;

public class CommandGroupXml : DecorationCommandXml
{
    [
    XmlElement(Type = typeof(SqlRunXml), ElementName = "sql-run"),
    XmlElement(Type = typeof(TableLoadXml), ElementName = "table-load"),
    XmlElement(Type = typeof(TableResetXml), ElementName = "table-reset"),
    XmlElement(Type = typeof(ServiceStartXml), ElementName = "service-start"),
    XmlElement(Type = typeof(ServiceStopXml), ElementName = "service-stop"),
    XmlElement(Type = typeof(ExeRunXml), ElementName = "exe-run"),
    XmlElement(Type = typeof(ExeKillXml), ElementName = "exe-kill"),
    XmlElement(Type = typeof(WaitXml), ElementName = "wait"),
    XmlElement(Type = typeof(ConnectionWaitXml), ElementName = "connection-wait"),
    XmlElement(Type = typeof(FileDeleteXml), ElementName = "file-delete"),
    XmlElement(Type = typeof(FileCopyXml), ElementName = "file-copy"),
    XmlElement(Type = typeof(EtlRunXml), ElementName = "etl-run")
    ]
    public List<DecorationCommandXml> InternalCommands { get; set; }

    [XmlIgnore]
    public List<DecorationCommandXml> Commands
    {
        get
        {
            return InternalCommands.Cast<DecorationCommandXml>().ToList();
        }
        set
        {
            InternalCommands = value.Cast<DecorationCommandXml>().ToList();
        }
    }

    [XmlIgnore()]
    public override Settings.SettingsXml Settings
    {   get { return base.Settings; }
        set
        {
            base.Settings = value;
            foreach (var cmd in InternalCommands)
                cmd.Settings = value;
        }
    }

    [DefaultValue(true)]
    [XmlAttribute("parallel")]
    public bool Parallel { get; set; }

    [DefaultValue(false)]
    [XmlAttribute("run-once")]
    public bool RunOnce { get; set; }

    [XmlIgnore]
    public bool HasRun { get; set; }

    public CommandGroupXml()
    {
        Parallel = true;
        RunOnce = false;
        HasRun = false;
        InternalCommands = new List<DecorationCommandXml>();
    }
}

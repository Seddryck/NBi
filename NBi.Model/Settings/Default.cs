using System;
using System.Collections.Generic;
using NBi.Model.Items;

namespace NBi.Model.Settings;

public class Default
{
    public SuiteSettings.DefaultScope ApplyTo { get; set; }

    public ConnectionString ConnectionString { get; set; }

    [Obsolete("Replaced by connection-string")]
    public ConnectionString ConnectionStringOld
    {
        get => ConnectionString;
        set { ConnectionString = value; }
    }

    public bool ConnectionStringSpecified
    {
        get { return !string.IsNullOrEmpty(ConnectionString.Inline) || ConnectionString.Environment!=null; }
        set { return; }
    }

    public List<QueryParameter> Parameters { get; set; }

    public List<QueryTemplateVariable> Variables { get; set; }

    public ReportBase Report { get; set; }

    public bool ReportSpecified
    {
        get { return !string.IsNullOrEmpty(Report.Path) || !string.IsNullOrEmpty(Report.Source); }
        set { return; }
    }

    public EtlBase Etl { get; set; }

    public bool EtlSpecified
    {
        get { return !Etl.IsEmpty(); }
        set { return; }
    }

    public Default(SuiteSettings.DefaultScope applyTo) : this()
    {
        ApplyTo = applyTo;
    }

    public Default()
    {
        ConnectionString = new ConnectionString();
        Parameters = new List<QueryParameter>();
        Variables = new List<QueryTemplateVariable>();
        Report = new ReportBase();
        Etl = new EtlBase();
    }

}

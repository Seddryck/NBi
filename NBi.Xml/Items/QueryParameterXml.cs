using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Query;

namespace NBi.Xml.Items;

public class QueryParameterXml: ParameterXml
{
    private string sqlType;
    [XmlAttribute("sql-type")]
    public string SqlType
    {
        get
        {
            return sqlType;
        }
        set
        {
            sqlType = value;
        }
    }


}

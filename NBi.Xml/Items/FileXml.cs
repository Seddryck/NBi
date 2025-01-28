using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items;

public class FileXml
{
    [Obsolete("Use 'Path' instead of 'Value'")]
    public string? Value { get => Path; set => Path = value; }

    [XmlElement("path")]
    public virtual string? Path { get; set; }

    [XmlElement("parser")]
    public virtual ParserXml? Parser { get; set; }

    [XmlElement("if-missing")]
    public virtual IfMissingXml? IfMissing { get; set; }

    public virtual bool IsBasic() => Parser == null && IfMissing==null;
    public virtual bool IsEmpty() => string.IsNullOrEmpty(Path);
}

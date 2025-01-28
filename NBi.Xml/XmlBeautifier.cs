using System;
using System.Linq;
using System.Text;
using System.Xml;

namespace NBi.Xml;

internal class XmlBeautifier
{
    static public string Beautify(XmlDocument doc)
    {
        var sb = new StringBuilder();
        var settings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "  ",
            NewLineChars = "\r\n",
            NewLineHandling = NewLineHandling.Replace
        };
        using (var writer = XmlWriter.Create(sb, settings))
        {
            doc.Save(writer);
        }
        return sb.ToString();
    }
}

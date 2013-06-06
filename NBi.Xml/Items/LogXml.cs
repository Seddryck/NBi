using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Xml.Settings;

namespace NBi.Xml.Items
{
    public class LogXml
    {
        public DefaultXml Default { get; set; }
        
        [XmlAttribute("file")]
        public string File { get; set; }

        [XmlAttribute("content")]
        public LogContent Content { get; set; }

        [XmlAttribute("condition")]
        public LogCondition Condition { get; set; }

        public enum LogContent
        {
            [XmlEnum(Name = "resultSet")]
            ResultSet = 0,
            [XmlEnum(Name = "query")]
            Query = 1,
            [XmlEnum(Name = "statistics")]
            Statistics = 2,
        }

        public enum LogCondition
        {
            [XmlEnum(Name = "always")]
            Always = 0,
            [XmlEnum(Name = "only-if-failed")]
            Failure = 1,
        }

        public string GetExtension()
        {
            switch (Content)
            {
                case LogContent.ResultSet:
                    return ".csv";
                case LogContent.Query:
                    return ".query";
                case LogContent.Statistics:
                    return ".dat";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public string GetFilename()
        {
            return File + GetExtension();
        }
    }
}

using System;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using NBi.Core;

namespace NBi.Xml.Decoration.Command
{
    public class TableLoadXml : DataManipulationAbstractXml
    {
        [XmlAttribute("name")]
        public string TableName { get; set; }
        
        [XmlAttribute("file")]
        public string InternalFileName { get; set; }

        [XmlIgnore]
        public string FileName
        {
            get
            {
                var file = string.Empty;
                if (Path.IsPathRooted(InternalFileName))
                    file = InternalFileName;
                else
                    file = Settings.BasePath + InternalFileName;
                if (!System.IO.File.Exists(file))
                    throw new ExternalDependencyNotFoundException(file);

                return file;
            }
        }
    }
}

using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.FileManipulation;
using NBi.Xml.Settings;
using System.IO;

namespace NBi.Xml.Decoration.Command
{
    public class FileManipulationAbstractXml : DecorationCommandXml, IFileManipulationCommand
    {
        [XmlAttribute("name")]
        public string FileName { get; set; }

        [XmlAttribute("path")]
        public string InternalPath { get; set; }

        [XmlIgnore]
        public string FullPath
        {
            get
            {
                var fullPath = string.Empty;
                if (Path.IsPathRooted(InternalPath) || String.IsNullOrEmpty(Settings.BasePath))
                    fullPath = InternalPath + FileName;
                else
                    fullPath = Settings.BasePath + InternalPath + FileName;

                return fullPath;
            }
        }
    }
}

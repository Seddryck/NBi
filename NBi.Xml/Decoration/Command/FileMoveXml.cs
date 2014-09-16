using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.FileManipulation;
using System.IO;
using NBi.Core;

namespace NBi.Xml.Decoration.Command
{
    public class FileMoveXml : FileManipulationAbstractXml, IMoveCommand
    {
        [XmlAttribute("original-path")]
        public string InternalOriginalPath { get; set; }

        [XmlIgnore]
        public string OriginalFullPath
        {
            get
            {
                var originalFullPath = string.Empty;
                if (Path.IsPathRooted(InternalOriginalPath) || String.IsNullOrEmpty(Settings.BasePath))
                    originalFullPath = InternalOriginalPath + FileName;
                else
                    originalFullPath = Settings.BasePath + InternalOriginalPath + FileName;

                return originalFullPath;
            }
        }
    }
}

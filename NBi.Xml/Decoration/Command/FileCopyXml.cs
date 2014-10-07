using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.FileManipulation;
using System.IO;
using NBi.Core;

namespace NBi.Xml.Decoration.Command
{
    public class FileCopyXml : FileManipulationAbstractXml, ICopyCommand
    {
        [XmlAttribute("source-path")]
        public string InternalSourcePath { get; set; }

        [XmlIgnore]
        public string SourceFullPath
        {
            get
            {
                var sourceFullPath = string.Empty;
                if (Path.IsPathRooted(InternalSourcePath) || String.IsNullOrEmpty(Settings.BasePath))
                    sourceFullPath = InternalSourcePath + FileName;
                else
                    sourceFullPath = Settings.BasePath + InternalSourcePath + FileName;

                return sourceFullPath;
            }
        }
    }
}

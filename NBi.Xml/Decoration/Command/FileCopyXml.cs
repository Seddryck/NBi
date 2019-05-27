using System;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using NBi.Core;

namespace NBi.Xml.Decoration.Command
{
    public class FileCopyXml : FileManipulationAbstractXml
    {
        [XmlAttribute("source-path")]
        public string InternalSourcePath { get; set; }

        [XmlIgnore]
        public string SourceFullPath
        {
            get
            {
                var sourceFullPath = string.Empty;
                if (System.IO.Path.IsPathRooted(InternalSourcePath) || string.IsNullOrEmpty(base.Settings.BasePath))
                    sourceFullPath = InternalSourcePath + FileName;
                else
                    sourceFullPath = base.Settings.BasePath + InternalSourcePath + FileName;

                return sourceFullPath;
            }
        }
    }
}

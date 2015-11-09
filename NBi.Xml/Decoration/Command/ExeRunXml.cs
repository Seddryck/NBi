using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using NBi.Core.Process;

namespace NBi.Xml.Decoration.Command
{
    public class ExeRunXml : DecorationCommandXml, IRunCommand
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("path")]
        public string InternalPath { get; set; }

        [XmlAttribute("arguments")]
        public string Arguments { get; set; }

        [XmlIgnore]
        public string Argument { get { return Arguments; } }

        [XmlIgnore]
        public string FullPath
        {
            get
            {
                var fullPath = string.Empty;
                if (Path.IsPathRooted(InternalPath) || String.IsNullOrEmpty(Settings.BasePath))
                    fullPath = InternalPath + Name;
                else
                    fullPath = Settings.BasePath + InternalPath + Name;

                return fullPath;

            }
        }

        [XmlAttribute("timeout-milliseconds")]
        [DefaultValue(0)]
        public int TimeOut { get; set; }

        public ExeRunXml()
        {
            TimeOut = 0;
        }
    }
}

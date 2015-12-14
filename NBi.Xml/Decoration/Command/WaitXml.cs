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
    public class WaitXml : DecorationCommandXml, IWaitCommand
    {
        [XmlAttribute("milliseconds")]
        public int MilliSeconds { get; set; }
    }
}

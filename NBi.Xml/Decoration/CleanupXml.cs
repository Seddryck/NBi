using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Xml.Decoration;

public class CleanupXml : DecorationXml
{
    public CleanupXml() { }

    public CleanupXml(CleanupStandaloneXml standalone)
        => Commands = standalone.Commands;
}

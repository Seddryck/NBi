using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Model.Xml.Decoration;

public class SetupXml : DecorationXml
{
    public SetupXml() { }

    public SetupXml(SetupStandaloneXml standalone)
        => Commands = standalone.Commands;
}

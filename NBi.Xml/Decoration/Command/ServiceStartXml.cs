using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.WindowsService;

namespace NBi.Xml.Decoration.Command
{
    public class ServiceStartXml : ServiceAbstractXml, IWindowsServiceStopCommand
    {
    }
}

using NBi.Core.Decoration.IO;
using NBi.Core.Decoration.IO.Commands;
using System;
using System.Data.SqlClient;
using System.Linq;


namespace NBi.Core.Decoration.IO
{
    public class IOFactory
    {
        public IDecorationCommand Instantiate(IIoCommandArgs args)
        {
            switch (args)
            {
                case IoDeleteCommandArgs deleteArgs: return new DeleteCommand(deleteArgs);
                case IoDeletePatternCommandArgs patternArgs: return new DeletePatternCommand(patternArgs);
                case IoDeleteExtensionCommandArgs extensionArgs: return new DeleteExtensionCommand(extensionArgs);
                case IoCopyCommandArgs copyArgs: return new CopyCommand(copyArgs);
                case IoCopyPatternCommandArgs patternArgs: return new CopyPatternCommand(patternArgs);
                case IoCopyExtensionCommandArgs extensionArgs: return new CopyExtensionCommand(extensionArgs);
                default: throw new ArgumentException();
            }
        }
    }
}

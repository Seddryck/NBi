using NBi.Core.Decoration.IO;
using NBi.Core.Decoration.IO.Commands;
using System;
using Microsoft.Data.SqlClient;
using System.Linq;


namespace NBi.Core.Decoration.IO
{
    public class IOFactory
    {
        public IDecorationCommand Instantiate(IIoCommandArgs args)
        {
            return args switch
            {
                IoDeleteCommandArgs deleteArgs => new DeleteCommand(deleteArgs),
                IoDeletePatternCommandArgs patternArgs => new DeletePatternCommand(patternArgs),
                IoDeleteExtensionCommandArgs extensionArgs => new DeleteExtensionCommand(extensionArgs),
                IoCopyCommandArgs copyArgs => new CopyCommand(copyArgs),
                IoCopyPatternCommandArgs patternArgs => new CopyPatternCommand(patternArgs),
                IoCopyExtensionCommandArgs extensionArgs => new CopyExtensionCommand(extensionArgs),
                _ => throw new ArgumentException(),
            };
        }
    }
}

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
                case IDeleteCommandArgs deleteArgs: return new DeleteCommand(deleteArgs);
                case ICopyCommandArgs copyArgs: return new CopyCommand(copyArgs);
                default: throw new ArgumentException();
            }
        }
    }
}

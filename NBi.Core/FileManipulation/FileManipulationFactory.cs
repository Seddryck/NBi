using System;
using System.Data.SqlClient;
using System.Linq;
using NBi.Core.FileManipulation;

namespace NBi.Core.DataManipulation
{
    public class FileManipulationFactory
    {
        public IDecorationCommandImplementation Get(IFileManipulationCommand command)
        {

            if (command is IDeleteCommand)
                return new DeleteCommand(command as IDeleteCommand);
            if (command is ICopyCommand)
                return new CopyCommand(command as ICopyCommand);

            throw new ArgumentException();
        }
    }
}

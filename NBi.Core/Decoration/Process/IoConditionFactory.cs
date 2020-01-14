using NBi.Core.Decoration.IO;
using NBi.Core.Decoration.IO.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.Process
{
    class IoConditionFactory
    {
        public IDecorationCondition Instantiate(IIoConditionArgs args)
        {
            switch (args)
            {
                case FolderExistsConditionArgs folderExistsArgs: return new FolderExistsCondition(folderExistsArgs);
                case FileExistsConditionArgs fileExistsArgs: return new FileExistsCondition(fileExistsArgs);
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}

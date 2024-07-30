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
            return args switch
            {
                FolderExistsConditionArgs folderExistsArgs => new FolderExistsCondition(folderExistsArgs),
                FileExistsConditionArgs fileExistsArgs => new FileExistsCondition(fileExistsArgs),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}

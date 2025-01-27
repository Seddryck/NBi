using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Renaming;

public class RenamingFactory
{
    public IRenamingEngine Instantiate(IRenamingArgs args)
    {
        return args switch
        {
            NewNameRenamingArgs x => new NewNameRenamingEngine(x.OriginalIdentification, x.NewIdentification, x.MissingColumnStrategy),
            _ => throw new ArgumentException(),
        };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Renaming
{
    public interface IRenamingEngine
    {
        ResultSet Execute(ResultSet rs);
    }
}

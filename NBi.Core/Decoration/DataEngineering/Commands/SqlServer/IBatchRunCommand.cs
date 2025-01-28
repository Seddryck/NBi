using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Decoration.DataEngineering;

public interface IBatchRunCommand
{
    void Execute(string fullPath, IDbConnection Connection);
}

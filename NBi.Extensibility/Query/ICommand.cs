using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Extensibility.Query;

public interface ICommand
{
    object Implementation { get; }
    object Client { get; }
}

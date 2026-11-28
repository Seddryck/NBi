using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NBi.Model.Decoration.Command;

public class ExeKill : DecorationCommand
{
    public string ProcessName { get; set; }
}

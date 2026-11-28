using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NBi.Model.Decoration.Command;

public class ExeRun : DecorationCommand
{
    public string Name { get; set; }

    public string Path { get; set; }

    public string Arguments { get; set; }

    public string Argument { get { return Arguments; } }

    public string TimeOut { get; set; }

    public ExeRun()
    {
        TimeOut = "0";
    }
}

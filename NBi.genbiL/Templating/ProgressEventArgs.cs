using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Templating;


public class ProgressEventArgs : EventArgs
{
    public int Total { get; set; }
    public int Done { get; set; }

    public ProgressEventArgs(int done, int total)
    {
        Done = done;
        Total = total;
    }
}

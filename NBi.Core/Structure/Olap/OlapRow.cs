using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Olap;

class OlapRow(string caption, string displayFolder) : IDisplayFolder
{
    public string Caption { get; set; } = caption;
    public string DisplayFolder { get; set; } = displayFolder;
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Olap;

class DimensionRow : OlapRow, IDimensionType
{
    public DimensionRow(string caption, string displayFolder, short dimensionType)
        : base(caption, displayFolder)
        => DimensionType = dimensionType;

    public short DimensionType { get; set; }
}

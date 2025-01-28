using NBi.Core.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Olap.PostFilters;

class DisplayFolder : IPostCommandFilter
{
    public string Caption { get; private set; }

    public DisplayFolder(string caption)
    {
        Caption = caption;
    }

    public bool Evaluate(object row)
    {
        if (row is IDisplayFolder)
            return Evaluate((IDisplayFolder)row);

        throw new ArgumentException();
    }

    protected bool Evaluate(IDisplayFolder row)
    {
        return this.Caption.Equals(row.DisplayFolder);
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Parser.Valuable;

class Column : IValuable
{
    public string Name { get; private set; }

    public Column(string name)
    {
        Name = name;
    }

    public string Display
    {
        get { return string.Format("column '{0}'",Name); }
    }

    public string GetValue(DataRow data)
    {
        if (!data.Table.Columns.Contains(Name))
            throw new ArgumentOutOfRangeException(String.Format("No column named '{0}' has been found.", Name));

        return (string)data[Name];
    }
}

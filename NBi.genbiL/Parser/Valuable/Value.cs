using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Parser.Valuable;

class Value : IValuable
{
    public string Text { get; private set; }

    public Value(string text)
    {
        Text = text;
    }

    public string Display
    {
        get { return string.Format("value '{0}'", Text); }
    }

    public string GetValue(DataRow data)
    {
        return Text;
    }
}

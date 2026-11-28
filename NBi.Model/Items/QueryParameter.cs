using System;
using System.Linq;

namespace NBi.Model.Items;

public class QueryParameter: Parameter
{
    private string sqlType;
    public string SqlType
    {
        get
        {
            return sqlType;
        }
        set
        {
            sqlType = value;
        }
    }


}

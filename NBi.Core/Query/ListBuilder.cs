using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NBi.Core.Query;

/// <summary>
/// This class build a list of string froma query taking the first cell of each row
/// </summary>
public class ListBuilder
{
    
    protected List<string> Load(DataSet dataSet)
    {
        return Load(dataSet.Tables[0]);
    }

    protected List<string> Load(DataTable table)
    {
        var list = new List<string>();
        foreach (DataRow row in table.Rows)
            list.Add((string)row[0]);

        return list;
    }
}

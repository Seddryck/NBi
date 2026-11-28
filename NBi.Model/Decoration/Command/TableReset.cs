using System;
using System.Linq;

namespace NBi.Model.Decoration.Command;

public class TableReset : DataManipulationAbstract
{
    public string TableName {get; set;}
}

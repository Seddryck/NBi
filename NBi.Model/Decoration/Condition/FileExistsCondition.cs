using System;
using System.Linq;

namespace NBi.Model.Decoration.Condition;

public class FileExistsCondition : DecorationCondition
{
    public string Path { get; set; }

    public string Name { get; set; }

    public bool NotEmpty { get; set; } = false;
}

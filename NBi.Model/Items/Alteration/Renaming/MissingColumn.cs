using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Alteration.Renaming;

public class MissingColumn
{
    public MissingColumnBehavior Behavior { get; set; } = MissingColumnBehavior.Failure;
}

public enum MissingColumnBehavior
{
    Failure = 0,
    Skip = 1,
}

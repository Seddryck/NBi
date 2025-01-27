using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action;

public enum DefaultType
{
    Everywhere = 0,
    SystemUnderTest = 1,
    Assert = 2,
    SetupCleanup = 3,
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Extensibility.Resolving
{
    public interface IResultSetResolver
    {
        IResultSet Execute();
    }
}

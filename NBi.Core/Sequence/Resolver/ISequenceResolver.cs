﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Resolver
{
    public interface ISequenceResolver<T>
    {
        IList<T> Execute();
    }
}

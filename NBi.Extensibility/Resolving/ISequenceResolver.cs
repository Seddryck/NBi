using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Extensibility.Resolving;

public interface ISequenceResolver : IResolver
{
    new IList Execute();
}

public interface ISequenceResolver<T> : ISequenceResolver
{
    new List<T> Execute();
}

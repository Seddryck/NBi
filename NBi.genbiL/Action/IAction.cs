using System;
using System.Linq;

namespace NBi.GenbiL.Action
{
    public interface IAction
    {
        void Execute(GenerationState state);
        string Display { get; }
    }
}

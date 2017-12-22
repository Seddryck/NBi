using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Configuration
{
    interface IConfiguration
    {
        IReadOnlyDictionary<string, string> Providers { get; }
        IReadOnlyCollection<Type> Extensions { get; }
    }
}

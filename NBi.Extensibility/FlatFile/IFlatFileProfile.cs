using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Extensibility.FlatFile;

public interface IFlatFileProfile
{
    IDictionary<string, object> Attributes { get; }
}

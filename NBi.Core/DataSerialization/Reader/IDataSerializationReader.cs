using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataSerialization.Reader;

public interface IDataSerializationReader : IDisposable
{
    TextReader Execute();
}

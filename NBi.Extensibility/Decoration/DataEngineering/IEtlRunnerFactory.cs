using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Extensibility.Decoration.DataEngineering;

public interface IEtlRunnerFactory
{
    IEtlRunner Instantiate(IEtlArgs args);
}

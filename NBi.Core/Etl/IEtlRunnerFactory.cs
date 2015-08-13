using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Etl
{
    public interface IEtlRunnerFactory
    {
        IEtlRunner Get(IEtl etl);
    }
}

using NBi.Core.Decoration.DataEngineering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Etl
{
    class EtlRun : EtlRunCommand, IExecution
    {
        public EtlRun()
        : base(null) { }

        public IExecutionResult Run()
        {
            throw new NotImplementedException();
        }
    }
}

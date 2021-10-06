using NBi.Core.Injection;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Format
{
    public class FormatterFactory
    {
        private readonly ServiceLocator serviceLocator;

        public FormatterFactory(ServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public IFormatter Instantiate(IDictionary<string, IVariable> globalVariables)
            => new InvariantFormatter(serviceLocator, globalVariables);
    }
}

using NBi.Core.Injection;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Format
{
    public class FormatterFactory(ServiceLocator serviceLocator)
    {
        private readonly ServiceLocator serviceLocator = serviceLocator;

        public IFormatter Instantiate(Context context)
            => new InvariantFormatter(serviceLocator, context);
    }
}

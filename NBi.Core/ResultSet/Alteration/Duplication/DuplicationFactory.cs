using NBi.Core.Injection;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Duplication
{
    public class DuplicationFactory
    {
        protected ServiceLocator ServiceLocator { get; }
        protected Context Context { get; }

        public DuplicationFactory(ServiceLocator serviceLocator, Context context)
            => (ServiceLocator, Context) = (serviceLocator, context);

        public IDuplicationEngine Instantiate(IDuplicationArgs args)
        {
            return args switch
            {
                DuplicateArgs x => new DuplicateEngine(ServiceLocator, Context, x.Predication, x.Times, x.Outputs),
                _ => throw new ArgumentException(),
            };
            ;
        }
    }
}

using NBi.Core.Injection;
using NBi.Core.Transformation;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Extension
{
    public class ExtensionFactory
    {
        protected ServiceLocator ServiceLocator { get; }
        protected Context Context { get; }

        public ExtensionFactory(ServiceLocator serviceLocator, Context context)
            => (ServiceLocator, Context) = (serviceLocator, context);

        public IExtensionEngine Instantiate(IExtensionArgs args)
        {
            switch(args)
            {
                case ExtendArgs x:
                    switch (x.Language)
                    {
                        case LanguageType.NCalc: return new NCalcExtendEngine(ServiceLocator, Context, x.NewColumn, x.Code);
                        case LanguageType.Native: return new NativeExtendEngine(ServiceLocator, Context, x.NewColumn, x.Code);
                        default: throw new ArgumentException();
                    }
                default: throw new ArgumentException();
            };
        }
    }
}

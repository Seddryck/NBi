using NBi.Core.Injection;
using NBi.Core.Transformation;
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

        public ExtensionFactory(ServiceLocator serviceLocator)
            => ServiceLocator = serviceLocator;

        public IExtensionEngine Instantiate(IExtensionArgs args)
        {
            switch(args)
            {
                case ExtendArgs x:
                    switch (x.Language)
                    {
                        case LanguageType.NCalc: return new NCalcExtendEngine(ServiceLocator, x.NewColumn, x.Code);
                        case LanguageType.Native: return new NativeExtendEngine(ServiceLocator, x.NewColumn, x.Code);
                        default: throw new ArgumentException();
                    }
                default: throw new ArgumentException();
            };
        }
    }
}

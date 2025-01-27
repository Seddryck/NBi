using NBi.Core.Injection;
using NBi.Core.Transformation;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Extension;

public class ExtensionFactory
{
    protected ServiceLocator ServiceLocator { get; }
    protected Context Context { get; }

    public ExtensionFactory(ServiceLocator serviceLocator, Context context)
        => (ServiceLocator, Context) = (serviceLocator, context);

    public IExtensionEngine Instantiate(IExtensionArgs args)
    {
        return args switch
        {
            ExtendArgs x => x.Language switch
            {
                LanguageType.NCalc => new NCalcExtendEngine(ServiceLocator, Context, x.NewColumn, x.Code),
                LanguageType.Native => new NativeExtendEngine(ServiceLocator, Context, x.NewColumn, x.Code),
                _ => throw new ArgumentException(),
            },
            _ => throw new ArgumentException(),
        };
        ;
    }
}

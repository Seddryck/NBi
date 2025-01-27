using NBi.Core.Injection;
using NBi.Core.ResultSet.Alteration.Extension;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Transformation;
using NBi.Core.Variable;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Duplication.OuputStrategies;

class ScriptOuputStrategy : IOutputStrategy
{
    public IScalarResolver Resolver { get; }
    
    public ScriptOuputStrategy(ServiceLocator serviceLocator, Context context, string script, LanguageType language)
    {
        var factory = serviceLocator.GetScalarResolverFactory();
        var args = language switch
        {
            LanguageType.NCalc => new NCalcScalarResolverArgs(script, context),
            LanguageType.Native => new ScalarResolverArgsFactory(serviceLocator, context).Instantiate(script),
            _ => throw new ArgumentException()
        };
        Resolver = factory.Instantiate(args);
    }

    public object? Execute(bool isOriginal, bool isDuplicable, int times, int index)
        =>  Resolver.Execute();

    public bool IsApplicable(bool isOriginal) => !isOriginal;
}

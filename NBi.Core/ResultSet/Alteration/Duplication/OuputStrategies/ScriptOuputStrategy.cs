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

namespace NBi.Core.ResultSet.Alteration.Duplication.OuputStrategies
{
    class ScriptOuputStrategy : IOutputStrategy
    {
        public IScalarResolver Resolver { get; }
        
        public ScriptOuputStrategy(ServiceLocator serviceLocator, Context context, string script, LanguageType language)
        {
            var factory = serviceLocator.GetScalarResolverFactory();
            IScalarResolverArgs args;
            switch (language)
            {
                case LanguageType.NCalc:
                    args = new NCalcScalarResolverArgs(script, context);
                    break ;
                case LanguageType.Native:
                    args = new ScalarResolverArgsFactory(serviceLocator, context).Instantiate(script);
                    break;
                default: throw new ArgumentException();
            }
            Resolver = factory.Instantiate(args);
        }

        public object Execute(bool isOriginal, bool isDuplicable, int times, int index)
            =>  Resolver.Execute();

        public bool IsApplicable(bool isOriginal) => !isOriginal;
    }
}

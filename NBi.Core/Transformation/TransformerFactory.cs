using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Injection;
using NBi.Core.Transformation.Transformer;
using NBi.Core.Variable;

namespace NBi.Core.Transformation
{
    public class TransformerFactory
    {
        protected ServiceLocator ServiceLocator { get; }
        protected Context Context { get; }

        public TransformerFactory(ServiceLocator serviceLocator, Context context)
            => (ServiceLocator, Context) = (serviceLocator, context);

        public ITransformer Instantiate(ITransformationInfo info)
        {
            if (info.Language == LanguageType.Format && (info.OriginalType == ResultSet.ColumnType.Boolean || info.OriginalType == ResultSet.ColumnType.Text))
                throw new InvalidOperationException("Language 'format' is only supporting transformation from 'numeric' and 'dateTime' data types");

            if (info.Language == LanguageType.NCalc && (info.OriginalType == ResultSet.ColumnType.Boolean || info.OriginalType == ResultSet.ColumnType.DateTime))
                throw new InvalidOperationException("Language 'ncalc' is only supporting transformation from 'numeric' and 'text' data types");
            var valueType = info.OriginalType switch
            {
                ResultSet.ColumnType.Text => typeof(string),
                ResultSet.ColumnType.Numeric => typeof(decimal),
                ResultSet.ColumnType.DateTime => typeof(DateTime),
                ResultSet.ColumnType.Boolean => typeof(bool),
                _ => throw new ArgumentOutOfRangeException(),
            };
            var providerType = info.Language switch
            {
                LanguageType.CSharp => typeof(CSharpTransformer<>),
                LanguageType.NCalc => typeof(NCalcTransformer<>),
                LanguageType.Format => typeof(FormatTransformer<>),
                LanguageType.Native => typeof(NativeTransformer<>),
                _ => throw new ArgumentOutOfRangeException(),
            };
            var provider = providerType.MakeGenericType(valueType);
            var transformer = (ITransformer)(Activator.CreateInstance(provider, [ServiceLocator, Context])
                                ?? throw new NullReferenceException());

            return transformer;
        }
    }
}

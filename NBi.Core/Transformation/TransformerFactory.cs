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

            Type valueType;
            switch (info.OriginalType)
            {
                case ResultSet.ColumnType.Text:
                    valueType = typeof(string);
                    break;
                case ResultSet.ColumnType.Numeric:
                    valueType = typeof(decimal);
                    break;
                case ResultSet.ColumnType.DateTime:
                    valueType = typeof(DateTime);
                    break;
                case ResultSet.ColumnType.Boolean:
                    valueType = typeof(bool);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

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

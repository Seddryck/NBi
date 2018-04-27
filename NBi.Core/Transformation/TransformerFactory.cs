using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Transformation.Transformer;

namespace NBi.Core.Transformation
{
    public class TransformerFactory
    {
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

            Type providerType;
            switch (info.Language)
            {
                case LanguageType.CSharp:
                    providerType = typeof(CSharpTransformer<>);
                    break;
                case LanguageType.NCalc:
                    providerType = typeof(NCalcTransformer<>);
                    break;
                case LanguageType.Format:
                    providerType = typeof(FormatTransformer<>);
                    break;
                case LanguageType.Native:
                    providerType = typeof(NativeTransformer<>);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var provider = providerType.MakeGenericType(valueType);
            var transformer = (ITransformer)Activator.CreateInstance(provider, new object[] { });

            return transformer;
        }
    }
}

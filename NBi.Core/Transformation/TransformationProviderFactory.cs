using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation
{
    public class TransformationProviderFactory
    {
        public ITransformationProvider Build(ITransformationInfo info)
        {
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
                    providerType = typeof(CSharp.CSharpTransformationProvider<>);
                    break;
                case LanguageType.NCalc:
                    throw new ArgumentOutOfRangeException();
                case LanguageType.Format:
                    throw new ArgumentOutOfRangeException();
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var provider = providerType.MakeGenericType(valueType);
            var transformer = (ITransformationProvider)Activator.CreateInstance(provider, new[] { info.Code });

            return transformer;
        }
    }
}

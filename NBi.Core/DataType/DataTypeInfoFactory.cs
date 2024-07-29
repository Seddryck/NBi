using NBi.Core.DataType.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataType
{
    public class DataTypeInfoFactory
    {
        public DataTypeInfo Instantiate(RelationalRow row)
        {
            DataTypeInfo dataTypeInfo;
            if (row.CharacterMaximumLength > 0)
            {
                dataTypeInfo = new TextInfo();
                ((TextInfo)dataTypeInfo).Length = row.CharacterMaximumLength;
                ((TextInfo)dataTypeInfo).CharSet = row.CharacterSetName;
                ((TextInfo)dataTypeInfo).Collation = row.CollationName;
                ((TextInfo)dataTypeInfo).Domain = row.DomainName;
            }
            else if (row.NumericScale > 0)
            {
                dataTypeInfo = new NumericInfo();
                ((NumericInfo)dataTypeInfo).Scale = row.NumericScale;
                ((NumericInfo)dataTypeInfo).Precision = row.NumericPrecision;
            }
            else if (row.DateTimePrecision > 0)
            {
                dataTypeInfo = new DateTimeInfo();
                ((DateTimeInfo)dataTypeInfo).Precision = row.DateTimePrecision;
            }
            else
            {
                dataTypeInfo = new DataTypeInfo();
            }

            dataTypeInfo.Name = row.DataType.ToLower();
            dataTypeInfo.Nullable = row.IsNullable.ToUpper() == "YES".ToUpper();
            return dataTypeInfo;
        }

        public DataTypeInfo Instantiate(string value)
        {
            DataTypeInfo dataTypeInfo;

            var type = value.Split('(')[0];
            dataTypeInfo = Decrypt(type);

            int? first = null;
            int? second = null;

            if (value.Split('(').Length>1)
            {
                first = Convert.ToInt32(value.Split('(')[1].Split(',')[0].Replace(")",""));

                if (value.Split('(')[1].Split(',').Length>1)
                    second = Convert.ToInt32(value.Split('(')[1].Split(',')[1].Replace(")",""));
            }

            if (second.HasValue && dataTypeInfo is IScale)
                ((IScale)dataTypeInfo).Scale = second.Value;

            if (first.HasValue && dataTypeInfo is IPrecision)
                ((IPrecision)dataTypeInfo).Precision = first.Value;

            if (first.HasValue && dataTypeInfo is ILength)
                ((ILength)dataTypeInfo).Length = first.Value;            
            
            return dataTypeInfo;
        }

        protected DataTypeInfo Decrypt(string type)
        {
            DataTypeInfo value = null;
            switch (type)
            {
                case "bit":
                    value = new DataTypeInfo();
                    break;
                case "ntext":
                case "nvarchar":
                case "varchar":
                case "nchar":
                case "text":
                case "char":
                    value = new TextInfo();
                    break;
                case "smalldatetime":
                case "datetime":
                    value = new DateTimeInfo();
                    break;
                case "bigint":
                case "money":
                case "smallmoney":
                case "decimal":
                case "float":
                case "int":
                case "real":
                case "smallint":
                case "tinyint":
                    value = new NumericInfo();
                    break;
                default:
                    value = new DataTypeInfo();
                    break;
            }

            value.Name = type;
            return value;
        }
    }
}

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
            DataTypeInfo dataTypeInfo = null;
            

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
    }
}

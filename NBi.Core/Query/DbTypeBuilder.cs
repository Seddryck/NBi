using System;
using System.Data;
using System.Linq;

namespace NBi.Core.Query
{
    public class DbTypeBuilder
    {
        public DbTypeBuilderResult Build(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return null;

            name = name.ToLowerInvariant().Trim().Replace(" ","");
            var result = new DbTypeBuilderResult();

            var typeName = name;
            if (name.Contains("("))
                typeName = name.Substring(0, name.IndexOf("("));


            DbType value;
            if (!Enum.TryParse<DbType>(typeName, true, out value))
            {
                if (!TryParseEquivalent(typeName, out value))
                {
                    throw new ArgumentException(String.Format("Unknown type for database: {0}", typeName), nameof(name));
                }
            }
            result.Value = value;

            if (CanDefinePrecision(value))
            {
                byte precison;
                if (TryDefinePrecision(name, out precison))
                    result.Precision = precison;
            }

            if (CanDefineSize(value))
            {
                int size;
                if (TryDefineSize(name, out size))
                    result.Size = size;
            }

            return result;
        }

        private bool TryDefineSize(string name, out int size)
        {
            size = 0;
            var start = name.IndexOf("(");
            var end = name.IndexOf(",");
            if (end == -1)
                end = name.IndexOf(")");

            if (start == -1 || end == -1)
                return false;

            var valueString = name.Substring(start + 1, end - start -1);

            return int.TryParse(valueString, out size);
        }

        private bool CanDefineSize(DbType value)
        {
            if (value == DbType.Decimal)
                return true;

            return Enum.GetName(typeof(DbType), value).Contains("String");
        }

        private bool TryDefinePrecision(string name, out byte precison)
        {
            precison = 0;
            var start = name.IndexOf(",");
            var end = name.IndexOf(")");

            if (start == -1 || end == -1)
                return false;

            var valueString = name.Substring(start + 1, end - start - 1);

            return byte.TryParse(valueString, out precison);

        }

        private bool CanDefinePrecision(DbType value)
        {
            return (value == DbType.Decimal);
        }

        private bool TryParseEquivalent(string name, out DbType value)
        {
            value = DbType.Object;
            SqlDbType sqlValue = SqlDbType.Variant;
            if (Enum.TryParse<SqlDbType>(name, true, out sqlValue))
            {
                switch (sqlValue)
                {
                    //Boolean
                    case SqlDbType.Bit: value = DbType.Boolean;
                        break;
                    //Integers
                    case SqlDbType.TinyInt: value = DbType.Byte;
                        break;
                    case SqlDbType.SmallInt: value = DbType.Int16;
                        break;
                    case SqlDbType.Int: value = DbType.Int32;
                        break;
                    case SqlDbType.BigInt: value = DbType.Int64;
                        break;
                    //Floating points
                    case SqlDbType.Real: value = DbType.Single;
                        break;
                    case SqlDbType.Float: value = DbType.Double;
                        break;
                    //Money
                    case SqlDbType.Money: value = DbType.Currency;
                        break;
                    //String
                    case SqlDbType.Char: value = DbType.AnsiStringFixedLength;
                        break;
                    case SqlDbType.VarChar: value = DbType.AnsiString;
                        break;
                    case SqlDbType.NChar: value = DbType.StringFixedLength;
                        break;
                    case SqlDbType.NVarChar: value = DbType.String;
                        break;
                    //UID
                    case SqlDbType.UniqueIdentifier: value = DbType.Guid;
                        break;
                    //Others
                    default: return false;
                }
                return true;
            }
            return false;
        }


        public class DbTypeBuilderResult
        {
            public DbType Value { get; internal set; }
            public byte Precision { get; internal set; }
            public int Size { get; internal set; }
        }
    }
}

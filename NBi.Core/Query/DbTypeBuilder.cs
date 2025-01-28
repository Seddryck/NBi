using System;
using System.Data;
using System.Linq;

namespace NBi.Core.Query;

public class DbTypeBuilder
{
    public DbTypeBuilderResult? Build(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        name = name.ToLowerInvariant().Trim().Replace(" ","");
        var result = new DbTypeBuilderResult();

        var typeName = name;
        if (name.Contains('('))
            typeName = name[..name.IndexOf("(")];


        DbType value;
        if (!Enum.TryParse<DbType>(typeName, true, out value))
        {
            if (!TryParseEquivalent(typeName, out value))
                throw new ArgumentException($"Unknown type for database: {typeName}", nameof(name));
        }
        result.Value = value;

        if (DbTypeBuilder.CanDefinePrecision(value))
        {
            if (DbTypeBuilder.TryDefinePrecision(name, out var precison))
                result.Precision = precison;
        }

        if (DbTypeBuilder.CanDefineSize(value))
        {
            if (DbTypeBuilder.TryDefineSize(name, out var size))
                result.Size = size;
        }

        return result;
    }

    private static bool TryDefineSize(string name, out int size)
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

    private static bool CanDefineSize(DbType value)
    {
        if (value == DbType.Decimal)
            return true;

        return (Enum.GetName(typeof(DbType), value) ?? string.Empty).Contains("String");
    }

    private static bool TryDefinePrecision(string name, out byte precison)
    {
        precison = 0;
        var start = name.IndexOf(",");
        var end = name.IndexOf(")");

        if (start == -1 || end == -1)
            return false;

        var valueString = name.Substring(start + 1, end - start - 1);

        return byte.TryParse(valueString, out precison);

    }

    private static bool CanDefinePrecision(DbType value)
        => value == DbType.Decimal;

    private bool TryParseEquivalent(string name, out DbType value)
    {
        value = DbType.Object;
        if (Enum.TryParse<SqlDbType>(name, true, out var sqlValue))
        {
            switch (sqlValue)
            {
                //Boolean
                case SqlDbType.Bit:
                    value = DbType.Boolean;
                    break;
                //Integers
                case SqlDbType.TinyInt:
                    value = DbType.Byte;
                    break;
                case SqlDbType.SmallInt:
                    value = DbType.Int16;
                    break;
                case SqlDbType.Int:
                    value = DbType.Int32;
                    break;
                case SqlDbType.BigInt:
                    value = DbType.Int64;
                    break;
                //Floating points
                case SqlDbType.Real:
                    value = DbType.Single;
                    break;
                case SqlDbType.Float:
                    value = DbType.Double;
                    break;
                //Money
                case SqlDbType.Money:
                    value = DbType.Currency;
                    break;
                //String
                case SqlDbType.Char:
                    value = DbType.AnsiStringFixedLength;
                    break;
                case SqlDbType.VarChar:
                    value = DbType.AnsiString;
                    break;
                case SqlDbType.NChar:
                    value = DbType.StringFixedLength;
                    break;
                case SqlDbType.NVarChar:
                    value = DbType.String;
                    break;
                //UID
                case SqlDbType.UniqueIdentifier:
                    value = DbType.Guid;
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

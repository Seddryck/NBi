using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Caster
{
    public class CasterFactory<T>
    {
        public ICaster<T> Instantiate()
        {
            switch (typeof(T).Name)
            {
                case "String": return (ICaster<T>)new TextCaster();
                case "Decimal": return (ICaster<T>)new NumericCaster();
                case "Boolean": return (ICaster<T>)new BooleanCaster();
                case "DateTime": return (ICaster<T>)new DateTimeCaster();
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}

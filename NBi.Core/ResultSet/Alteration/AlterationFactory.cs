using NBi.Core.ResultSet.Alteration.ColumnBased;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration
{
    public class AlterationFactory
    {
        public IAlteration Instantiate(AlterationType type, IEnumerable<IColumnIdentifier> columns)
        {
            switch (type)
            {
                case AlterationType.RemoveColumns:
                    return new RemoveIdentification(columns);
                case AlterationType.HoldColumns:
                    return new HoldIdentification(columns);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

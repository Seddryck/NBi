using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Presentation;

public class PresenterFactory
{
    public IPresenter Instantiate(ColumnType columnType)
    {
        return columnType switch
        {
            ColumnType.Text => new TextPresenter(),
            ColumnType.Numeric => new NumericPresenter(),
            ColumnType.DateTime => new DateTimePresenter(),
            ColumnType.Boolean => new BooleanPresenter(),
            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}

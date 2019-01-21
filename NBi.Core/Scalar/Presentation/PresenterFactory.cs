using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Presentation
{
    public class PresenterFactory
    {
        public IPresenter Instantiate(ColumnType columnType)
        {
            switch (columnType)
            {
                case ColumnType.Text:
                    return new TextPresenter();
                case ColumnType.Numeric:
                    return new NumericPresenter();
                case ColumnType.DateTime:
                    return new DateTimePresenter();
                case ColumnType.Boolean:
                    return new BooleanPresenter();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

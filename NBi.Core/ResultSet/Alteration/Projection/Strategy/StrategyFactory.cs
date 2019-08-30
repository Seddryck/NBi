//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace NBi.Core.ResultSet.Alteration.Projection.Strategy
//{
//    class StrategyFactory
//    {
//        public IStrategy Instantiate(ApplyToRow applyToRow)
//        {
//            switch (applyToRow)
//            {
//                case ApplyToRow.All:
//                    return new AllRowsStrategy();
//                case ApplyToRow.Top:
//                    return new TopRowsStrategy();
//                case ApplyToRow.Any:
//                    return new AnyRowsStrategy();
//                default:
//                    throw new NotImplementedException();
//            }
//        }
//    }
//}


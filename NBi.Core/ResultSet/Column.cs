using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.ResultSet
{
    public class Column : IColumn
    {
        public int Index {get; set;} 
        public ColumnRole Role {get; set;} 
        public ColumnType Type {get; set;}

        protected decimal _tolerance;
        public decimal Tolerance
        {
            get
            {
                return _tolerance;
            }

            set
            {
                IsToleranceSpecified = true;
                _tolerance = value;
            }
        }
        public bool IsToleranceSpecified { get; set; }
       
    }
}

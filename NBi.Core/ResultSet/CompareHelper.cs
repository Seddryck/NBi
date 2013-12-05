using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.ResultSet
{
    class CompareHelper : IComparable<CompareHelper>
    {
        public Int64 KeysHashed { get; set; }
        public Int64 ValuesHashed { get; set; }
        public System.Data.DataRow DataRowObj { get; set; }

        // Summary:
        //     Compares the current object with another object of the same type.
        //
        // Parameters:
        //   other:
        //     An object to compare with this object.
        //
        // Returns:
        //     A value that indicates the relative order of the objects being compared.
        //     The return value has the following meanings: Value Meaning Less than zero
        //     This object is less than the other parameter.Zero This object is equal to
        //     other. Greater than zero This object is greater than other.
        public int CompareTo(CompareHelper other)
        {
            if (this.KeysHashed == other.KeysHashed)
            {
                return 0;
                //if (this.ValuesHashed == other.ValuesHashed)
                //{
                //    return 0;
                //}
                //else if (this.ValuesHashed < other.ValuesHashed)
                //{
                //    return -1;
                //}
                //else
                //{
                //    return 1;
                //}
            }
            else if (this.KeysHashed < other.KeysHashed)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }
}

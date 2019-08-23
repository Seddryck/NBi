using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Resolver
{
    class NCalcScalarResolverArgs : IScalarResolverArgs
    {
        public string Code { get; }
        public DataRow Row { get; }

        public NCalcScalarResolverArgs(string code, DataRow row)
         => (Code, Row) = (code, row);

    }
}

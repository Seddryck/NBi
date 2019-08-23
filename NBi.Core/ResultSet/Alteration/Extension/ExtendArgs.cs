using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Extension
{
    class ExtendArgs : IExtensionArgs
    {
        public IColumnIdentifier NewColumn { get; set; }
        public string Code { get; set; }

        public ExtendArgs(IColumnIdentifier newColumn, string code)
            => (NewColumn, Code) = (newColumn, code);
    }
}

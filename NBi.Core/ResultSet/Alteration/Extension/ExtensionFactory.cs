using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Extension
{
    public class ExtensionFactory
    {
        public IExtensionEngine Instantiate(IExtensionArgs args)
        {
            switch(args)
            {
                case ExtendArgs x: return new ExtendEngine(x.NewColumn, x.Code);
                default: throw new ArgumentException();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Renaming
{
    public class NewNameRenamingArgs : IRenamingArgs
    {
        public IColumnIdentifier OriginalIdentification { get; set; }
        public IColumnIdentifier NewIdentification { get; set; }

        public NewNameRenamingArgs(IColumnIdentifier originalIdentification, IColumnIdentifier newIdentification)
            => (OriginalIdentification, NewIdentification) = (originalIdentification, newIdentification);
    }
}

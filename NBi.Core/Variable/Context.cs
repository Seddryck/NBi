using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable
{
    public class Context
    {
        public DataRow CurrentRow { get; private set; }

        public void Switch(DataRow currentRow)
            => CurrentRow = currentRow;
    }
}

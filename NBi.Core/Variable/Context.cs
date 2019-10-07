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
        public IDictionary<string, ITestVariable> Variables { get; }
        public DataRow CurrentRow { get; private set; }

        public Context(IDictionary<string, ITestVariable> variables)
            => Variables = variables;

        public void Switch(DataRow currentRow)
            => CurrentRow = currentRow;


    }
}

using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Fake
{
    class UserDefinedFunctionFake : IFakeInstance
    {
        public string Script { get; private set; }
        protected UserDefinedFunction UserDefinedFunction { get; private set; }


        public UserDefinedFunctionFake(UserDefinedFunction userDefinedFunction)
        {
            this.UserDefinedFunction = userDefinedFunction;
        }
        

        public void Initialize()
        {
            Script = UserDefinedFunction.TextBody;
        }

        public void Fake(string code)
        {
            UserDefinedFunction.TextBody = code;
            UserDefinedFunction.Alter();
        }

        public void Rollback()
        {
            UserDefinedFunction.TextBody = Script;
            UserDefinedFunction.Alter();
        }
    }
}

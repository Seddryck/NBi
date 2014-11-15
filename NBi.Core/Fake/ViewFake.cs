using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Fake
{
    class ViewFake : IFakeInstance
    {
        public string Script { get; private set; }
        protected View View { get; private set; }


        public ViewFake(View view)
        {
            this.View = view;
        }
        

        public void Initialize()
        {
            Script = View.TextBody;
        }

        public void Fake(string code)
        {
            View.TextBody = code;
            View.Alter();
        }

        public void Rollback()
        {
            View.TextBody = Script;
            View.Alter();
        }
    }
}

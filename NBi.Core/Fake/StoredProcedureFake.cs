using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Fake
{
    class StoredProcedureFake : IFakeInstance
    {
        public string Script { get; private set; }
        protected StoredProcedure StoredProcedure { get; private set; }


        public StoredProcedureFake(StoredProcedure storedProcedure)
        {
            this.StoredProcedure = storedProcedure;
        }
        

        public void Initialize()
        {
            Script = StoredProcedure.TextBody;
        }

        public void Fake(string code)
        {
            StoredProcedure.TextBody = code;
            StoredProcedure.Alter();
        }

        public void Rollback()
        {
            StoredProcedure.TextBody = Script;
            StoredProcedure.Alter();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Fake
{
    public interface IFakeInstance
    {
        void Initialize();
        void Fake(string code);
        void Rollback();
    }
}

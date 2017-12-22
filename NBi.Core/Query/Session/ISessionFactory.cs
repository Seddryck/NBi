using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Session
{
    public interface ISessionFactory
    {
        bool CanHandle(string connectionString);
        ISession Instantiate(string connectionString);
    }
}

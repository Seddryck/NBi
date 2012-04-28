using System;
using NUnit.Framework.Constraints;
using NF = NUnit.Framework;

namespace NBi.NUnit
{
    public class OnDataSource
    {
        protected static string _connectionString;

        protected OnDataSource(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static OnDataSource Localized(string connectionString)
        {
            return new OnDataSource(connectionString);
        }
        
        public Is Is
        {
            get
            {
                return new Is();
            }
        }
    }

    public class Is : NF.Is
    {

        public Is()
        {
        }
        
        public static Constraint SyntacticallyCorrect()
        {
            return new SyntacticallyCorrectConstraint();
        }

        public static Constraint FasterThan(int maxTimeMilliSeconds, bool cleanCache)
        {
            return new FasterThanConstraint(maxTimeMilliSeconds, cleanCache);
        }

        
    }
}

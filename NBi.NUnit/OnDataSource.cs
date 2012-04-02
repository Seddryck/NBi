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
                return new Is(_connectionString);
            }
        }
    }

    public class Is : NF.Is
    {
        protected static string _connectionString;

        public Is(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public Constraint SyntacticallyCorrect()
        {
            return new QueryParserConstraint(_connectionString);
        }

        public Constraint FasterThan(int maxTimeMilliSeconds)
        {
            return new QueryPerformanceConstraint(_connectionString, maxTimeMilliSeconds);
        }

        public Constraint SameThan(string expectedConnectionString, string expectedSql)
        {
            return new DataSetConstraint(expectedConnectionString, expectedSql, _connectionString);
        }

        public Constraint SameThan(string expectedConnectionString)
        {
            return new DataSetConstraint(expectedConnectionString, _connectionString);
        }

        public Constraint SameStructureThan(string expectedConnectionString, string expectedSql)
        {
            return new DataSetStructureConstraint(expectedConnectionString, expectedSql, _connectionString);
        }

        public Constraint SameStructureThan(string expectedConnectionString)
        {
            return new DataSetStructureConstraint(expectedConnectionString, _connectionString);
        }


        public Constraint SameContentThan(string expectedConnectionString, string expectedSql)
        {
            return new DataSetConstraint(expectedConnectionString, expectedSql, _connectionString);
        }

        public Constraint SameContentThan(string expectedConnectionString)
        {
            return new DataSetConstraint(expectedConnectionString, _connectionString);
        }
    }
}

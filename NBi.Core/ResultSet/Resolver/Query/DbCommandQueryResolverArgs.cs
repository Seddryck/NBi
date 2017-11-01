using System.Data;

namespace NBi.Core.ResultSet.Resolver.Query
{
    public class DbCommandQueryResolverArgs : QueryResolverArgs
    {
        private readonly IDbCommand command;

        public IDbCommand Command { get => command; }

        public DbCommandQueryResolverArgs(IDbCommand command)
            : base(string.Empty, null, null, 0)
        {
            this.command = command;
        }
    }
}
using NBi.Core.Neo4j.Query.Session;
using NBi.Core.Query;
using NBi.Core.Query.Command;
using NBiSession = NBi.Core.Query.Session;
using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Neo4j.Query.Command
{
    class BoltCommandFactory : ICommandFactory
    {
        public bool CanHandle(NBiSession.ISession session) => session is BoltSession;

        public ICommand Instantiate(NBiSession.ISession session, IQuery query)
        {
            if (!CanHandle(session))
                throw new ArgumentException();
            var statement = Instantiate(query);
            return new BoltCommand(session.CreateNew() as ISession, statement);
        }

        protected Statement Instantiate(IQuery query)
        {
            var parameters = new Dictionary<string, object>();
            foreach (var paramater in query.Parameters)
                parameters.Add(RenameParameter(paramater.Name), paramater.GetValue());

            var statementText = query.Statement;

            if (query.TemplateTokens != null && query.TemplateTokens.Count() > 0)
                statementText = ApplyVariablesToTemplate(query.Statement, query.TemplateTokens);

            return new Statement(statementText, parameters);
        }

        private string ApplyVariablesToTemplate(string template, IEnumerable<IQueryTemplateVariable> variables)
        {
            var templateEngine = new StringTemplateEngine(template, variables);
            return templateEngine.Build();
        }

        protected virtual string RenameParameter(string originalName)
        {
            if (originalName.StartsWith("$"))
                return originalName.Substring(1, originalName.Length - 1);
            else
                return originalName;
        }
    }
}

using NBi.Core.Query;
using NBi.Core.Query.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using NBi.Core.CosmosDb.Graph.Query.Session;
using NBi.Core.Query.Session;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Graphs;
using Microsoft.Azure.Documents;

namespace NBi.Core.CosmosDb.Graph.Query.Command
{
    class GremlinCommandFactory : ICommandFactory
    {
        public bool CanHandle(ISession session) => session is GremlinSession;

        public ICommand Instantiate(ISession session, IQuery query)
        {
            if (!CanHandle(session))
                throw new ArgumentException();
            CosmosDbSession cosmosdbSession = session.CreateNew() as CosmosDbSession;
            var cosmosdbQuery = Instantiate(cosmosdbSession, query);
            return new GremlinCommand(cosmosdbSession, cosmosdbQuery);
        }

        protected CosmosDbQuery Instantiate(CosmosDbSession cosmosdbSession, IQuery query)
        {
            var statementText = query.Statement;

            if (query.TemplateTokens != null && query.TemplateTokens.Count() > 0)
                statementText = ApplyVariablesToTemplate(query.Statement, query.TemplateTokens);

            return new CosmosDbQuery(cosmosdbSession, statementText);
        }

        private object GetParameterValue(object originalValue, string type)
        {
            switch (type)
            {
                case "integer": return Convert.ToInt64(originalValue, CultureInfo.InvariantCulture.NumberFormat);
                case "float": return Convert.ToDouble(originalValue, CultureInfo.InvariantCulture.NumberFormat);
                case "boolean": return Convert.ToBoolean(originalValue, CultureInfo.InvariantCulture.NumberFormat);
                default:
                    return originalValue;
            }
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

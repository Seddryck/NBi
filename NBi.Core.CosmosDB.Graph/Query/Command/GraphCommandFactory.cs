using NBi.Core.Query;
using NBi.Core.Query.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using NBi.Core.CosmosDb.Query.Client;
using NBi.Core.Query.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Graphs;
using Microsoft.Azure.Documents;

namespace NBi.Core.CosmosDb.Query.Command
{
    class GraphCommandFactory : ICommandFactory
    {
        public bool CanHandle(IClient client) => client is GraphClient;

        public ICommand Instantiate(IClient client, IQuery query)
        {
            if (!CanHandle(client))
                throw new ArgumentException();
            GremlinClient cosmosdbSession = client.CreateNew() as GremlinClient;
            var cosmosdbQuery = Instantiate(cosmosdbSession, query);
            return new GraphCommand(cosmosdbSession, cosmosdbQuery);
        }

        protected GremlinQuery Instantiate(GremlinClient cosmosdbSession, IQuery query)
        {
            var statementText = query.Statement;

            if (query.TemplateTokens != null && query.TemplateTokens.Count() > 0)
                statementText = ApplyVariablesToTemplate(query.Statement, query.TemplateTokens);

            return new GremlinQuery(cosmosdbSession, statementText);
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

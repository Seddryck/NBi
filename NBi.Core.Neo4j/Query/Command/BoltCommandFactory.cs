using NBi.Core.Neo4j.Query.Client;
using NBi.Core.Query;
using NBi.Core.Query.Command;
using NBiSession = NBi.Core.Query.Client;
using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace NBi.Core.Neo4j.Query.Command
{
    class BoltCommandFactory : ICommandFactory
    {
        public bool CanHandle(NBiSession.IClient client) => client is BoltClient;

        public ICommand Instantiate(NBiSession.IClient client, IQuery query)
        {
            if (!CanHandle(client))
                throw new ArgumentException();
            var statement = Instantiate(query);
            return new BoltCommand(client.CreateNew() as ISession, statement);
        }

        protected Statement Instantiate(IQuery query)
        {
            var parameters = new Dictionary<string, object>();
            foreach (var paramater in query.Parameters)
                parameters.Add(
                    RenameParameter(paramater.Name), 
                    GetParameterValue(
                        paramater.GetValue(), 
                        paramater.SqlType.ToLowerInvariant().Trim()
                    ));

            var statementText = query.Statement;

            if (query.TemplateTokens != null && query.TemplateTokens.Count() > 0)
                statementText = ApplyVariablesToTemplate(query.Statement, query.TemplateTokens);

            return new Statement(statementText, parameters);
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

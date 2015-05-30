using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Relational.Builders
{
    abstract class RelationalDiscoveryCommandBuilder : IDiscoveryCommandBuilder
    {
        protected virtual string BasicCommandText 
        {
            get { return "select [{0}_name] from INFORMATION_SCHEMA.{1} where 1=1"; }
        }

        private string commandText;
        private IEnumerable<IPostCommandFilter> postFilters;
        private bool isBuild = false;


        protected string CaptionName { get; set; }
        protected string TableName { get; set; }

        public void Build(IEnumerable<CaptionFilter> filters)
        {
            commandText = BuildCommandText();

            var allFilters = BuildFilters(filters).ToList();
            var comnandFilters = allFilters.Where(f => f is CommandFilter).Cast<CommandFilter>();
            var valueFilters = comnandFilters.Select(f => f.Value);

            foreach (var valueFilter in valueFilters)
                commandText += " and " + valueFilter;

            postFilters = allFilters.Where(f => f is IPostCommandFilter).Cast<IPostCommandFilter>();
            isBuild = true;
        }

        protected abstract IEnumerable<ICommandFilter> BuildFilters(IEnumerable<CaptionFilter> filters);

        protected string BuildCommandText()
        {
            return string.Format(BasicCommandText, CaptionName, TableName);
        }

        public string GetCommandText()
        {
            if (!isBuild)
                throw new InvalidOperationException();

            return commandText;
        }

        public IEnumerable<IPostCommandFilter> GetPostFilters()
        {
            if (!isBuild)
                throw new InvalidOperationException();

            return postFilters;
        }

    }
}

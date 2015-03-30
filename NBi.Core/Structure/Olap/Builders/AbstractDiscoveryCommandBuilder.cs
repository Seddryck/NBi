using NBi.Core.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Olap.Builders
{
    abstract class AbstractDiscoveryCommandBuilder : IDiscoveryCommandBuilder
    {
        protected abstract string BasicCommandText { get; }
        private string commandText;
        private IEnumerable<IPostCommandFilter> postFilters;
        private bool isBuild=false;


        protected string CaptionName { get; set; }
        protected string DisplayFolderName { get; set; }
        protected string TableName { get; set; }
        protected string VisibleName { get; set; }


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
            string visibleFilter = string.Empty;
            if (!string.IsNullOrEmpty(VisibleName))
                visibleFilter = string.Format(" and {0}_is_visible", VisibleName);

            string displayFolderField = "''";
            if (!string.IsNullOrEmpty(DisplayFolderName))
                displayFolderField = string.Format("{0}_display_folder", DisplayFolderName);

            return string.Format(BasicCommandText, CaptionName, displayFolderField, TableName, visibleFilter);
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

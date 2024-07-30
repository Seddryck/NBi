using NBi.Core.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Olap.Builders
{
    abstract class AbstractDiscoveryCommandBuilder
        (
            string captionName
            , string displayFolderName
            , string tableName
            , string visibleName
        )
        : IDiscoveryCommandBuilder
    {
        protected abstract string BasicCommandText { get; }
        private string? commandText;
        private IEnumerable<IPostCommandFilter> postFilters = [];

        protected string CaptionName { get; set; } = captionName;
        protected string DisplayFolderName { get; set; } = displayFolderName;
        protected string TableName { get; set; } = tableName;
        protected string VisibleName { get; set; } = visibleName;


        public void Build(IEnumerable<IFilter> filters)
        {
            commandText = BuildCommandText();

            var captionFilters = BuildCaptionFilters(filters.Where(f => f is CaptionFilter).Cast<CaptionFilter>());
            var otherFilters = BuildNonCaptionFilters(filters.Where(f => !(f is CaptionFilter)).Cast<CaptionFilter>());

            var allFilters = captionFilters.Union(otherFilters).ToList();
            var comnandFilters = allFilters.Where(f => f is CommandFilter).Cast<CommandFilter>();
            var valueFilters = comnandFilters.Select(f => f.Value);

            foreach (var valueFilter in valueFilters)
                commandText += " and " + valueFilter;

            postFilters = allFilters.Where(f => f is IPostCommandFilter).Cast<IPostCommandFilter>();
        }

        protected abstract IEnumerable<IFilter> BuildCaptionFilters(IEnumerable<CaptionFilter> filters);
        protected virtual IEnumerable<ICommandFilter> BuildNonCaptionFilters(IEnumerable<IFilter> filters)
            => [];

        protected string BuildCommandText()
        {
            string visibleFilter = string.Empty;
            if (!string.IsNullOrEmpty(VisibleName))
                visibleFilter = $" and {VisibleName}_is_visible";

            string displayFolderField = "''";
            if (!string.IsNullOrEmpty(DisplayFolderName))
                displayFolderField = $"{DisplayFolderName}_display_folder";

            return string.Format(BasicCommandText, CaptionName, displayFolderField, TableName, visibleFilter);
        }

        public string GetCommandText()
            => commandText ?? throw new InvalidOperationException();

        public IEnumerable<IPostCommandFilter> GetPostFilters()
        {
            if (commandText is null)
                throw new InvalidOperationException();

            return postFilters;
        }
    }
}

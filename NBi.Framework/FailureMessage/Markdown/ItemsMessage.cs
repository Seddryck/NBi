using MarkdownLog;
using NBi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage.Markdown
{
    public class ItemsMessage : SampledFailureMessage<string>
    {
        public ItemsMessage()
            : base(FailureReportProfile.Default)
        { }

        public ItemsMessage(IFailureReportProfile profile)
            : base(profile)
        { }


        public void Build(IEnumerable<string> expectedItems, IEnumerable<string> actualItems, ListComparer.Result result)
        {
            expectedItems = expectedItems ?? new List<string>();
            actualItems = actualItems ?? new List<string>();
            result = result ?? new ListComparer.Result(null, null);

            expected = BuildList(expectedItems, Profile.ExpectedSet);
            actual = BuildList(actualItems, Profile.ActualSet);
            compared = BuildIfNotEmptyList(result.Missing ?? new List<string>(), "Missing", Profile.AnalysisSet);
            compared.Append(BuildIfNotEmptyList(result.Unexpected ?? new List<string>(), "Unexpected", Profile.AnalysisSet));
        }

        private MarkdownContainer BuildList(IEnumerable<string> items, FailureReportSetType sampling)
        {
            var sampledItems = Sample(items, sampling).ToList();

            var container = new MarkdownContainer();
            if (items.Count() > 0)
            {
                container.Append(string.Format("Set of {0} item{1}", items.Count(), items.Count() > 1 ? "s" : string.Empty).ToMarkdownParagraph());
                container.Append(sampledItems.ToMarkdownBulletedList());
            }
            else
                container.Append("An empty set.".ToMarkdownParagraph());

            if (IsSampled(items, sampling))
                container.Append(string.Format("... and {0} others not displayed.", CountExcludedRows(items)).ToMarkdownParagraph());

            return container;
        }

        private MarkdownContainer BuildList(IEnumerable<string> items, string title, FailureReportSetType sampling)
        {
            var container = new MarkdownContainer();
            container.Append((title + " items:").ToMarkdownSubHeader());
            container.Append(BuildList(items, sampling));

            return container;
        }


        private MarkdownContainer BuildIfNotEmptyList(IEnumerable<string> items, string title, FailureReportSetType sampling)
        {
            if (items.Count() == 0)
                return new MarkdownContainer();
            else
                return BuildList(items, title, sampling);
        }

    }
}

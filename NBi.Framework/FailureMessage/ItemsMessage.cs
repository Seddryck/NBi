using MarkdownLog;
using NBi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage
{
    public class ItemsMessage : SampledFailureMessage<string>
    {
        public ItemsMessage()
            : base(10, 15)
        { }
        public ItemsMessage(int sampleItemCount, int maxItemCount)
            : base(sampleItemCount, maxItemCount)
        {
        }

        public void Build(IEnumerable<string> expectedItems, IEnumerable<string> actualItems, ListComparer.Result result)
        {
            expectedItems = expectedItems ?? new List<string>();
            actualItems = actualItems ?? new List<string>();
            result = result ?? new ListComparer.Result(null, null);

            expected = BuildList(expectedItems);
            actual = BuildList(actualItems);
            compared = BuildIfNotEmptyList(result.Missing ?? new List<string>(), "Missing");
            compared.Append(BuildIfNotEmptyList(result.Unexpected ?? new List<string>(), "Unexpected"));
        }

        private MarkdownContainer BuildList(IEnumerable<string> items)
        {
            var sampledItems = Sample(items).ToList();

            var container = new MarkdownContainer();
            if (items.Count() > 0)
            {
                container.Append(string.Format("Set of {0} item{1}", items.Count(), items.Count() > 1 ? "s" : string.Empty).ToMarkdownParagraph());
                container.Append(sampledItems.ToMarkdownBulletedList());
            }
            else
                container.Append("An empty set.".ToMarkdownParagraph());
                
            if (IsSampled(items))
                container.Append(string.Format("... and {0} others not displayed.", CountExcludedRows(items)).ToMarkdownParagraph());

            return container;
        }

        private MarkdownContainer BuildList(IEnumerable<string> items, string title)
        {
            var container = new MarkdownContainer();
            container.Append((title+ " items:").ToMarkdownSubHeader());
            container.Append(BuildList(items));

            return container;
        }


        private MarkdownContainer BuildIfNotEmptyList(IEnumerable<string> items, string title)
        {
            if (items.Count() == 0)
                return new MarkdownContainer();
            else
                return BuildList(items, title);
        }

    }
}

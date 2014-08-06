using MarkdownLog;
using NBi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage
{
    public class ItemsMessage : IFailureMessage
    {
        protected readonly int maxItemCount;
        protected readonly int sampleItemCount;

        protected MarkdownContainer expected;
        protected MarkdownContainer actual;
        protected MarkdownContainer compared;

        public ItemsMessage() : this(10,15)
        { }
        public ItemsMessage(int sampleItemCount, int maxItemCount)
        {
            this.sampleItemCount = sampleItemCount;
            this.maxItemCount = maxItemCount;
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
            var sampledItems = items.Take(items.Count() > maxItemCount ? sampleItemCount : items.Count()).ToList();

            var container = new MarkdownContainer();
            container.Append(string.Format("Set of {0} item{1}", items.Count(), items.Count() > 1 ? "s" : string.Empty).ToMarkdownParagraph());
            container.Append(sampledItems.ToMarkdownBulletedList());
            if (items.Count() > sampledItems.Count())
                container.Append(string.Format("... and {0} others not displayed.", items.Count() - sampledItems.Count()).ToMarkdownParagraph());

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


        public string RenderExpected()
        {
            return expected.ToMarkdown();
        }

        public string RenderActual()
        {
            return actual.ToMarkdown();
        }

        public string RenderCompared()
        {
            return compared.ToMarkdown();
        }
    }
}

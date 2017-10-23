using NBi.Framework.FailureMessage.Markdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage
{
    public class ItemsMessageFormatterFactory
    {
        public IItemsMessageFormatter Instantiate(IFailureReportProfile profile)
        {
            return new ItemsMessageMarkdown(profile);
        }
    }
}

using NBi.Framework.FailureMessage.Markdown;
using NBi.Framework.Sampling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage.Json;

class ItemsMessageJson : ItemsMessageMarkdown
{
    public ItemsMessageJson(IDictionary<string, ISampler<string>> samplers)
        : base(samplers)
    {
    }
}

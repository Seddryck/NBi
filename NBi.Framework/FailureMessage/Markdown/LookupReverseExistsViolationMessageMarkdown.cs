using MarkdownLog;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NBi.Core.ResultSet.Lookup.Violation;
using NBi.Extensibility;
using NBi.Framework.FailureMessage.Common;
using NBi.Framework.FailureMessage.Common.Helper;
using NBi.Framework.FailureMessage.Markdown.Helper;
using NBi.Framework.Sampling;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage.Markdown;

class LookupReverseExistsViolationMessageMarkdown : LookupExistsViolationMessageMarkdown
{

    public LookupReverseExistsViolationMessageMarkdown(IDictionary<string, ISampler<IResultRow>> samplers)
        : base(samplers) { }

}

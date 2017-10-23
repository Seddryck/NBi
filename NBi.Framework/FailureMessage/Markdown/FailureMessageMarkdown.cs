using MarkdownLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Framework.FailureMessage.Markdown
{
    public abstract class FailureMessageMarkdown
    {   
        protected MarkdownContainer expected;
        protected MarkdownContainer actual;
        protected MarkdownContainer analysis;

        protected FailureMessageMarkdown()
        {
            
        }

        public virtual string RenderExpected()
        {
            return expected.ToMarkdown();
        }

        public virtual string RenderActual()
        {
            return actual.ToMarkdown();
        }

        public virtual string RenderAnalysis()
        {
            return analysis.ToMarkdown();
        }
    }
}

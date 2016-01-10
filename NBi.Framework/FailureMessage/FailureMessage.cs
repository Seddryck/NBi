using MarkdownLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Framework.FailureMessage
{
    public abstract class FailureMessage
    {   
        protected MarkdownContainer expected;
        protected MarkdownContainer actual;
        protected MarkdownContainer filtered;
        protected MarkdownContainer compared;

        protected FailureMessage()
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

        public virtual string RenderFiltered()
        {
            return filtered.ToMarkdown();
        }

        public virtual string RenderCompared()
        {
            return compared.ToMarkdown();
        }
    }
}

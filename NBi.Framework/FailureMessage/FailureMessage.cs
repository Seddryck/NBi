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
        protected MarkdownContainer compared;


        public virtual string RenderExpected()
        {
            return expected.ToMarkdown();
        }

        public virtual string RenderActual()
        {
            return actual.ToMarkdown();
        }

        public virtual string RenderCompared()
        {
            return compared.ToMarkdown();
        }
    }
}

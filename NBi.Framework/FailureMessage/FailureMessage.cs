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

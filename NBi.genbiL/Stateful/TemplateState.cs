using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Stateful
{
    public class TemplateState
    {
        public string Code { get; set; }

        public IEnumerable<string> PredefinedLabels { get; set; }

        public TemplateState()
        {
            PredefinedLabels = new List<string>();
        }
    }
}

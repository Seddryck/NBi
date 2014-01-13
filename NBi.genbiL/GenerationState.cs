using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NBi.GenbiL
{
    class GenerationState
    {
        public DataTable TestCases { get; set; }
        public IEnumerable<string> Variables { get; set; }
        public string Template { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Summarization
{
    public class SummarizationFactory
    {
        public ISummarizationEngine Instantiate(ISummarizationArgs args)
        {
            switch(args)
            {
                case SummerizeArgs x: return new SummerizeEngine(x);
                default: throw new ArgumentException();
            }
        }
    }
}

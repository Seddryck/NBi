using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Summarization
{
    public interface ISummarizationEngine
    {
        ResultSet Execute(ResultSet rs);
    }
}

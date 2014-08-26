﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Case
{
    class FilterDistinctCaseAction: ICaseAction
    {

        public FilterDistinctCaseAction()
        {
        }

        public void Execute(GenerationState state)
        {
            state.TestCases.FilterDistinct();
        }

        public virtual string Display
        {
            get
            {
                return string.Format("Filtering distinct cases.");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Ranking.Scoring;

interface IScorer
{
    ScoredObject Execute(object obj);
}

interface IScorer<T>
{
    ScoredObject Execute(T obj);
}



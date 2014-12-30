using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Template
{
    public class LoadTemplateActionBuilder
    {
        public ITemplateAction Build(LoadType loadType, string name)
        {
            switch (loadType)
            {
                case LoadType.File:
                    return new LoadExternalTemplateAction(name);
                case LoadType.Predefined:
                    return new LoadPredefinedTemplateAction(name);
                case LoadType.Query:
                    throw new ArgumentOutOfRangeException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

using System;
using System.Linq;
using NBi.GenbiL.Action.Case;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;
using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Template
{
    public class LoadExternalTemplateAction : ITemplateAction
    {
        public string Path { get; set; }
        public LoadExternalTemplateAction(string path)
            : base()
        {
            Path = path;
        }

        public void Execute(GenerationState state)
        {
            var tpl = string.Empty;           
            using (var stream = new StreamReader(Path))
            {
                tpl = stream.ReadToEnd();
            }
            state.Template.Code = tpl;
        }

        public string Display
        {
            get
            {
                return string.Format("Loading external template from '{0}'"
                    , Path
                    );
            }
        }
    }
}

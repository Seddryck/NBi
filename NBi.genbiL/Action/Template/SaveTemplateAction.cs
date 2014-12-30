using System;
using System.Linq;
using NBi.GenbiL.Action.Case;
using System.IO;

namespace NBi.GenbiL.Action.Template
{
    public class SaveTemplateAction : ITemplateAction
    {
        public string Path { get; set; }
        public SaveTemplateAction(string path)
            : base()
        {
            Path = path;
        }

        public void Execute(GenerationState state)
        {
            using (TextWriter tw = new StreamWriter(Path))
            {
                tw.Write(state.Template);
            }  
        }

        public string Display
        {
            get
            {
                return string.Format("Saving template as '{0}'"
                    , Path
                    );
            }
        }
    }
}

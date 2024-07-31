using System;
using System.Linq;
using NBi.GenbiL.Action.Case;
using System.IO;
using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Template
{
    public class AddFileTemplateAction : ITemplateAction
    {
        public string Filename { get; set; }
        public AddFileTemplateAction(string filename)
            : base()
        {
            Filename = filename;
        }

        public void Execute(GenerationState state)
        {
            using var stream = new StreamReader(Filename);
            state.Templates.Add(stream.ReadToEnd());
        }
        
        public string Display
        {
            get
            {
                return string.Format($"Adding new Template from externam file '{Filename}'");
            }
        }
    }
}

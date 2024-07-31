using System;
using System.Collections.Generic;
using System.Linq;
using NBi.GenbiL.Action;
using NBi.GenbiL.Parser;
using NBi.Xml;
using Sprache;
using System.IO;
using NBi.GenbiL.Stateful;

namespace NBi.GenbiL
{
    public class TestSuiteGenerator
    {
        public string Text { get; private set; } = string.Empty;

        public TestSuiteGenerator()
        {
            
        }

        public void Load(string filename)
        {
            using var stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            Load(stream);
        }

        protected internal virtual void Load(Stream stream)
        {
            using var reader = new StreamReader(stream, System.Text.Encoding.UTF8, true);
            Text = reader.ReadToEnd();
        }

        public void Save(string filename)
        {
            using var stream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
            Save(stream);
        }

        protected internal virtual void Save(Stream stream)
        {
            using var writer = new StreamWriter(stream, System.Text.Encoding.UTF8);
            writer.Write(Text);
        }

        public void WriteLine(string line)
        {
            Text += line + Environment.NewLine;
        }

        public void Execute()
        {
            var recipe = Interpret(Text);
            Apply(recipe);
        }

        protected virtual IEnumerable<IAction> Interpret(string input)
        {
            return Recipe.Parser.Parse(input);
        }

        protected GenerationState Apply(IEnumerable<IAction> actions)
        {
            var state = new GenerationState();
            foreach (var action in actions)
            {
                OnActionInfoEvent(new ActionInfoEventArgs(action.Display));
                action.Execute(state);
            }
                
            return state;
        }


        protected virtual void Save(string filename, TestSuiteXml testSuite)
        {
            var xmlManager = new XmlManager();
            xmlManager.Persist(filename, testSuite);
        }

        public class ActionInfoEventArgs : EventArgs
        {
            public ActionInfoEventArgs(string message)
            {
                Message = message;
            }
            public string Message { get; set; }
        }

        public event EventHandler<ActionInfoEventArgs>? ActionInfoEvent;

        protected virtual void OnActionInfoEvent(ActionInfoEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of 
            // a race condition if the last subscriber unsubscribes 
            // immediately after the null check and before the event is raised.
            EventHandler<ActionInfoEventArgs> handler = ActionInfoEvent!;

            // Event will be null if there are no subscribers 
            if (handler != null)
            {
                // Format the string to send inside the CustomEventArgs parameter
                e.Message += String.Format(" at {0}", DateTime.Now.ToString());

                // Use the () operator to raise the event.
                handler(this, e);
            }
        }
    }
}

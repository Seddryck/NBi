using System;
using System.Collections.Generic;
using System.Linq;
using NBi.GenbiL.Action;
using NBi.GenbiL.Parser;
using NBi.Xml;
using Sprache;

namespace NBi.GenbiL
{
    public class TestSuiteGenerator
    {
        public string Text { get; private set; }

        public TestSuiteGenerator()
        {
            
        }

        public void Load(string filename)
        {
            Text = System.IO.File.ReadAllText(filename);
        }

        public void Save(string filename)
        {
            System.IO.File.WriteAllText(filename, Text);
        }

        public void WriteLine(string line)
        {
            Text += line + Environment.NewLine;
        }

        public void Execute()
        {
            var recipe = Interpret(Text);
            var testSuite = Apply(recipe);
        }

        protected IEnumerable<IAction> Interpret(string input)
        {
            return Recipe.Parser.Parse(input);
        }

        protected GenerationState Apply(IEnumerable<IAction> actions)
        {
            var state = new GenerationState();
            foreach (var action in actions)
                action.Execute(state);

            return state;
        }

       
        protected void Save(string filename, TestSuiteXml testSuite)
        {
            var xmlManager = new XmlManager();
            xmlManager.Persist(filename, testSuite);
        }
    }
}

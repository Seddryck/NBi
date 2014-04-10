using System;
using System.Linq;
using Sprache;

namespace NBi.GenbiL.Parser
{
    class Keyword
    {
        public static readonly Parser<string> Case = Parse.IgnoreCase("Case").Text().Token();
        public static readonly Parser<string> Template = Parse.IgnoreCase("Template").Text().Token();
        public static readonly Parser<string> Setting = Parse.IgnoreCase("Setting").Text().Token();
        public static readonly Parser<string> Suite = Parse.IgnoreCase("Suite").Text().Token();

        public static readonly Parser<string> Load = Parse.IgnoreCase("Load").Text().Token();
        public static readonly Parser<string> Remove = Parse.IgnoreCase("Remove").Text().Token();
        public static readonly Parser<string> Move = Parse.IgnoreCase("Move").Text().Token();
        public static readonly Parser<string> Rename = Parse.IgnoreCase("Rename").Text().Token();
        public static readonly Parser<string> Generate = Parse.IgnoreCase("Generate").Text().Token();
        public static readonly Parser<string> Save = Parse.IgnoreCase("Save").Text().Token();
        public static readonly Parser<string> Filter = Parse.IgnoreCase("Filter").Text().Token();

        public static readonly Parser<string> Into = Parse.IgnoreCase("Into").Text().Token();
        public static readonly Parser<string> To = Parse.IgnoreCase("To").Text().Token();
        public static readonly Parser<string> On = Parse.IgnoreCase("On").Text().Token();
        public static readonly Parser<string> In = Parse.IgnoreCase("In").Text().Token();
        public static readonly Parser<string> Out = Parse.IgnoreCase("Out").Text().Token();
    }
}

using System;
using System.Linq;
using NBi.GenbiL.Action;
using NBi.GenbiL.Action.Template;
using Sprache;

namespace NBi.GenbiL.Parser
{
    class Template
    {
        public static readonly Parser<LoadType> LoadTypeParser =
            Parse.IgnoreCase("file").Return(LoadType.File)
                .Or(Parse.IgnoreCase("predefined").Return(LoadType.Predefined))
                .Token();

        readonly static  Parser<LoadTemplateAction> TemplateLoadParser =
            (
                from loadType in LoadTypeParser
                from filename in Grammar.QuotedTextual
                select new LoadTemplateAction(loadType, filename)
            );

        readonly static Parser<LoadTemplateAction> TemplateParser = 
        (
                from load in Keyword.Load
                from text in TemplateLoadParser
                select text
        );

        public readonly static Parser<IAction> Parser =
        (
                from load in Keyword.Template
                from text in TemplateParser
                select text
        );
    }
}

using System;
using System.Linq;
using NBi.GenbiL.Action;
using NBi.GenbiL.Action.Template;
using Sprache;

namespace NBi.GenbiL.Parser;

class Template
{
    
    readonly static  Parser<ITemplateAction> templateLoadEmbeddedParser =
        (
            from load in Keyword.Load
            from embedded in Keyword.Embedded.Or(Keyword.Predefined)
            from filename in Grammar.QuotedTextual
            select new LoadEmbeddedTemplateAction(filename)
        );

    readonly static Parser<ITemplateAction> templateLoadFileParser =
        (
            from load in Keyword.Load
            from embedded in Keyword.File
            from filename in Grammar.QuotedTextual
            select new LoadFileTemplateAction(filename)
        );

    readonly static Parser<ITemplateAction> templateAddEmbeddedParser =
        (
            from add in Keyword.Add
            from embedded in Keyword.Embedded.Or(Keyword.Predefined)
            from filename in Grammar.QuotedTextual
            select new AddEmbeddedTemplateAction(filename)
        );

    readonly static Parser<ITemplateAction> templateAddFileParser =
        (
            from add in Keyword.Add
            from embedded in Keyword.File
            from filename in Grammar.QuotedTextual
            select new AddFileTemplateAction(filename)
        );

    readonly static Parser<ITemplateAction> templateClearParser =
        (
            from clear in Keyword.Clear
            select new ClearTemplateAction()
        );
    
    public readonly static Parser<IAction> Parser =
    (
            from @case in Keyword.Template
            from action in templateClearParser
                            .Or(templateAddEmbeddedParser)
                            .Or(templateAddFileParser)
                            .Or(templateLoadEmbeddedParser)
                            .Or(templateLoadFileParser)
            select action
    );
}

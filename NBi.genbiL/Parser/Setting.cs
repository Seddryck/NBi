using System;
using System.Linq;
using NBi.GenbiL.Action;
using NBi.GenbiL.Action.Setting;
using Sprache;

namespace NBi.GenbiL.Parser
{
    class Setting
    {
        static readonly Parser<DefaultType> SettingsTypeParser =
            Parse.IgnoreCase("systemundertest").Return(DefaultType.SystemUnderTest)
            .Or(Parse.IgnoreCase("sut").Return(DefaultType.SystemUnderTest))
            .Or(Parse.IgnoreCase("assert").Return(DefaultType.Assert))
                .Token();

        readonly static Parser<ISettingAction> DefaultParser =
        (
                from settingType in Parse.IgnoreCase("default").Token()
                from target in SettingsTypeParser.Token()
                from variable in Grammar.Textual.Token()
                from value in Grammar.QuotedTextual.Token()
                select new DefaultAction(target, value)
        );


        readonly static Parser<ISettingAction> ReferenceParser =
        (
                from settingType in Parse.IgnoreCase("reference").Token()
                from name in Grammar.QuotedTextual.Token()
                from variable in Grammar.Textual.Token()
                from value in Grammar.QuotedTextual.Token()
                select new ReferenceAction(name, value)
        );

        readonly static Parser<ISettingAction> ParameterParser =
        (
                from setKeyword in Keyword.Set
                from name in Grammar.QuotedTextual.Token()
                from value in Grammar.Boolean
                select new ParameterActionFactory().Build(name.ToString(), value)
        );

        public readonly static Parser<IAction> Parser =
        (
                from setting in Keyword.Setting
                from text in ReferenceParser.Or(DefaultParser).Or(ParameterParser)
                select text
        );
    }
}

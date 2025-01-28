using NBi.GenbiL.Action;
using NBi.GenbiL.Parser;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sprache;

namespace NBi.GenbiL.Testing.Parser;

class CommentParserTest
{
    [Test]
    public void Parser_SingleLineComment_EmptyAction()
    {
        var input = "//This is a comment" + Environment.NewLine;
        var result = Comment.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<EmptyAction>());
    }

    [Test]
    public void Parser_MultiLineComment_EmptyAction()
    {
        var input = "/*This is a comment\r\nOn Multiple lines*/";
        var result = Comment.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<EmptyAction>());
    }

}

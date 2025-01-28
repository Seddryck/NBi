using Moq;
using NBi.Core;
using NBi.Core.Configuration;
using NBi.Core.Configuration.FailureReport;
using NBi.Framework;
using NBi.Framework.FailureMessage;
using NBi.Framework.FailureMessage.Json;
using NBi.Framework.FailureMessage.Markdown;
using NBi.Framework.Sampling;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.Testing.FailureMessage.Markdown;

public class ScoreMessageMardownTest
{
    [Test]
    public void RenderExpected_Threshold_ReturnCorrectSentence()
    {
        var msg = new ScoreMessageMarkdown();
        msg.Initialize(0.62m, 0.75m, false);
        Assert.That(msg.RenderExpected(), Is.EqualTo("threshold was set to 0.75"));
    }

    [Test]
    public void RenderActual_Threshold_ReturnCorrectSentence()
    {
        var msg = new ScoreMessageMarkdown();
        msg.Initialize(0.62m, 0.75m, false);
        Assert.That(msg.RenderActual(), Is.EqualTo("score is 0.62"));
    }

    [Test]
    public void RenderMessage_Insufficient_ReturnCorrectSentence()
    {
        var msg = new ScoreMessageMarkdown();
        msg.Initialize(0.62m, 0.75m, false);
        Assert.That(msg.RenderMessage(), Does.Contain("insufficient score"));
    }

    [Test]
    public void RenderMessage_Good_ReturnCorrectSentence()
    {
        var msg = new ScoreMessageMarkdown();
        msg.Initialize(0.98m, 0.75m, true);
        Assert.That(msg.RenderMessage(), Does.Contain("good score"));
    }

}

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

public class ScoreMessageJsonTest
{
    [Test]
    public void RenderMessage_Insufficient_ReturnCorrectJson()
    {
        var msg = new ScoreMessageJson();
        msg.Initialize(0.62m, 0.75m, false);
        Assert.That(msg.RenderMessage(), Does.EndWith("\"success\":false,\"score\":0.62,\"threshold\":0.75}"));
        Assert.That(msg.RenderMessage(), Does.StartWith("{\"timestamp\":\"20"));
    }

    [Test]
    public void RenderMessage_Good_ReturnCorrectJson()
    {
        var msg = new ScoreMessageJson();
        msg.Initialize(0.98m, 0.75m, true);
        Assert.That(msg.RenderMessage(), Does.EndWith("\"success\":true,\"score\":0.98,\"threshold\":0.75}"));
        Assert.That(msg.RenderMessage(), Does.StartWith("{\"timestamp\":\"20"));
    }

}

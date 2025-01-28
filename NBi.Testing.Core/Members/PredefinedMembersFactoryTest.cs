using System;
using System.Linq;
using NBi.Core.Members.Predefined;
using NUnit.Framework;

namespace NBi.Core.Testing.Members;

[TestFixture]
public class PredefinedMembersFactoryTest
{
     
    #region SetUp & TearDown
    //Called only at instance creation
    [OneTimeSetUp]
    public void SetupMethods()
    {

    }

    //Called only at instance destruction
    [OneTimeTearDown]
    public void TearDownMethods()
    {
    }

    //Called before each test
    [SetUp]
    public void SetupTest()
    {
    }

    //Called after each test
    [TearDown]
    public void TearDownTest()
    {
    }
    #endregion

    [Test]
    public void Instantiate_DaysOfWeekInFrench_ListFromLundiToDimanche()
    {
        var factory = new PredefinedMembersFactory();
        var days = factory.Instantiate(PredefinedMembers.DaysOfWeek, "fr").ToList();

        Assert.That(days[0], Is.EqualTo("Lundi"));
        Assert.That(days[6], Is.EqualTo("Dimanche"));
        Assert.That(days.Count, Is.EqualTo(7));
    }

    [Test]
    public void Instantiate_DaysOfWeekInEnglish_ListFromMondayToSunday()
    {
        var factory = new PredefinedMembersFactory();
        var days = factory.Instantiate(PredefinedMembers.DaysOfWeek, "en").ToList();

        Assert.That(days[0], Is.EqualTo("Monday"));
        Assert.That(days[6], Is.EqualTo("Sunday"));
        Assert.That(days.Count, Is.EqualTo(7));
    }

    [Test]
    public void Instantiate_MonthsOfYearInEnglish_ListFromJanuaryToDecember()
    {
        var factory = new PredefinedMembersFactory();
        var months = factory.Instantiate(PredefinedMembers.MonthsOfYear, "en").ToList();

        Assert.That(months[0], Is.EqualTo("January"));
        Assert.That(months[11], Is.EqualTo("December"));
        Assert.That(months.Count, Is.EqualTo(12));
    }
}

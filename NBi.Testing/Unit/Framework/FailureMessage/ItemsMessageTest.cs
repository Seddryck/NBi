using Moq;
using NBi.Core;
using NBi.Framework.FailureMessage;
using NBi.Framework.FailureMessage.Markdown;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Framework.FailureMessage
{
    public class ItemsMessageTest
    {
        #region Helpers
        private IEnumerable<string> GetDataRows(int count)
        {
            var list = new List<string>();
            for (int i = 0; i < count; i++)
                list.Add(String.Format("Item {0:00}", i));

            return list;
        }
        #endregion

        [Test]
        public void RenderExpected_MoreThanMaxRowsCount_ReturnCorrectNumberOfRowsOnTop()
        {
            var list = new List<string>();
            for (int i = 0; i < 20; i++)
                list.Add(String.Format("Item {0:00}", i));

            var msg = new ItemsMessage(FailureReportProfile.Default);
            msg.Build(list, null, null);
            var value = msg.RenderExpected();
            var lines = value.Replace("\n", string.Empty).Split('\r');
            var firstLine = lines[0];

            Assert.That(firstLine, Is.EqualTo("Set of 20 items"));
        }

        public void RenderExpected_OneRow_ReturnCorrectNumberOfRowsOnTopWithoutPlurial()
        {
            var list = new List<string>();
            list.Add("Item 01");

            var msg = new ItemsMessage(FailureReportProfile.Default);
            msg.Build(list, null, null);
            var value = msg.RenderExpected();
            var lines = value.Replace("\n", string.Empty).Split('\r');
            var firstLine = lines[0];

            Assert.That(firstLine, Is.EqualTo("Set of 1 item"));
        }

        public void RenderExpected_MoreThanMaxRowsCount_ReturnSampleRowsCountAndHeaderAndSeparation()
        {
            var list = new List<string>();
            for (int i = 0; i < 20; i++)
                list.Add(String.Format("Item {0:00}", i));

            var msg = new ItemsMessage(FailureReportProfile.Default);
            msg.Build(list, null, null);
            var value = msg.RenderExpected();
            var lines = value.Replace("\n", string.Empty).Split('\r');


            Assert.That(lines.Count(l => l.Contains("*")), Is.EqualTo(10));
        }

        [Test]
        public void RenderExpected_MoreThanSampleRowsCountButLessThanMaxRowsCount_ReturnEachRowAndHeaderAndSeparation()
        {
            var rowCount = 12;

            var list = new List<string>();
            for (int i = 0; i < rowCount; i++)
                list.Add(String.Format("Item {0:00}", i));

            var msg = new ItemsMessage(FailureReportProfile.Default);
            msg.Build(list, null, null);
            var value = msg.RenderExpected();
            var lines = value.Replace("\n", string.Empty).Split('\r');


            Assert.That(lines.Count(l => l.Contains("*")), Is.EqualTo(rowCount));
        }

        [Test]
        public void RenderExpected_MoreThanSampleRowsAndMaxRowsCountWithSpecificFailureReportProfile_ReturnEachRowAndHeaderAndSeparation()
        {
            var rowCount = 120;
            var threshold = rowCount - 20;
            var max=threshold/2;

            var list = new List<string>();
            for (int i = 0; i < rowCount; i++)
                list.Add(String.Format("Item {0:00}", i));

            var profile = Mock.Of<IFailureReportProfile>(p =>
                p.MaxSampleItem == max
                && p.ThresholdSampleItem == threshold
                && p.ExpectedSet == FailureReportSetType.Sample
            );

            var msg = new ItemsMessage(profile);
            msg.Build(list, null, null);
            var value = msg.RenderExpected();
            var lines = value.Replace("\n", string.Empty).Split('\r');


            Assert.That(lines.Count(l => l.Contains("*")), Is.EqualTo(max));
        }

        [Test]
        public void RenderExpected_MoreThanMaxRowsCount_ReturnCorrectCountOfSkippedRow()
        {
            var list = new List<string>();
            for (int i = 0; i < 22; i++)
                list.Add(String.Format("Item {0:00}", i));

            var msg = new ItemsMessage(FailureReportProfile.Default);
            msg.Build(list, null, null);
            var value = msg.RenderExpected();
            var lines = value.Replace("\n", string.Empty).Split('\r');
            //Not exactly the last line but the previous due to paragraph rendering.
            var lastLine = lines.Reverse().ElementAt(1);

            Assert.That(lastLine, Is.EqualTo("... and 12 others not displayed."));
        }

        [Test]
        [TestCase(5)]
        [TestCase(12)]
        public void RenderExpected_LessThanMaxRowsCount_DoesntDisplaySkippedRow(int rowCount)
        {
            var list = new List<string>();
            for (int i = 0; i < rowCount; i++)
                list.Add(String.Format("Item {0:00}", i));

            var msg = new ItemsMessage(FailureReportProfile.Default);
            msg.Build(list, null, null);
            var value = msg.RenderExpected();

            Assert.That(value, Is.Not.StringContaining(" others not displayed."));
        }

        [Test]
        [TestCase(0, 5, "Missing items:")]
        [TestCase(5, 0, "Unexpected items:")]
        public void RenderCompared_NoSpecialRows_DoesntDisplayTextForThisKindOfRows(
            int missingRowCount
            , int unexpectedRowCount
            , string unexpectedText)
        {
            var compared = new ListComparer.Result(
                    GetDataRows(missingRowCount)
                    , GetDataRows(unexpectedRowCount)
                );


            var msg = new ItemsMessage(FailureReportProfile.Default);
            msg.Build(null, null, compared);
            var value = msg.RenderCompared();

            Assert.That(value, Is.Not.StringContaining(unexpectedText));
        }

        [Test]
        [TestCase(3, 0, "Missing items:")]
        [TestCase(0, 3, "Unexpected items:")]
        public void RenderCompared_WithSpecialRows_DisplayTextForThisKindOfRows(
            int missingRowCount
            , int unexpectedRowCount
            , string expectedText)
        {
            var compared = new ListComparer.Result(
                    GetDataRows(missingRowCount)
                    , GetDataRows(unexpectedRowCount)
                );

            var msg = new ItemsMessage(FailureReportProfile.Default);
            msg.Build(null, null, compared);
            var value = msg.RenderCompared();

            Assert.That(value, Is.StringContaining(expectedText));
        }

    }
}

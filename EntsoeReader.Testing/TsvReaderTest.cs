using NBi.Core.FlatFile;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Entsoe.FileManagement.Testing
{
    [TestFixture]
    public class TsvReaderTest
    {
        [Test]
        public void Read_EntsoeFile_Get24Records()
        {
            var reader = new TsvReader();
            var dataTable = reader.ToDataTable(@"Resources\Entsoe.csv");

            Assert.That(dataTable.Columns.Count, Is.EqualTo(13));
            Assert.That(dataTable.Rows.Count, Is.EqualTo(24));
        }

        [Test]
        public void Read_EntsoeFile_Get14RecordsFromBelgium()
        {
            var reader = new TsvReader(@"10YBE----------2");
            var dataTable = reader.ToDataTable(@"Resources\Entsoe.csv");

            Assert.That(dataTable.Columns.Count, Is.EqualTo(13));
            Assert.That(dataTable.Rows.Count, Is.EqualTo(14));
        }

        [Test]
        public void Read_EntsoeFile_Get10RecordsFromFrance()
        {
            var reader = new TsvReader(@"10YFR-RTE------C");
            var dataTable = reader.ToDataTable(@"Resources\Entsoe.csv");

            Assert.That(dataTable.Columns.Count, Is.EqualTo(13));
            Assert.That(dataTable.Rows.Count, Is.EqualTo(10));
        }

        [Test]
        public void Compare_Readers_TsvFasterThanStd()  
        {
            var stopWatch = new Stopwatch();
            var tsvReader = new TsvReader(@"10YBE");
            stopWatch.Start();
            var tsvDataTable = tsvReader.ToDataTable(@"Resources\Entsoe.csv");
            stopWatch.Stop();
            var tsvElapsedTime = stopWatch.Elapsed;

            var stdReader = new CsvReader(new CsvProfile('\t', '\"', "\r\n", false, false, "(empty)", "(null)"));
            stopWatch.Start();
            var stdDataTable = stdReader.ToDataTable(@"Resources\Entsoe.csv");
            stopWatch.Stop();
            var stdElapsedTime = stopWatch.Elapsed;

            Assert.That(tsvDataTable.Columns.Count, Is.EqualTo(stdDataTable.Columns.Count));
            Assert.That(tsvDataTable.Rows.Count, Is.LessThanOrEqualTo(stdDataTable.Rows.Count));

            Console.WriteLine($"{tsvElapsedTime.TotalMilliseconds} vs {stdElapsedTime.TotalMilliseconds} => {Math.Round(tsvElapsedTime.Ticks * 100.0 / stdElapsedTime.Ticks , 2)}%");
            Assert.That(tsvElapsedTime, Is.LessThanOrEqualTo(stdElapsedTime));
        }
    }
}

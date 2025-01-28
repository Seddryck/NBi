using NBi.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing;

[TestFixture]
public class OfficeDataConnectionFileParserTest
{
    [Test]
    public void GetConnectionStringFromText_ValidText_CorrectConnectionString()
    {
        var text = "<html xmlns:o=\"urn:schemas-microsoft-com:office:office\"xmlns=\"http://www.w3.org/TR/REC-html40\"><head>";
        text += "  <odc:ConnectionString>Data Source=foo;initial catalog=bar</odc:ConnectionString> ";
        text +="</head></html>";

        var parser = new OfficeDataConnectionFileParser();
        var connectionString = parser.GetConnectionStringFromText(text);
        Assert.That(connectionString, Is.EqualTo("Data Source=foo;initial catalog=bar"));
    }
}

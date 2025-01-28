using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core;

public class OfficeDataConnectionFileParser
{
    private string BasePath { get; set; }

    public OfficeDataConnectionFileParser()
        : this(string.Empty)
    { }

    public OfficeDataConnectionFileParser(string basePath)
    {
        BasePath = basePath;
    }

    public string GetConnectionString(string path)
    {
        if (!Path.IsPathRooted(path))
            path = BasePath + path;

        if (!File.Exists(path))
            throw new FileNotFoundException(string.Format("Impossible to read the connection from odc file. The file '{0}' doesn't exist.", path));

        var text = File.ReadAllText(path);
        return GetConnectionStringFromText(text);
    }

    internal string GetConnectionStringFromText(string text)
    {
        var startConnectionTag = text.IndexOf("<odc:ConnectionString");
        if (startConnectionTag == -1)
            throw new InvalidDataException(string.Format("Impossible to read the connection from odc file. This file has no tag '<odc:connection>'. "));
        startConnectionTag = text.IndexOf(">", startConnectionTag)+1;

        var endConnectionTag = text.IndexOf("</odc:ConnectionString");

        var connectionString = text[startConnectionTag..endConnectionTag];
        return connectionString;
    }
}

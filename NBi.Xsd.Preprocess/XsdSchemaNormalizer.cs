// XSD Schema Include Normalizer
// To compile: 
// csc filename.cs
//
// How to use:
//
// Arguments: [-q] input.xsd [output.xsd]
//
// input.xsd       - file to normalize
// output.xsd      - file to output, default is console
// -q              - quiet
// 
// Example:
// 
// filename.exe schema.xsd
// 
using System;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Collections;
namespace NBi.Xsd.Preprocess;

public class XsdSchemaNormalizer
{
    private static bool NormalizeXmlSchema(string url, TextWriter writer)
    {
        try
        {
            var txtRead = new XmlTextReader(url);
            var sch = XmlSchema.Read(txtRead, null) ?? throw new NullReferenceException();
            var schemaSet = new XmlSchemaSet();
            schemaSet.Add(sch);

            // Compiling Schema
            schemaSet.Compile();

            var outSch = XmlSchemaIncludeNormalizer.BuildIncludeFreeXmlSchema(sch);

            outSch.Write(writer);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            return false;
        }
        return true;
    }

    public static void usage()
    {
        Console.WriteLine("Arguments: [-q] [-v] input.xsd [output.xsd]\n");
        Console.WriteLine("input.xsd       - file to normalize");
        Console.WriteLine("output.xsd      - file to output, default is console");
        Console.WriteLine("-q              - quiet");
    }
    
    public static void Main(String[] args)
    {
        if (args.GetLength(0) < 1)
        {
            usage();
            return;
        }
        int argi = 0;
        bool quiet = false;
        if (args[argi] == "-q")
        {
            quiet = true;
            argi++;
        }

        if (argi == args.GetLength(0))
        {
            usage();
            return;
        }

        String url = args[argi];

        if (!quiet)
            Console.WriteLine("Loading Schema: " + url);

        if (argi < (args.GetLength(0) - 1))
        {
            if (!quiet)
                Console.WriteLine("Outputing to file: " + args[argi + 1]);

            var output =
           new StreamWriter(new FileStream(args[argi + 1], FileMode.Create));

            NormalizeXmlSchema(url, output);
        }
        else
        {
            NormalizeXmlSchema(url, Console.Out);
        }

    }
}

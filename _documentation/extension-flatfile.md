---
layout: documentation
title: Flat file extensions
prev_section: extension-installation
next_section: config-defaults-references
permalink: /docs/extension-flatfile/
---

## Concept

It's possible to create your own flat-file parser and specify in the test-suite that you'll be using the parser to read some files.

## Create a custom parser

To create a custom parser for flat files, you'll need to develop your own parser in C#. To achieve this, you'll need to create a C# library project and compile it as a library. One of the class of your project should implement the interface *IFlatFileReader* from the namespace *NBi.Extensibility.FlatFile* and available in the nuget package *NBi.Extensibility*.

The following example, illustrates a TSV (table-separated values) parser skipping rows where the substring "10YBE" is not available.

{% highlight csharp %}
public class TsvReader : CsvReader, IFlatFileReader
{
    public bool IsFirstLine { get; set; } = true;
    public TsvReader()
        : base(new CsvProfile('\t', '\"', "\r\n", true, true, 4096, "(empty)", "(null)"))
    {
        base.ProgressStatusChanged += (s, e) 
            => ProgressStatusChanged?.Invoke(this
            , new NBi.Extensibility.ProgressStatusEventArgs(e.Status, e.Progress.Current, e.Progress.Total));
    }
    public new event NBi.Extensibility.ProgressStatusHandler ProgressStatusChanged;
    public new DataTable ToDataTable(string filename) => base.ToDataTable(filename);
    protected override IEnumerable<string> GetNextRecords(StreamReader reader, string recordSeparator, int bufferSize, string alreadyRead, out string extraRead)
    {
        extraRead = string.Empty;
        while (!reader.EndOfStream)
        {
            var value = reader.ReadLine();
            
            //The first line should also be submitted because it will be skipped in the base class implementation
            if (((!string.IsNullOrEmpty(value)) && value.Contains("10YBE")) || IsFirstLine)
            {
                IsFirstLine = false;
                return new List<string>() { value };
            }
        }
        return new List<string>();
    }
    protected override IEnumerable<string> SplitLine(string row, char fieldSeparator, char textQualifier, char escapeTextQualifier, string emptyCell)
        => row.Split(new[] { fieldSeparator }).Select(x => x == string.Empty ? emptyCell : x);
    protected override string CleanRecord(string record, string recordSeparator)
        => record;
}
{% endhighlight %}

### Register the extension

You've to inform NBi that an extension is installed. To achieve this, you'll have to edit your *configuration file* and add the section *extensions* to the *nbi* section of your file. for more information check [how to register an extension for databases](../extension-installation)

{% highlight csharp %}
<extensions>
  <add assembly="NBi.Testing" extension="custom"/>
</extensions>
{% endhighlight %}

## Reference the custom parser

### Inline reference

To specify that a flat file should be read with a specific custom parser, you'll need to explictely tell it in the test definition. Immediately after the filename, add a exclamation point (!) and foolow it by the custom parser's name (defined in the parameter *extension* of your *config* file).

{% highlight csharp %}
<system-under-test>
  <result-set file="..\Csv\entsoe.tsv!custom"></result-set>
</system-under-test>
{% endhighlight %}

### Explicit reference

Another way to express the need of a custom parser is to use the long form of a file reference.

{% highlight csharp %}
<system-under-test>
  <result-set>
    <file path="..\Csv\entsoe.tsv">
      <parser name="custom" />
    </file>
  </result-set>
</system-under-test>
{% endhighlight %}
---
layout: documentation
title: Transform a column
prev_section: primitive-result-set
next_section: query-syntax
permalink: /docs/transform-column/
---
It happens that it's impossible (stored procedures) or not suitable (readibility of the test) to tune the query in your assertion to match with the expectations of your *system-under-test*. The feature *transform* is built for these cases.

The element *transform* works as a modification of the content of all cells (scalar) for a given column.

## Languages

At the moment, you can make usage of three different languages and a few ready-to-go transformations provided by the framework to define your transformation:

* NCalc - ```ncalc```
* Format - ```format```
* C# - ```c-sharp```
* Native - ```native```

These options support different purposes:

*NCalc* is specifically dedicated to calculations on numerical values but also offers limited features for booleans and texts.

*Format* is a quick way to transform a *dateTime* or *numeric* to a text representation taking into account culture specific formats (leading 0, 2 or 4 digits for years, coma or point for decimal spearator and many more).

*C#* is there to support advanced transformations requiring a little bit more of code.

*Native* is a collection of ready-to-go transformations for general purposes. It's especially useful around *null*, *empty*, *blank* and *text*

| Language | source | destination
| ----------------------------
| NCalc | numeric (limited for boolean and text)| unchanged
| Format | numeric or dateTime | text
| C# | all | all
| Native | text or dateTime | text or numeric

Technically, the destination could be something else that the option(s) defined in the table above, but most of the time it has no added-value.

The content of the cell is always casted on .NET based on the information provided into the attribute *original-type*

### NCalc

The whole documentation of this language and the supported syntax is available on the website of this project
(especially [here](https://ncalc.codeplex.com/wikipage?title=functions&referringTitle=Home) and [there](https://ncalc.codeplex.com/wikipage?title=operators&referringTitle=Home)).

The cell's content is available through the variable *value*.

The exemple here under is transformating the content of two columns:

* The first column is hosting a *numeric* that will be multiplied by 1.21 
* The second column is hosting a *numeric* that will be divided by 1000 before rounded with 2 digits.

{% highlight xml %}
<assert>
  <equal-to>
    <column index="1" role="value" type="text">
      <transform language="ncalc" original-type="numeric">value * 1.21</transform>
    </column>
    <column index="2" role="value" type="text">
      <transform language="ncalc" original-type="numeric">Round(value/1000, 2)</transform>
    </column>
  </equal-to>
</assert>
{% endhighlight %}

### Format

The syntax is exactly the one supported by the *Format* function and is available [for dateTime](https://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx) and [numeric](https://msdn.microsoft.com/en-us/library/0c899ak8(v=vs.110).aspx)

The exemple here under is transformating the content of three columns:

* The first column is hosting a *dateTime* that will be converted to a year/month representation: 4 digits for years, 2 for months and seperated by a dot.
* The second column is hosting a *numeric* that will be converted to a currency representation: a euro symbol followed by 3 digits before the decimal separator (a coma in this case because we use the french culture) and 2 digits after this seperator 
* The third column is hosting a *numeric* that will be converted to a currency representation but this time using k€ (thousand of euros, the cell content with be divided by 1000).

{% highlight xml %}
<assert>
  <equal-to>
    <column index="0" role="key" type="text">
      <transform language="format" original-type="dateTime">yyyy.MM</transform>
    </column>
    <column index="1" role="value" type="text" culture="fr-fr">
      <transform language="format" original-type="numeric">€000.00</transform>
    </column>
    <column index="2" role="value" type="text">
      <transform language="format" original-type="numeric">k€000,</transform>
    </column>
  </equal-to>
</assert>
{% endhighlight %}

### C-Sharp

Currently, it's limited to one line of code (one expression) but later you'll be able to call functions defined in an assembly or directly write a few lines of code in your test's definition. The syntax is the syntax supported by C# 5.0 without a terminator (;) at the end of the expression.

The cell's content is available through the variable *value*.

The exemple here under is transformating the content of two columns:

* The first column is hosting a *dateTime* that will be manipulated to add a month and extract the year and month
* The second column is hosting a *numeric* that will be multiplied by 1.21 after taking the absolute value
* The third column is hosting a *numeric* that will be divided by 1000 before rounded with 2 digits, then we'll add the symbols k€ in front of the result.

{% highlight xml %}
<assert>
  <equal-to>
    <column index="0" role="key" type="text">
      <transform language="c-sharp" original-type="dateTime">
        value.AddMonth(1).Year.ToString() + "." + (value.AddMonth(1).Month.ToString()
      </transform>
    </column>
    <column index="1" role="value" type="text">
      <transform language="c-sharp" original-type="numeric">
        Math.Abs(value * 1.21)
      </transform>
    </column>
    <column index="2" role="value" type="text">
      <transform language="c-sharp" original-type="numeric">
        value < 5000 ? string.Format(€{0:##00.00}, value) : "k€" + Math.Round(value/1000, 2).ToString()
      </transform>
    </column>
  </equal-to>
</assert>
{% endhighlight %}

### Native

Currently, you cannot assemble native transformations, it means that you're limited to use one and only one of them in a column's transformation.

* ```blank-to-empty```: if the current content of the cell is ```blank``` (zero or many spaces) replace the content by ```(empty)```
* ```blank-to-null```: if the current content of the cell is ```blank``` (zero or many spaces) replace the content by ```(null)```
* ```empty-to-null```: if the current content of the cell is ```empty``` (length=0) replace the content by ```(null)```
* ```null-to-empty```: if the current content of the cell is ```null``` replace the content by ```(empty)```
* ```null-to-value```: if the current content of the cell is ```null``` replace the content by ```(value)```
* ```any-to-any```: replaces the content of each cell by ```(any)```
* ```value-to-value```: if the cell's value is not ```null``` will replace the content by ```(value)```
* ```text-to-without-diacritics```: if the current cell's value contains any accents or diacritics, they are removed
* ```text-to-without-whitespaces```: removes blanks from anywhere within the cell. If the cell is ```null```, it returns ```null``` but if ```empty``` or ```blank```, it returns ```empty```.
* ```text-to-upper```: returns a copy of this text converted to uppercase
* ```text-to-lower```: returns a copy of this text converted to lowercase
* ```html-to-text```: decodes the html to text
* ```text-to-html```: encodes the content to html
* ```text-to-trim```: removes blanks from the beginning and end of the cell.
* ```text-to-length```: returns the length of the *text* value of the cell. If the cell is ```null``` or ```empty```, it returns 0.
* ```text-to-token-count```: returns the count of tokens. A token is considered as one or more letter or digit or hyphen seperated by one or more whitespace. If the cell is ```null``` or ```empty``` or ```blank```, it returns 0.
* ```null-to-zero```: if the cell is ```null``` or ```empty``` or ```blank```, it replaces the content by ```0```.
* ```numeric-to-floor```: returns the largest integral value less than or equal to the specified number. If the cell is ```null``` or ```empty``` or ```blank```, it returns ```null```.
* ```numeric-to-ceiling```: returns the smallest integral value greater than or equal to the specified number. If the cell is ```null``` or ```empty``` or ```blank```, it returns ```null```.
* ```numeric-to-integer```: rounds a value to the nearest integer. If the cell is ```null``` or ```empty``` or ```blank```, it returns ```null```.
* ```date-to-age```: returns the age according to the *dateTime* value of the cell at the moment of execution of the test.
* ```dateTime-to-date```: remove information about the time (equivalent to set the dateTime to midnight)
* ```dateTime-to-first-of-month```: returns the first day of the month where a given date lies in.
* ```dateTime-to-first-of-year```: returns the first day of the year where a given date lies in.
* ```dateTime-to-last-of-month```: returns the last day of the month where a given date lies in.
* ```dateTime-to-last-of-year```: returns the last day of the year where a given date lies in.
* ```dateTime-to-next-day```: returns the next day.
* ```dateTime-to-previous-day```: returns the previous day.
* ```dateTime-to-next-month```: returns a dateTime corresponding to one month after the given date.
* ```dateTime-to-previous-month```: returns a dateTime corresponding to one month before the given date.
* ```dateTime-to-next-year```: returns a dateTime corresponding to one year after the given date.
* ```dateTime-to-previous-year```: returns a dateTime corresponding to one year before the given date.

#### Parameterized transformations

The following transformations except parameters to operate. You must replace the information beween parenthesis with a string matching your expectation.

The parameter is a valid TimeZone. User must specify the identification of a time zone (Romance Standard Time ...) or the name of one of the city listed in the display (Brussels, Paris ...).

* ```utc-to-local(TimeZone)```: returns the dateTime converted from UTC to the local time of the specified time zone
* ```local-to-utc(TimeZone)```: returns the dateTime converted from the local time of the specified time zone to utc. If the local time was ambiguous (at the moment of the switch between summer and winter the same local time occurs twice) then the first occurance is selected.
* ```null-to-date(dateTime)```: returns the original date if the value wasn't null or empty else returns the value specified as a parameter. ```dateTime``` must be expressed as string: ```2018-05-09```
* ```numeric-to-round(integer)```: rounds a value to the specified number of fractional digits.
* ```numeric-to-clip(numeric, numeric)```: Clip a value such as if smaller than the first argument then it will return the first argument or if larger than the second argument then will return the second argument. If the original value is between the first and second argument then the original value is returned.
* ```dateTime-to-clip(dateTime, dateTime)```: Clip a value such as if smaller than the first argument then it will return the first argument or if larger than the second argument then will return the second argument. If the original value is between the first and second argument then the original value is returned.
* ```dateTime-to-set-time(timeSpan)```: Set the hours, minutes, second of a dateTime to the specified value without changing the date part. The timespan should be defined with the format *hh:mm:ss* such as ```07:00:00```.

#### Path and file transformations

The following transformations will consider the location of the test-suite as the base path when facing a relative path.

* ```path-to-filename```:  returns the file name and extension of the specified path string. The characters after the last directory separator character in path. If the last character of path is a directory or volume separator character, this method returns ```empty```.
* ```path-to-filename-without-extension```: Returns the file name of the specified path string without the extension. The text returned by ```path-to-filename```, minus the last period (.) and all characters following it.
* ```path-to-extension```: Returns the extension (including the period ".") of the specified path string. The extension of the specified path (including the period ".") or ```(empty)```.
* ```path-to-root```: Gets the root directory information of the specified path including a directory separator character at the end.
* ```path-to-directory```: returns the directory information for the specified path string. 

For these transformations, the input must corresponds to an existing file. If it's not the case an exception will be generated and the test will fail.

* ```file-to-size```: Gets the size, in bytes, of the file.
* ```file-to-creation-dateTime```: Gets the creation time of the file.
* ```file-to-creation-dateTime-utc```: Gets the creation time, in coordinated universal time (UTC), of the file.
* ```file-to-update-dateTime```: Gets the time that the current file was last written to.
* ```file-to-update-dateTime-utc```: Gets the time, in coordinated universal time (UTC), that the current file was last written to.

{% highlight xml %}
<assert>
  <equal-to>
    <column index="0" role="key" type="text">
      <transform language="native" original-type="text">
        string-to-trim
      </transform>
    </column>
    <column index="1" role="value" type="text">
      <transform language="native" original-type="text">
        empty-to-null
      </transform>
    </column>
    <column index="2" role="value" type="text">
      <transform language="native" original-type="text">
        any-to-value
      </transform>
    </column>
    <column index="3" role="value" type="dateTime">
      <transform language="native" original-type="dateTime">
        dateTime-to-clip(2010-01-01, 2019-12-31)
      </transform>
    </column>
  </equal-to>
</assert>
{% endhighlight %}
---
layout: documentation
title: Result-set
prev_section: primitive-sequence
next_section: transform-column
permalink: /docs/primitive-result-set/
---
Two-dimensional size-mutable, potentially heterogeneous tabular data structure with labeled axes (rows and columns). The primary NBi data structure.

## Definition

It's possible to specify a result-set in different ways.

### Inline definition

The most straightforward is to define rows and cells inline. This is relatively useful when your result-set is small.

{% highlight xml %}
<result-set>
  <row>
    <cell>Canada</cell>
    <cell>130</cell>
  </row>
  <row>
    <cell>Belgium</cell>
    <cell>45</cell>
  </row>
</result-set>
{% endhighlight %}

### External definition

You can also refer to an external CSV file:

{% highlight xml %}
<result-set file="myFile.csv"/>
{% endhighlight %}

the filename can be dynamically evaulated based on a variable (formatting). To enable this feature, you must precede the filename by a tilt ```~``` and mix static part of the filename with dynamic part. The dynamic part must be contained between curly barces ```{}``` and start by the variable name to consider.

{% highlight xml %}
<result-set file="File_{@myVar}.csv"/>
{% endhighlight %}

Using the previous notation, if the value of *myVar* is *2018* then the filename *File_2018.csv* will be considered for loading the result-set.

In case the variable is a numeric or dateTime, it can be useful to format it. This formatting must be specified after a column (```:```).

{% highlight xml %}
<result-set file="File_{@myDate:yyyy}_{@myDate:MM}.csv"/>
{% endhighlight %}

Using the previous notation, if the value of *myVar* is *1st January 2018* then the filename *File_2018_01.csv* will be considered for loading the result-set.

The formatting syntax is the one supported by .Net and explained in MSDN for the [numerics](https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-numeric-format-strings) and [dateTimes](https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)

### Sequences-based definition

You can define a result-set as the combination of one or more sequences. Each sequence creates a new column in the result-set. The resulting rows' count is depending on the combination type. Currently, the only combination-type supported is a *cartesian-product*. The *cartesian-product* will create one row for each combination of the different elements of the two sequences.

In the following definition, the two sequences contain 2 elements and 3 elements. The result of this combination will be a result-set with 2 columns (first of type *text*, next of type *dateTime*) and 6 rows.

{% highlight xml %}
<sequences-combination operation="cartesian-product">
  <sequence type="text">
    <item>be</item>
    <item>fr</item>
  </sequence>
  <sequence type="dateTime">
    <loop-sentinel seed="2015-01-01" terminal="2017-01-01" step="1 year"/>
  </sequence>
</sequences-combination>

{% endhighlight %}

### Query-based definition

Naturally, all the queries defined here under can take advantage of all features: [parameters](../query-parameter), [template-variables](../query-template), [timeout](../query-timeout).

#### Inline query

This query can be sourced from an inline definition

{% highlight xml %}
<result-set>
  <query>
    select * from myTable
  </query>
<result-set>
{% endhighlight %}

#### Query defined in an external file

{% highlight xml %}
<result-set>
  <query file="myQuery.sql"/>
<result-set>
{% endhighlight %}

#### Query defined in an assembly's method

More info about [assembly](../docs/query-assembly)

{% highlight xml %}
<result-set>
  <query>
    <assembly ...>
  <query>
<result-set>
{% endhighlight %}

#### Query defined in a report (SQL Server Reporting Server)

More info about [report](../docs/query-report#dataset)

{% highlight xml %}
<result-set>
  <query>
    <report ...>
  <query>
<result-set>
{% endhighlight %}

#### Query defined in a shared dataset (SQL Server Reporting Server)

More info about [shared-dataset](../docs/shared-dataset)

{% highlight xml %}
<result-set>
  <query>
    <shared-dataset ...>
  <query>
<result-set>
{% endhighlight %}

## Alterations

You can also define an alteration to the result-set. For the moment, three kinds of alterations are supported by NBi:

* [filter](../resultset-rows-count-advanced/#filter).
* [convert](../resultset-alterations/#converts)
* [transform](../transform-column/)

{% highlight xml %}
<result-set>
  <query>
    ...
  <query>
  <alteration>
    <filter ...>
  </alteration>
<result-set>
{% endhighlight %}
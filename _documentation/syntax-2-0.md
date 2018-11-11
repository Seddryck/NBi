---
layout: documentation
title: Syntax 2.0
permalink: /docs/syntax-2-0/
---
NBi was created in 2012 (as a New Year's resolution) and is supporting the same syntax in the last six years. Unfortunately, I made some poor decisions during the design of a few features. That’s the problem when you’re working on your free time: it’s not always possible to be smart when it’s 1.00AM or when you’ve been interrupted ten times by your daughters, your wife or sport at TV during the last 30 minutes :-) Anyway, it starts to be cumbersome to go forward and to make improvements to the tool without fixing these design issues. To fix these issues, some of the xml elements will need to be changed.  On the long-term, some of the syntaxes currently supported by NBi will not work anymore but we hope that the new syntax will be more user-friendly.

## PascalCase vs dashes

Some of the names of the xml elements are written in Pascal-case and not with dashes between words. This will be adapted along the way to the release 2.0 Until this moment both syntaxes will be supported. The current status is visible in this [issue](http://github.com/Seddryck/NBi/issues/288)

## *execution* replaced by *result-set*

The system-under-test *execution* was a bit ambiguous, sometimes used for performance or successfulness of a query/ETL and sometimes for the result-set of a query. The new syntax will clarify this by introducing the system-under-test *result-set*.

This result-set can be defined in different way.

### Inline definition

The most straightforward is to define rows and cells inline.

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

the filename can be dynamically evaulated based on a variable (formatting). To enable this featureou must precede the filename by a tilt ```~``` and mix static part of the filename with dynamic part. The dynamic part must be contained between curly barces ```{}``` and start by the variable name to consider.

{% highlight xml %}
<result-set file="File_{@myVar}.csv"/>
{% endhighlight %}

In case the variable is a numeric or dateTime, it can be useful to format it. This formatting must be specified after a column (```:```).

{% highlight xml %}
<resultSet file="File_{@myDate:yyyy}_{@myDate:MM}.csv"/>
{% endhighlight %}

The formatting syntax is the one supported by .Net and explained in MSDN for the [numerics](https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-numeric-format-strings) and [dateTimes](https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)

### Query-based definition

Naturally, all the queries defined here under can take advantage of all features: [parameters](../query-parameter), [template-variables](../query-template), [timeout](../query-timeout) for the old syntax of a query.

#### Inline query

This query can be sourced from an inline definition

{% highlight xml %}
<resultSet>
  <query>
    select * from myTable
  </query>
<resultSet>
{% endhighlight %}

#### Query defined in an external file

{% highlight xml %}
<resultSet>
  <query file="myQuery.sql"/>
<resultSet>
{% endhighlight %}

#### Query defined in an assembly's method

More info about [assembly](../docs/query-assembly)

{% highlight xml %}
<resultSet>
  <query>
    <assembly ...>
  <query>
<resultSet>
{% endhighlight %}

#### Query defined in a report (SQL Server Reporting Server)

More info about [report](../docs/query-report#dataset)

{% highlight xml %}
<resultSet>
  <query>
    <report ...>
  <query>
<resultSet>
{% endhighlight %}

#### Query defined in a shared dataset (SQL Server Reporting Server)

More info about [shared-dataset](../docs/shared-dataset)

{% highlight xml %}
<resultSet>
  <query>
    <shared-dataset ...>
  <query>
<resultSet>
{% endhighlight %}

## Alterations

You can also define an alteration to the result-set. For the moment, three kinds of alterations are supported by NBi:

* [filter](../resultset-rows-count-advanced/#filter).
* [convert](../resultset-alterations/#converts)
* [transform](../transform-column/)

{% highlight xml %}
<resultSet>
  <query>
    ...
  <query>
  <alteration>
    <filter ...>
  </alteration>
<resultSet>
{% endhighlight %}

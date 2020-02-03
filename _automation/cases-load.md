---
layout: automation
title: Load a set of test-cases
prev_section: comments
next_section: cases-save
permalink: /automation/cases-load/
---
This action lets you load from a file or query a list of test cases data. In addition to the action *load* you must supply from where you'll want to load your test cases.

## File (Csv)

The first option available is a CSV file, translated to *file* in the genbiL language. You must also supply the location of your file just after as a parameter.
Sample:
{% highlight xml %}
case load file 'Relative Directory\myFileWithTestCases.csv';
{% endhighlight %}

## Optional file (Csv)

Sometimes, you're not sure that the CSV file will effectively be there. You'd like that the script continue independently of the existence of this file with an empty case-set. In most cases, your script will fail if the case-set has not the expected columns. To avoid this, you'll have to specify some columns that will be added to the empty case-set.

{% highlight xml %}
case load optional file 'Relative Directory\myFileWithTestCases.csv' with columns 'foo', 'bar';
{% endhighlight %}

## Query

GenbiL offers the opportunity to load a set of test-cases from a database. To achieve this, you'll need to specify a query and the connection-string. You've two options to define your query:

* from a file
{% highlight xml %}
case load query 'Relative Directory\myQuery.sql' on 'Data Source=...';
{% endhighlight %}

* in-line
{% highlight xml %}
case load query
{
  select * from myTable
}
on 'Data Source=...';
{% endhighlight %}

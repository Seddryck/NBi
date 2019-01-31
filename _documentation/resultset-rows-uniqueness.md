---
layout: documentation
title: Rows' uniqueness
prev_section: resultset-rows-count-advanced
next_section: resultset-format
permalink: /docs/resultset-rows-uniqueness/
---
This assertion checks that all rows of a result-set are unique. If a row has two or more occurences the test will fail. To compare two rows of a result-set, all the columns will be used and all the columns will be considered as *text* column.

## System under test

The system-under-test is a query, please reports to other tests for more information about how to define a query. More info [here](/docs/compare-equivalence-resultsets).

## Assertion

The assertion consists in an xml element named *unique-rows*.
{% highlight xml %}
<assert>
  <unique-rows />
</assert>
{% endhighlight %}

By default this assertion considers that all columns are keys.

### Keys and values

The assertion is comparing two rows of the same datasets based on the columns defined as keys (by default all of them).  Values columns are just ignored and won't be reported in case of failure.

It's possible to define the keys by yourself by using columns' index or columns' name. If you're using indexes, you can specify the attribute *keys*

{% highlight xml %}
<assert>
  <unique-rows keys="first"/>
</assert>
{% endhighlight %}

If the predefined values are not enough, you can also use the following method to define the key columns.

{% highlight xml %}
<unique-rows>
  <column index="0" role="value"/>
  <column index="1" role="key" type="numeric"/>
</unique-rows>
{% endhighlight %}

{% highlight xml %}
<unique-rows>
  <column name="myFirstColumn" role="value"/>
  <column name="mySecondColumn" role="key" type="numeric"/>
</unique-rows>
{% endhighlight %}
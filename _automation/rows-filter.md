---
layout: automation
title: Filter rows
prev_section: column-concatenate
next_section: rows-distinct
permalink: /automation/rows-filter/
---
This command lets you remove some rows from your test-cases based on conditions.

## Filter based on the value of a column
It's possible to filter the test cases and only hold the test-cases with the expected value. It will save you to define multiple files with similar test-cases.

To filter a set of test-cases, you must specify the column on which you'll execute your filter and the operator (*equal* or *like*). You can also apply a negation to your operator to revert its result.

Examples:
{% highlight xml%}
case filter on column 'dimension' values like 'first-%' ;
{% endhighlight %}

{% highlight xml%}
case filter on column 'dimension' values not equal 'foo' ;
{% endhighlight %}

## Filter on empty and none values

You can also filter on two special values: *empty* and *none*. The value *empty* is useful to filter a value with a length of 0 character. The value *none* lets you filter when no value has been provided to some cells of the set of test-cases. The difference between empty and none is important when using the grouping option.

To achieve this kind of filter, just provide the value *empty* or *none* without the quotes surrounding usual values.

{% highlight xml%}
case filter on column 'dimension' values not equal none ;
case filter on column 'dimension' values equal empty ;
{% endhighlight %}

Usage of the operator like is tolerated but not really relevant and the behavior of genbiL will be exactly the same than with an equal.

## Filter on multiple values

You can apply an '*or*' condition in your filter expression by supplying multiples values. The expression *values equal '1', '2'* will return all the rows where the specified column has a value of '1' or '2'. By extension, the expression *values not equal '1', '2'* will return all the rows where the specified column hasn't a value of '1' and neither '2'.

Examples:
{% highlight xml%}
case filter on column 'dimension' values like 'first-%', '%bar' ;
{% endhighlight %}

{% highlight xml%}
case filter on column 'dimension' values not equal 'foo', 'bar' ;
{% endhighlight %}

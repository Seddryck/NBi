---
layout: documentation
title: Rows' uniqueness
prev_section: resultset-rows-count-advanced
next_section: 
permalink: /docs/resultset-rows-uniqueness/
---
This assertion checks that all rows of a result-set are unique. If a row has two or more occurences the test will fail. To compare two rows of a result-set, all the columns will be used and all the columns will be considered as *text* column.

## System under test

The system-under-test is a query, please reports to other tests for more information about how to define a query. More info [here](/docs/compare-equivalence-resultsets).

## Assertion

The assertion consists in an xml element named *no-duplicate*.
{% highlight xml %}
<assert>
  <no-duplicate />
</assert>
{% endhighlight %}


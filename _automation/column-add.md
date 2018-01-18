---
layout: automation
title: Add a column
prev_section: cases-cross
next_section: column-duplicate
permalink: /automation/column-add/
---
This action offers the opportunity to add a column to an existing set of test-cases.

In addition to the keyword *add* you must provide the name of this new column. The column will be added as the last column (most-right) of your set. The correct syntax is **add column 'column name'**.

Example:
{% highlight xml %}
case add column 'dimensionName';
{% endhighlight %}

## Default value

If you don't specify a default value for the newly added column, the default value will be *(none)*. To specify the initial value of this new column for each row, you must use the keyword *values* followed by the value.

Example:
{% highlight xml %}
case add column 'dimensionName' values 'my-value' ;
{% endhighlight %}

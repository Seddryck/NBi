---
layout: automation
title: Replace the content of a column
prev_section: column-move
next_section: column-substitute
permalink: /automation/column-replace/
---
This command lets you replace the value of a column by another value.

To achieve this you'll need to use the keyword *replace* and specify the impacted column then finally the new value of this column. The correct syntax is **case replace column 'column name' with values 'value'**.

Example:
{% highlight xml %}
case replace column 'alpha' with values 'my new value';
{% endhighlight %}

## Conditional replacement

If you want to replace the value of only of a subset of the rows in your set of test-cases then you'll need to add a condition. A condition for a replacement is expressed in the same way than the condition for a [filter](../rows-filter), check this page for more information.

Example:
{% highlight xml %}
case replace column 'alpha' with values 'my new value' when values not equal 'foo', empty, 'bar';
{% endhighlight %}

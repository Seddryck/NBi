---
layout: automation
title: Move a column
prev_section: column-rename
next_section: column-replace
permalink: /automation/column-move/
---
This command lets you move a column to the right or to the left in your set of test-cases. This is useful when you want to generate your test suite with the *grouping* option.

In addition to the keyword *move* you must provide the column's name and the direction (left or right). The correct syntax is **move column 'name' to [left % right % first % last]**. Note that left and right are considered as keywords and don't need simple quotes around them. If you want to move the column twice to the left, you'll need to repeat twice this code line.

Example:
{% highlight xml %}
case move column 'dimensionName' to left;
case move column 'dimensionName' to right;
{% endhighlight %}

It's possible to move a column to the first or last position.

{% highlight xml %}
case move column 'dimensionName' to first;
case move column 'dimensionName' to last;
{% endhighlight %}

---
layout: automation
title: Reduce arrays
prev_section: rows-group
next_section: rows-
permalink: /automation/rows-reduce/
---
This command will not decrease the count of rows but will reduce the size of the arrays in these rows.The command will, row by row, remove duplicates for the array-columns defined in parameters.

{% highlight xml%}
case reduce columns 'foo', 'bar';
{% endhighlight %}

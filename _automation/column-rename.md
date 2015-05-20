---
layout: automation
title: Rename a column
prev_section: column-hold
next_section: column-move
permalink: /automation/column-rename/
---
This command lets you rename a column (variable) in your test cases.

In addition to the keyword *rename* you must provide the current name of the column and its new name. The correct syntax is **rename_ column 'old name' into 'new_name'**

Example:
{% highlight xml %}
case rename column 'dimensionY' into 'dimension';
{% endhighlight %}

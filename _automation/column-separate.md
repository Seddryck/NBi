---
layout: automation
title: Concatenate content
prev_section: column-concatenate
next_section: rows-filter
permalink: /automation/column-separate/
---
This command is useful if you want to split a column into several columns. For this action you need to know how many columns will be created and you need to specify their names.

{% highlight xml%}
case separate column 'longtext' into 'foo', 'bar', 'remaining' with value '-';
{% endhighlight %}

If the original value contains more tokens than new columns defined the remaining tokens will be concatenated in the last column. If the original value has less tokens than columns specified, remaining columns will be credited of a value **(none)**

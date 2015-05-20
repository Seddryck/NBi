---
layout: automation
title: Hold some columns
prev_section: column-remove
next_section: column-rename
permalink: /automation/column-hold/
---
This command lets you remove some columns from your set of test-cases without naming them one by one. In place, you'll enumerate the column that you want to hold in your set of test-cases.

In addition to the keyword *hold* you must provide which columns (minimum 1) will be held (based on their names). The correct syntax is ** hold column 'column name', 'other column name' ** .

Example:
{% highlight xml %}
case hold column 'dimensionName', 'perspectiveName';
{% endhighlight %}

Note also that the command named [remove](../column-remove), could also be useful when you want to remove some columns from a set of test-cases.

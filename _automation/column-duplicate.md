---
layout: automation
title: Duplicate a column
prev_section: cases-add
next_section: column-remove
permalink: /automation/column-duplicate/
---
This action offers the opportunity to duplicate a column, one or more times, into an existing set of test-cases. For each row, the content of the newly created columns will be identical to the content of the original column.

In addition to the keyword *duplicate* you must provide the name of the column to duplicate and the names of the newly created columns. The columns will be added as the last columns (most-right) of your set. The correct syntax is **duplicate column 'column name' as 'new column 1', 'new column 2'**.

Example:
{% highlight xml %}
case duplicate column 'column name' as 'new column 1', 'new column 2';
{% endhighlight %}

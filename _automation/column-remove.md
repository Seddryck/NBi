---
layout: automation
title: Remove a column
prev_section: column-duplicate
next_section: column-hold
permalink: /automation/column-remove/
---
This action lets you remove a useless column from your test-cases.

In addition to the keyword *remove* you must provide which column will be removed (based on its name). The correct syntax is **remove column 'column name'**.

Example:
{% highlight xml %}
case remove column 'dimensionName';
{% endhighlight %}

It's possible to remove more than column in a single command. For this just enlist the list of columns to remove separated by a comma.

Example:
{% highlight xml %}
case remove column 'dimensionName', 'hierarchyName', 'levelName';
{% endhighlight %}

Note also that the command named [hold](../column-hold), could also be useful when you want to remove some columns.

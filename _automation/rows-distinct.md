---
layout: automation
title: Filter distinct
prev_section: rows-filter
next_section: rows-group
permalink: /automation/rows-distinct/
---
This command reduce the set of test-cases by returning only distinct (different) rows.

This is especially useful when building large tests-suites from large test-cases. When this set of test-cases is used for different cases, you will probably enjoy this command.

Example:
{% highlight xml%}
case filter distinct ;
{% endhighlight %}

---
title: Split into arrays
prev_section: rows-group
next_section: consumable-set
permalink: /automation/rows-split/
parent: "Rows of test-cases"
grand_parent: Automation
nav_order: 5
---
This command will transform a non-array value into an array. The unique value will be split into an array by the means of a separator. Each token will create a new value in the array.

The following value ```A/B/C``` will be converted to an array with three values after applying the following command. The separator is defined after the keywords ```with values```.

{% highlight xml%}
case split column 'foo' with value '/';
{% endhighlight %}

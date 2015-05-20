---
layout: automation
title: Create a copy of a set of test-cases
prev_section: cases-scope
next_section: cases-merge
permalink: /automation/cases-copy/
---
This action lets you take a copy of a set of test-cases (in its current state) into another scope.

To create this copy, you must specify the name of the set of test-cases that you want to copy and the name of the new set that you want to create.

Note that the new set that you want to create *must not exist* or the copy will fail!

Sample:
{% highlight xml %}
case copy 'dimension' to 'new-dimension';
{% endhighlight %}

---
layout: automation
title: Merge two sets of test-cases
prev_section: cases-copy
next_section: cases-cross
permalink: /automation/cases-merge/
---
This action offers the opportunity to merge two sets of test-cases. The result of this action is one set of test-cases containing the rows of the initial set and the merged set (you can copare this to a UNION in sql).

To merge a set with another one, you must first move the scope to one of the two sets. Then you must specify the *merge* command and finally the name of the second set.

Note that if the two sets haven't the same columns, the non-existing columns will be initialized with a null value in the merged set.

Sample:
{% highlight xml %}
case merge with 'new-dimension';
{% endhighlight %}

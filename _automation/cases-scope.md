---
layout: automation
title: Change scope to another set of test-cases
prev_section: cases-save
next_section: cases-copy
permalink: /automation/cases-scope/
---
You can load multiple sets of test-cases an combine them together with [merge](../cases-merge) and [cross](../cases-cross). Most of the actions that you can apply on test-cases, doesn't accept the name of the set of test-cases in parameter: the actions are applied on the current set of test-cases. To switch the focus between two sets of test-cases, you must use the *cases-scope* command. After the application of this command, the specified set of test-cases will be the target of the following actions.

Note that when [loading](../cases-load) a csv file or from a query, the set of test-cases is created in the scope with the focus. Don't forget to change the scope before loading a new set of test-cases.

The syntax for this command is *case scope* 'name of the set of test-cases'.

Sample:
{% highlight xml %}
case scope 'First Set';
// all the actions here under are executed on the set named 'First Set'
...

case scope 'Second Set';
// all the actions here under are executed on the set named 'Second Set'
...

// if you want to switch back to the first set, just call again the action "scope" on the set named 'First Set'
case scope 'First Set';
...

{% endhighlight %}

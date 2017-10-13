---
layout: automation
title: Set consumable
prev_section: rows-split
next_section: template-load
permalink: /automation/consumable-set/
---
This command lets you define a consumable that you're able to use in your templates. This is useful if you need to use a few consumables across all your templates and you want to avoid to add them as columns for each scope.

In the example, here under, a consumable named 'myVar' is set to the value 'value' and then consummed by the template.

{% highlight xml%}
consumable set 'myVar' to 'value';
{% endhighlight %}

{% highlight xml%}
<test name="my test">
  <category>$myVar$</category>
  ...
</test>
{% endhighlight %}
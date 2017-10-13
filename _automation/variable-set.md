---
layout: automation
title: Set variable
prev_section: rows-split
next_section: template-load
permalink: /automation/variable-set/
---
This command lets you define a variable that you're able to use in your templates. This is useful if you need to use a few variables across all your templates and you want to avoid to add them as columns of each scope.

In the example here under a variable named 'myVar' is set to the value 'value' and then consummed into the template.

{% highlight xml%}
variable set 'myVar' to 'value';
{% endhighlight %}

{% highlight xml%}
<test name="my test">
  <category>$myVar$</category>
  ...
</test>
{% endhighlight %}
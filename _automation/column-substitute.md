---
layout: automation
title: Substitute part of the content
prev_section: column-replace
next_section: column-concatenate
permalink: /automation/column-substitute/
---
At the difference of the command [replace](../column-replace), the command *substitute* doesn't replace the whole content of a cell but only a subpart of it. It has the same definition than the function *substitute* in excel or *replace* in SQL and .Net

You need to specify in which column to look for, then the substring to be replaced and finally the new value.

{% highlight xml%}
case substitute into column 'beta' value 'bar' with value 'foo';
{% endhighlight %}

## Referring to the content of other columns

It's possible to use the content of other columns to define the value to be replaced or the value of substitution. In place of the keyword *value*, you should use the keyword *column*.

{% highlight xml%}
case substitute into column 'beta' column 'alpha' with value 'foo';
case substitute into column 'beta' column 'alpha' with value 'gamma';
case substitute into column 'beta' value 'bar' with column 'gamma';
{% endhighlight %}

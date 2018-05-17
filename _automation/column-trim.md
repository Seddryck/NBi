---
layout: automation
title: Trim content of columns
prev_section: column-substitute
next_section: column-concatenate
permalink: /automation/column-trim/
---
If you want to remove white characters at the beginning or end of your content, this function is what you're looking for.

You need to specify in which columns you're expecting to remove the characters but also if you're considering the leading (left), trailing (right) or both characters.

The example here under is removing white characters at the end of the content of columns foor and bar.

{% highlight xml%}
case trim right columns 'foo', 'bar' ;
{% endhighlight %}

The example here under is removing white characters at the begiing and end of the content of column foor.

{% highlight xml%}
case trim column 'foo' ;
{% endhighlight %}

---
layout: automation
title: Format variables
prev_section: condition-template
next_section: source-test-cases-from-query
permalink: /automation/format-variable/
---
Sometimes, you need to render the value of a variable with a given format. A good case for this is when you want to render some specific elements such as < or > or & in an xml attribute. NBi supports the predefined StringRenderer from StringTemplate and specifically the following formats:

* upper
* lower
* cap
* url-encode
* xml-encode

To use this feature you need to match the StringTemplate syntax and specify the format after the semi-column:

{% highlight xml %}
<element attribute="$variable; format="xml-encode"$"/>
{% endhighlight %}

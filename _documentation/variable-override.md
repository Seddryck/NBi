---
layout: documentation
title: Override variables
prev_section: variable-define
next_section: variable-instance
permalink: /docs/variable-override/
---
Version 1.19 introduces the notion of *override* in a *variable*. This option offers the opportunity to enforce the value of a *variable* using a config file.

## Config file

To enforce the value of one or more variables, you must enlist them in the *variables* element of the config file. The attribute *name* specifies the name of the overriden variable, the attribute *value* specifies the value to apply to this variable and finally the attribute *type* sepcifies the type of the variable (could be *text*, *numeric*, *dateTime* or *boolean*).

{% highlight xml %}
<nbi ...>
  <variables>
    <add name="myDate" value="2013-12-01" type="DateTime"/>
    <add name="myNum" value="187" type="Numeric"/>
  </variables>
</nbi>
{% endhighlight %}
---
layout: automation
title: Load settings
prev_section: settings-load
next_section: suite-generate
permalink: /automation/variable-include/
---
For global variables, the command is always an *include*. You must specify a parameter option with the help of the keyword *file* and the name of the xml file containing the definition of the global variables.

Example:
{% highlight xml%}
variable include file 'global.nbivariables';
{% endhighlight %}
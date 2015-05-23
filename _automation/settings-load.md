---
layout: automation
title: Load settings
prev_section: template-load
next_section: suite-generate
permalink: /automation/settings-load/
---
For [settings (defaults and references)](../../docs/config-defaults-references), the command is implicit and is always an *add*. You must specify a parameter option with the help of the keywords *reference* or *default* to specify what you want to define. If the option is a reference, you must provide its name. Else you need to specify if the default is for *sut* (system-under-tests) or *assert* with the help of corresponding keywords. Then you need to specify which feature of the setting will be added. Only *connectionString* has been tested but others should work. Then finally, you need to specify the value that you want to set.

Example:
{% highlight xml%}
setting reference 'referenceName' connectionString 'data source=localhost; initial catalog=myDb';
setting default assert connectionString 'data source=localhost; initial catalog=myDb';
{% endhighlight %}

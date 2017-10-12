---
layout: automation
title: Load template
prev_section: variable-set
next_section: template-add
permalink: /automation/template-load/
---
This command lets you define the test-template that will be loaded by genbiL. This command unload all the templates that have been previously loaded. The command *load* has two options, you load from a predefined template embedded into the dll *NBi.genbi.dll* or you load your own template from your hard-disk.

## Load from predefined templates

If you've chosen an embedded template you must specify the name of this template. The name is the name displayed in the dropbox of genbiL without spaces!

{% highlight xml%}
template load embedded 'ExistsDimension';
{% endhighlight %}

Since release 1.16, the following syntax (keyword *predefined* in place of *embedded*) is deprecated

{% highlight xml%}
template load predefined 'ExistsDimension';
{% endhighlight %}

##Load from a file

If you've chosen the option *file* then you need to specify the path to access it.

{% highlight xml%}
template load file 'myTemplate.nbitt';
{% endhighlight %}

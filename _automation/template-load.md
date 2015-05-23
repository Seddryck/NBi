---
layout: automation
title: Load template
prev_section: rows-distinct
next_section: settings-load
permalink: /automation/template-load/
---
This command lets you define the test-template that will be loaded by genbiL. The command *load* has two options, you load from a predefined template embedded into genbi with the or you load you own template from your hard drive.

## Load from predefined templates

If you've chosen an embedded template you must specify the name of this template. The name is the name displayed in the drop-box without spaces!

{% highlight xml%}
template load predefined 'ExistsDimension';
{% endhighlight %}

##Load from a file

If you've chosen the option *file* then you need to specify the path to access it.

{% highlight xml%}
template load file 'myTemplate.nbitt';
{% endhighlight %}

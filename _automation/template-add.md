---
layout: automation
title: Add template
prev_section: template-load
next_section: settings-load
permalink: /automation/template-add/
---
This command lets you load a template without unloading the previously loaded templates. Using this command you'll have several templates. This command has the same options than *template load* and can load *embedded* or *file* templates. For more info about these options check the page about [load temmplate](/automation/template-load)

{% highlight %}
template add embedded 'ExistsDimension';
{% endhighlight %}

or

{% highlight %}
template add file 'myTemplate.nbitt';
{% endhighlight %}
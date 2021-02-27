---
layout: automation
title: Set csv profile
prev_section: settings-load
next_section: variable-include
permalink: /automation/settings-csv-profile/
---
For the definition of the [csv profile](../../docs/settings-csv-profile), you can specify the different elements of this csv profile one by one. If not specified the default value are applied. The following parameters are configurable *field-separator*, *record-separator*, *text-qualifier*, *first-row-header*, *empty-cell*, *missing-cell*.

Example:
{% highlight xml%}
csv-profile set field-separator to '#';
csv-profile set record-separator to '%';
csv-profile set text-qualifier to '#';
csv-profile set first-row-header to true;
csv-profile set empty-cell to '...';
csv-profile set missing-cell to '?';
{% endhighlight %}

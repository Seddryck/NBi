---
layout: automation
title: Include test-cases
prev_section: suite-save
next_section: suite-addrange
permalink: /automation/suite-include/
---
The command *include* offers the opportunity to add a set of tests or settings defined in an external file.

Usually, you'll use genbiL to build large tests-suite based on csv (or queries) and templates. For a few tests, it's pointless to use the generation's automation. In some occasions, you'll want to include additional tests manually-written in the test-suite. To achieve this, use the command *include*

{% highlight xml%}
suite include file 'myfile.nbitx';
{% endhighlight %}

The command *add* is a synonym of *include*.

{% highlight xml%}
suite add file 'myfile.nbitx';
{% endhighlight %}

It's also possible to import the settings defined in an external file using the following command:

{% highlight xml%}
setting include file 'settings.nbisettings';
{% endhighlight %}

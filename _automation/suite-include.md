---
layout: automation
title: Include test-cases
prev_section: suite-save
next_section: suite-addrange
permalink: /automation/suite-include/
---
The command *include* offers the opportunity to add a set of tests defined in a *test-file*.

Usually, you'll use genbiL to build large suite based on csv or queries and templates. For a few tests, it's pointless to use this generation's automation. Anyway you probably want to include these tests in the test-suite. To achieve this, use the command *include*

{% highlight xml%}
suite include file 'myfile.nbitx';
{% endhighlight %}

The command *add* is a synonym of *include*.

{% highlight xml%}
suite add file 'myfile.nbitx';
{% endhighlight %}

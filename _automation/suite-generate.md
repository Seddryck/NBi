---
layout: automation
title: Generate
prev_section: settings-load
next_section: suite-save
permalink: /automation/suite-generate/
---
The command *generate* will generate a test for each test-case based on the loaded template. The generated tests are automatically added to the test-suite.

{% highlight xml%}
suite generate;
{% endhighlight %}

If you want to use the *grouping* option, just add it after your command.
{% highlight xml%}
suite generate grouping;
{% endhighlight %}

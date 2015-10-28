---
layout: automation
title: Add a range of test-cases
prev_section: suite-include
permalink: /automation/suite-addrange/
---
The command *addrange* offers the opportunity to add a set of tests defined in a *test-suite*.

It happens that you need to add the tests of an existing test-suite to your automatically generated tests. In this case, you need to use this command and specify the filename of the test-suite.

{% highlight xml%}
suite addrange file 'mytestsuite.nbis';
{% endhighlight %}

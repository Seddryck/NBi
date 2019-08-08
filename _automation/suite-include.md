---
layout: automation
title: Include test-cases or settings
prev_section: suite-save
permalink: /automation/suite-include/
---

## Import one or more tests from an external file

Usually, you'll use genbiL to build large tests-suite based on csv (or queries) and templates. For a few tests, it's pointless to use the generation's automation. In some occasions, you'll want to include additional tests manually-written in the test-suite.

### Append one test

The command *include* offers the opportunity to add a test defined in an external file. This test is defined as a standalone test (extension .nbitx), it means the root element is the element *test* and not *test-suite*. It's also not possible to directly play with groups or to have settings or variables in this kind of document.

{% highlight xml%}
suite include file 'myfile.nbitx';
{% endhighlight %}

The command *add* is a synonym of *include*.

{% highlight xml%}
suite add file 'myfile.nbitx';
{% endhighlight %}

It's possible to define the group in which the imported test will be added.

{% highlight xml%}
suite include file 'myfile.nbitx' into 'group|subgroup';
{% endhighlight %}

or with the command *add*

{% highlight xml%}
suite add file 'myfile.nbitx' to 'group|subgroup';
{% endhighlight %}

### Append all the tests from another test-suite

The command *addrange* offers the opportunity to add the whole set of tests defined in a standard *test-suite*.

It happens that you need to add the tests of an existing test-suite to your automatically generated tests. In this case, you need to use this command and specify the filename of the test-suite.

Note that groups are not considered when importing tests from another test-suite.

{% highlight xml%}
suite addrange file 'mytestsuite.nbits';
{% endhighlight %}

It's possible to define the group in which all the imported tests will be added.

{% highlight xml%}
suite addrange file 'mytestsuite.nbits' to 'group|subgroup';
{% endhighlight %}

## Import settings from an external file

It's also possible to import the settings defined in an external file using the following command:

{% highlight xml%}
setting include file 'settings.nbisettings';
{% endhighlight %}

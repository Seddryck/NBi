---
layout: automation
title: Generate
prev_section: variable-include
next_section: suite-save
permalink: /automation/suite-generate/
---
The command *generate* will generate a test (or a setup/cleanup), for each test-case, based on the loaded template. The generated items (tests, setups or cleanups) are automatically added to the test-suite. when the keyword ```generate``` is not followed by one of the following options: ```tests```, ```setups```, ```cleanups``` then the option *tests* is selected. Meaning that the following syntax:

{% highlight xml%}
suite generate;
{% endhighlight %}

is equivalent to:

{% highlight xml%}
suite generate tests;
{% endhighlight %}

To generate a setup, you'll have to explicitely specify the kind of items that you want to generate with:

{% highlight xml%}
suite generate setups;
{% endhighlight %}

and for cleanups:

{% highlight xml%}
suite generate cleanups;
{% endhighlight %}

### Redirection of the items to groups

To redirect the generated items to different groups, you'll have to supply an additional parameter that is the name of the group. To achieve this, you'll need to use the variant ```suite generate ... group by 'group-name'```.

If you want to have several levels in your groups, you need to use the pipe (|) to specify the full path of your group. The group *sub-group* contained in the group *primary-group* will be noted *primary-group|sub-group*. This group's name can also be dynamic and use columns from the test-cases to be generated. To achieve this, specify the group's name attribute as a template.

{% highlight xml%}
suite generate group by '$dimension$|$hierarchy$';
{% endhighlight %}

Be careful that a group or sub-group can have a maximum of one (and only one) setup and cleanup!

### Deprecated grouping option

The *grouping* option, similar to [the feature available in genbi](../generate-tests/#use-grouping-option) is deprecated and only available for *tests* (not for *setups* or *cleanups*). In place you should use the command to [group rows in an array](../rows-group/). The syntax is still supported, at this moment, but won't be for long:
{% highlight xml%}
suite generate grouping;
{% endhighlight %}

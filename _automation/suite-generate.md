---
layout: automation
title: Generate
prev_section: variable-include
next_section: suite-save
permalink: /automation/suite-generate/
---
The command *generate* will generate a test, for each test-case, based on the loaded template. The generated tests are automatically added to the test-suite.

{% highlight xml%}
suite generate;
{% endhighlight %}

To redirect the generated tests to different groups, you'll have to supply an additional parameter that is the name of the group. You'll need to use the variant ```suite generate group by 'group-name'``` to achieve this.

If you want to have several levels in your groups, you need to use the pipe (```|```) to specify the full path of your group. The group *sub-group* contained in the group *primary-group* will be noted *primary-group|sub-group*. This group's name can also be dynamic and use columns from the test-cases to be generated. To achieve this, specify the group's name attribute as a template.

{% highlight xml%}
suite generate group by '$dimension$|$hierarchy$';
{% endhighlight %}

The *grouping* option, similar to [the feature available in genbi](../generate-tests/#use-grouping-option) is deprecated. In place you should use the command to [group rows in an array](../rows-group/). The syntax is still supported at the moment but won't be for long:
{% highlight xml%}
suite generate grouping;
{% endhighlight %}

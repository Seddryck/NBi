---
layout: documentation
title: Ignore
prev_section: metadata-description
next_section: metadata-not-implemented
permalink: /docs/metadata-ignore/
---
During your test session it can be helpful to ignore a few tests. The xml element named *ignore* indicates that you don't want to run a test. The test runner (Gallio or NUnit) does not run the test or tests marked with this metadata. The progress bar will turn yellow if a test is not run and the test will be mentioned in the reports as *not run*.

**Note that even if the test is ignored the xml syntax must be respected**: if the xml syntax is not verified the test-suite will not be loaded!

To mark a test as ignored:
{% highlight xml %}
<test name="test's name" uid="0001">
    <ignore/>
    <system-under-test>
        ...
    </system-under-test>
    <assert>
       ...
    </assert>
</test>
{% endhighlight %}

You can also specify why this test should be ignored. This information will be displayed in the test report. To do this add a text to the ignore element.

{% highlight xml %}
<test name="test's name" uid="0001">
    <ignore>
       Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore
       magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo
       consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
       Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
    <ignore>
    <system-under-test>
        ...
    </system-under-test>
    <assert>
        ...
    </assert>
</test>
{% endhighlight %}

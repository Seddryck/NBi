---
layout: documentation
title: Description
prev_section: metadata-concept
next_section: metadata-ignore
permalink: /docs/metadata-description/
---
You've already a name to identify your test-cases but it's also helpful to have a whole text to explicit the reason or the justification of your test. Especially when your query is complex. This text will be displayed in the execution reports.

To specify this description is to make usage of the "description" element available in the NBi syntax.
{% highlight xml %}
<test name="test's name" uid="0001">
    <description>
       Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore
       magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo
       consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
       Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
    </description>
    <system-under-test>
        ...
    </system-under-test>
    <assert>
       ...
    </assert>
</test>
{% endhighlight %}

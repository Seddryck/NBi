---
layout: documentation
title: Not implemented
prev_section: metadata-ignore
next_section: metadata-edition
permalink: /docs/metadata-not-implemented/
---
During your tests' development, it can be helpful to partially develop a test. In this case the resulting test is not syntactically valid and shouldn't be parsed. To avoid to comment it (and don't display it in the test-suite), you can mark it as *not-implemented*. The test will be listed in the test-suite but will be ignored during execution. The difference with [ignore](../metadata-ignore) is that an ignore test must be syntactically correct, it's not the case for *not-implemented*.

{% highlight xml %}
<test name="test's name" uid="0001">
    <not-implemnted>Put the reason to not finish implementation here</not-implemented>
    <some-invalid-tags/>
    <system-under-test/>
    <some-invalid-tags/>
</test>
{% endhighlight %}

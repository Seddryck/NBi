---
layout: documentation
title: Define a timeout
prev_section: query-performance
next_section: query-parameters
permalink: /docs/query-timeout/
---
It could be useful to define a timeout at the query level to avoid that bad queries run during hours and freeze your test-suite. This feature is supported by NBi with the attribute *timeout-milliSeconds* at the level of a query. If no value is specified then the query will throw a timeout after 30 seconds.

{% highlight xml %}
<execution>
    <query timeout-milliSeconds="60000">
        select * from myTable
    </query>
</execution>
{% endhighlight %}

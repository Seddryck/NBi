---
title: Define a timeout
prev_section: query-performance
next_section: query-parameters
permalink: /docs/query-timeout/
parent: "Define a query"
grand_parent: Documentation
nav_order: 1
---
It could be useful to define a timeout at the query level to avoid that bad queries run during hours and freeze your test-suite. This feature is supported by NBi with the attribute *timeout-milliSeconds* at the level of a query. If no value is specified then the query will run until it returns.

{% highlight xml %}
<result-set>
  <query timeout-milliSeconds="60000">
    select * from myTable
  </query>
</result-set>
{% endhighlight %}

---
layout: documentation
title: Superset of/subset of
prev_section: compare-single-row
next_section: connection-configuration
permalink: /docs/compare-superset-subset/
---

In some cases, the result-set must return additional (or less) rows than the expected result. During the comparison you don't want to fail the tests because of unexpected rows or missing rows. This feature has been added in v1.16

For these cases, NBi will let you define the assertions in the *superset-of* and *subset-of*

{% highlight xml %}
<superset-of>
  <query>select * from myTable</query>
</superset-of>
{% endhighlight %}
and 
{% highlight xml %}
<subset-of>
  <query>select * from myTable</query>
</subset-of>
{% endhighlight %}

All the features to define the type and role of columns are supported by these two assertions.
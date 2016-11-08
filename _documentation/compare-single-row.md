---
layout: documentation
title: Single row
prev_section: 
next_section: 
permalink: /docs/compare-single-row/
---

In some cases, the result-set must return a single row (or a single value). In these cases, most of the time it's not possible (and needed) to define a key. 

For these cases, NBi will let you define an option in the *equalTo* assertion through the attribute *behavior* and the value *single-row*.
If you apply this setting, all the columns will be considered as values and the other options such as *keys* or *values* will not be considered.

{% highlight xml %}
<equalTo behavior="single-row">
  <query>select max(value), min(value) from myTable</query>
</equalTo>
{% endhighlight %}
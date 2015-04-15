---
layout: documentation
title: Etl's performance
prev_section: etl-successful
next_section: etl-side-effects
permalink: /docs/etl-performance/
---
More information on how to define an etl in a system-under-test can be found [there](../etl-define).

It’s possible to test the performance (time to execute) of an etl by providing an time frame for the execution of the run. If the run is slower than expected the test will fail, else it will succeed. Note that the effective result of the execution is not considered during this test. So, if your package is failing after 1 second and your maximum was defined to 5 seconds: your test will succeed.

The xml element to define this kind of assertion is *fasterThan* and the time frame must be defined by the attribute *max-time-milliSeconds*. This value is expressed in milliseconds.

A full sample would be:
{% highlight xml %}
<test name="Etl is faster than 10 seconds" uid="0002">
  <system-under-test>
    <execution>
      <etl path="Etl\" name="Sample.dtsx"/>
    </execution>
  </system-under-test>
  <assert>
    <fasterThan max-time-milliSeconds="10000"/>
  </assert>
</test>
{% endhighlight %}

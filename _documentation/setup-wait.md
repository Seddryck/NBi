---
layout: documentation
title: Wait
prev_section: setup-file-manipulations
next_section: setup-condition
permalink: /docs/setup-wait/
---

This command offers the opportunity to ask your test to make a pause and wait for a delay. This delay is specified in milliseconds.

{% highlight xml %}
<setup>
  <wait
    milliseconds="1000"
  />
</setup>
{% endhighlight %}

An alternative, introduced in the version 1.13, is to wait until a connection is available. You can also define (in milliseconds) a timeout to establish this connection. If the connection can't be established before this timeout, the test will not be executed and will report a failure.

{% highlight xml %}
<wait-connection
   connectionString="@PowerBI"
   max-timeout="100000"
/>
{% endhighlight %}

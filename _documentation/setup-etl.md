---
layout: documentation
title: Run an Etl
prev_section: setup-exe-or-batch
next_section: setup-windows-services
permalink: /docs/setup-etl/
---
**Important note:** This command is executed on the server running the test-suite (not on a remote server).

* *etl-run*: this command let you start a new run for your Etl

The syntax to define an etl is defined at the page [define an Etl](../Etl-define).

Sample:
{% highlight xml %}
<test name="...">
  <setup>
    <etl-run name="Sample.dtsx" path="Etl\">
      <parameter name="DataToLoadPath">C:\data.csv</parameter>
    </etl-run>
  </setup>
…
</test>
{% endhighlight %}

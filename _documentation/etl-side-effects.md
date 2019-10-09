---
layout: documentation
title: Etl's side effects
prev_section: etl-performance
next_section: model-objects
permalink: /docs/etl-side-effects/
---
The first role of an Etl is often to perform modifications on objects (database content, create, move or delete files, …). NBi is supporting a way to test these state's updates after the run of a given etl. The process to achieve this is firstly to run the etl during the setup phase, then define your system-under-test as the impacted object by your etl (by example, through a query to a table) and finally assert the correct state of this object (the content of the result-set returned by your query).

In the sample bellow the etl will read the content of a file and populate a table. To test that the file has been correctly loaded, we’ll execute a select on the corresponding table and compare with the content of this file.

Define the setup
----------------
In the xml element named [*setup*](../setup-etl) you’ll need to define the etl to run. The syntax is exactly the same than for [etl defined in a system-under-test](../etl-define).
{% highlight xml %}
<test name="Etl in setup" uid="0003">
  <setup>
    <etl-run name="Sample.dtsx" path="Etl\">
      <parameter name="DataToLoadPath">C:\data.csv</parameter>
    </etl-run>
  </setup>
…
</test>
{% endhighlight %}

Next, you’ll need to define your query on a table as a system-under-test
{% highlight xml %}
<system-under-test>
  <result-set>
    <query>
      select CurrencyCode, Name from [Sales].[Currency]
    </query>
  </result-set>
</system-under-test>
{% endhighlight %}

And finally, you need to define your expected result-set:
{% highlight xml %}
<assert>
  <equal-to keys="first">
    <column index="1" type="text" role="value"/>
    <result-set file="C:\result.csv"/>
  </equal-to>
</assert>
{% endhighlight %}

The test will execute your etl, then execute the query and compare its result-set to the expected result-set defined in the assertion.

The whole sample would be:
{% highlight xml %}
<test name="Etl in setup" uid="0003">
  <setup>
    <etl-run name="Sample.dtsx" path="Etl\">
      <parameter name="DataToLoadPath">C:\data.csv</parameter>
    </etl-run>
  </setup>
  <system-under-test>
    <result-set>
      <query>
        select CurrencyCode, Name from [Sales].[Currency]
      </query>
    </result-set>
  </system-under-test>
  <assert>
    <equal-to keys="first">
      <column index="1" type="text" role="value"/>
      <result-set file="C:\data.csv"/>
    </equal-to>
  </assert>
</test>
{% endhighlight %}

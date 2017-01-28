---
layout: documentation
title: Successful etl
prev_section: etl-parameters-variables
next_section: etl-performance
permalink: /docs/etl-successful/
---
More information on how to define an etl in a system-under-test can be found [there](../etl-define).

The process for testing that an etl run successfully (no failure during the execution, the result of the execution is a success) is really straightforward: you run your etl and check the state at the end of the execution. If the package returns a valid status then the test is successful else it fails. To achieve this, in addition of the etl defined in the system-under-test you will need to define an xml element named *successful* within the assertion.

A full sample would be:
{% highlight xml %}
<test name="Etl is successful" uid="0001">
  <system-under-test>
    <execution>
      <etl path="Etl\" name="Sample.dtsx">
        <parameter name="DestinationPath">C:\toto.txt</parameter>
      </etl>
    </execution>
  </system-under-test>
  <assert>
    <successful/>
  </assert>
</test>
{% endhighlight %}

To assert that a run is not successful just add a **not** attribute to the **successful** element.

{% highlight xml %}
<test name="Etl is successful" uid="0001">
  <system-under-test>
    ...
  </system-under-test>
  <assert>
    <successful not="true"/>
  </assert>
</test>
{% endhighlight %}

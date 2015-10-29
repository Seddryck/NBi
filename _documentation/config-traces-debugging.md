---
layout: documentation
title: Traces for debugging
prev_section: config-dtd-processing
next_section: metadata-concept
permalink: /docs/config-traces-debugging/
---
If you've problems with one of your test it could be helpful to activate traces of NBi to debug this test. To enable the trace and display it in the output tab (with NUnit GUI), go to "Tools > Settings". In the pop-up, select the tree's element ,named "Gui >  Text Output" and check the flag "Trace Output".

If you execute your test-suite you'll receive a few information about the test execution in the "output tab". These information are info (high level) about the location of your test or the query executed on the servers. If you want to receive more information about your test execution, you'll need to activate this in your *config* file by the means of the standard .Net trace switch. Open the *config* file of your test suite and add the following xml fragment bellow the section config of NBi:

{% highlight xml %}
<system.diagnostics>
  <switches>
    <add name="NBi" value="4" />
  </switches>
</system.diagnostics>
{% endhighlight %}

The value "4", meaning "Verbose", will display all information raised by NBi. This can slow down considerably the execution of your test suite, be careful when using this configuration. This should only be in use during the debugging of your test-suite, change it back to a higher level as soon as your problem is fixed.

If you use "3", meaning information, you'll receive only high level info and the impact on the performance will be minimal. Values: "1" (Error) and "2" (Warning) will only display messages if internal errors occur in NBi. If this xml fragment is not present in your config file, NBi you'll use the value "3" (Information) by default.

Full config file example:
{% highlight xml %}
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="nbi" type="NBi.NUnit.Runtime.NBiSection, NBi.NUnit.Runtime"/>
  </configSections>
  <nbi testSuite="SubDirectory\myTestSuite.nbits"/>
  <system.diagnostics>
    <switches>
      <add name="NBi" value="3" />
    </switches>
  </system.diagnostics>
</configuration>
{% endhighlight %}

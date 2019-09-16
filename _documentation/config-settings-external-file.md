---
layout: documentation
title: Settings in external file
prev_section: config-defaults-references
next_section: config-profile-csv
permalink: /docs/config-settings-external-file/
---
You can define in the config file the xml attribute *settings* to express that the settings are defined in an external file and not in the test-suite. The value of this xml attribute *settings* matches with the name of the external file containing your xml fragment with the *settings* element of your test-suite.

It could be useful to develop several *configs* and *settings* file to manage different environments where you're executing your tests.

In the example, here under, the settings are defined in an external file named "MySettings.nbiset".

{% highlight xml %}
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="nbi" type="NBi.NUnit.Runtime.NBiSection, NBi.NUnit.Runtime"/>
  </configSections>
  <nbi
    testSuite="MyFirstTestSuite\MyFirstTestSuite.nbits"
    settings="MyFirstTestSuite\MySettings.nbiset"
  />
</configuration>
{% endhighlight %}

This file is an example of an external file containing the xml element *settings*.

{% highlight xml %}
<?xml version="1.0" encoding="utf-8"?>
<settings xmlns="http://NBi/TestSuite">
  <default apply-to="system-under-test">
    <connection-string>My Sut Default Connection String</connection-string>
  </default>
  <reference name="MyReference">
    <connection-string>My Reference Connection String</connection-string>
  </reference>
</settings>
{% endhighlight %}

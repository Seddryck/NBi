---
layout: documentation
title: Configuration file
prev_section: installation-test-suite
next_section: installation-nunit-project
permalink: /docs/installation-config/
---
The configuration file is a key element in your test environment. **This file defines the path to your test-suite**. This done by specifying the path to the test-suite in the attribute *testSuite* of xml element nbi.

Note also that this config file must contain the xml element *section* and define the type *nbi* as specified here under.

{% highlight xml %}
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
 <configSections>
  <section name="nbi" type="NBi.NUnit.Runtime.NBiSection, NBi.NUnit.Runtime"/>
 </configSections>
 <nbi testSuite="MyTestProject.nbits"/>
</configuration>
{% endhighlight %}

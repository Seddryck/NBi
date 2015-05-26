---
layout: documentation
title: Configuration file
prev_section: installation-nunit-project
next_section: run-with-nunit
permalink: /docs/installation-config/
---
The configuration file is a key element in your test environment. **This file defines the path to your test-suite**. This done by specifying the path to the test-suite in the attribute *testSuite* of xml element nbi. Keep in mind that this path is relative to the *AppBase* path defined in the nunit file.

Note also that this config file must contain the xml element *section* and define the type *nbi* as specified here under.

{% highlight xml %}
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
 <configSections>
  <section name="nbi" type="NBi.NUnit.Runtime.NBiSection, NBi.NUnit.Runtime"/>
 </configSections>
 <nbi testSuite="MyTestProject\MyTestProject.nbits"/>
</configuration>
{% endhighlight %}

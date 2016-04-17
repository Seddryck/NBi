---
layout: documentation
title: Power BI Desktop
prev_section: connection-providers
next_section: connection-roles
permalink: /docs/connection-powerbi-desktop/
---
To test a Power BI Desktop solution, you'll need to provide a connection-string redirecting to your Power BI Desktop solution. This connection-string must respect the following format: ```PBIX = Name of my Power BI Desktop solution```. The name of your solution should be the filename without the extension ".pbix".

{% highlight xml %}
<testSuite name="The Query TestSuite" xmlns="http://NBi/TestSuite">
	<settings>
		<default apply-to="system-under-test">
			<connectionString>PBIX = My Solution</connectionString>
		</default>
	</settings>
	<test name="...">
		...
	</test>
</testSuite>
{% endhighlight %}

**Important note:** In order to test a Power BI Desktop solution, the corresponding *pbix* file must be running/open during the execution of a test-suite.

You can open you pbix file by the means of the [setup](../setup-cleanup) feature.

{% highlight xml %}
<setup>
  <tasks run-once="true" parallel="false">
    <exe-kill
      name="PBIDesktop"
    />
    <exe-run
      name="Sales Analysis.pbix"
      path="..\PowerBiDesktop\"
    />
    <connection-wait
      connectionString="PBIX = Sales Analysis"
      timeout-milliseconds ="60000"
    />
  </tasks>
</setup>
{% endhighlight %}

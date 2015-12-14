---
layout: documentation
title: Power BI Desktop
prev_section: connection-providers
next_section: connection-roles
permalink: /docs/connection-powerbi-desktop/
---
To test a Power BI Desktop solution, you'll need to provide a connection-string redirecting to your Power BI Desktop solution. This connection-string must respect the following format: ```PBIX = Name of my Power BI Desktop solution```. The name of your solution should be the filename without the extension ".pbix".

**Important note: In order to test this Power BI Desktop solution, the pbix file must be running during the execution of a test-suite.**

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

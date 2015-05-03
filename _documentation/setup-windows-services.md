---
layout: documentation
title: Windows services
prev_section: setup-file-manipulations
next_section: setup-etl
permalink: /docs/setup-windows-services/
---
These commands will be executed on the server running the test-suite (not on a remote server). They're able to start or stop a Windows Service. For both of the commands, if the service is already started/stopped, this command will have no influence (and will not failed).

* *service-start*: this command starts a windows service.
* *service-stop*: this command stops a windows service.

For both commands, the xml attribute *name* specify the name of the windows service to start or stop. The attribute *timeout-milliseconds* is setting a maximum time to elapse before the command has successfuly completed. If the timeout is reached, the command will return a failure and the test will not be executed. This attribute is optional but a **default value of 5 seconds** is applied in case this attribute is not specified.

{% highlight xml %}
<setup>
	<service-start name="MyService"/>
</setup>
...
<cleanup>
	<service-stop name="MyService" timeout-milliseconds="15000"/>
</cleanup>
{% endhighlight %}

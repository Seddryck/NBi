---
layout: documentation
title: Conditions for execution
prev_section: setup-file-manipulations
next_section: config-connection-strings
permalink: /docs/setup-condition/
---
In addition to the [setup and cleanup decorations](../setup-cleanup) a test can also be preceded by a list of *conditions*. If one the conditions is not validated, the test will be *ignored (and not failed)*. An appropriate message will be inserted into the ignore message to explain why the test has been ignored by the framework.

This may be really interesting if you don't want that all your tests related to a specific windows service (SSRS or SSIS) fail, just because this service has not been started in the server executing your test-suite.

Note that the conditions are executed *before* the list of commands registered in the *setup*! In consequence, it's not possible to check that a service is running in the *condition* and stop it during the *setup*. If the service is not started when performing the check over the condition, the test will stop and be reported as *ignored* (and not failed).

{% highlight xml %}
<test>
	<condition>
		...
	</condition>
	<setup>
		...
	</setup>
	<system-under-test>
		...
	</system-under-test>
</test>
{% endhighlight %}

## Available predicates

### Windows Service

The following predicate is defined:

* service-running: this service will validate if the service is effectively running.

If the service is not in the expected state, NBi will wait maximum the time set in the attribute named *timeout-milliseconds* to ensure that the service is not changing its state before reporting the test as *ignored*. If this timeout is not defined, a default value of 5 seconds will be used.

{% highlight xml %}
<condition>
	<service-running name="MyService"/>
	<service-running name="MyService2" timeout-milliseconds="1000"/>
</condition>
{% endhighlight %}

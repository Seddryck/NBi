---
layout: documentation
title: Conditions for execution
prev_section: setup-wait
next_section: config-settings-external-file
permalink: /docs/setup-condition/
---
In addition to the [setup and cleanup decorations](../setup-cleanup) a test can also be preceded by a list of *conditions*. If one the conditions is not validated, the test will be *ignored (and not failed)*. An appropriate message will be inserted into the ignore message to explain why the test has been ignored by the framework.

This may be really interesting if you don't want that all your tests related to a specific windows service (SSRS or SSIS) fail, just because this service has not been started in the server executing your test-suite.

Note that the conditions are executed *before* the list of commands registered in the *setup*! In consequence, if you're checking that a service is running and it's not the case, the test will stop and be reported as *ignored* (and not failed). It's not possible to continue and ask to start the service in the setup. If your intempt is to be sure that a service is running and if it's not the case then start it, you can skip the condition and just add a setp *service-start* in the setup.

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

* service-running: this service will validate if the service is effectively running. This returns *false* when this service is not existing.

If the service is not in the expected state, NBi will wait maximum the time set in the attribute named *timeout-milliseconds* to ensure that the service is not changing its state before reporting the test as *ignored*. If this timeout is not defined, a default value of 5 seconds will be used.

{% highlight xml %}
<condition>
	<service-running name="MyService"/>
	<service-running name="MyService2" timeout-milliseconds="1000"/>
</condition>
{% endhighlight %}

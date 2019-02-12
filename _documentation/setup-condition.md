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

## Custom conditions

It's possible to develop your own conditions in C#. To achieve this, you'll need to create a C# library project and compile it as a library. One of the class of your project should implement the interface *ICustomCondition* from the namespace *NBi.Extensibility.Condition* and available in the nuget package *NBi.Extensibility*.

This interface declares one method *Execute()* returning the result of the condition. The property *IsValid* is a boolean describing if the condition is successful (*true*) or not (*false*). The message is a *string* that will be displayed to explain that the condition is not met.

The following custom condition, returns true if the day of month is even, else returns false:

{% highlight csharp %}
public class CustomConditionBasedOnDay : ICustomCondition
{
	public CustomConditionResult Execute() 
		=> new CustomConditionResult(Date.Now.Day % 2 == 0, "Oh man, retry tomorrow!");
}
{% endhighlight %}

Once the custom condition is coded and built, you need to deploy the dll somewhere that your test-suite will be able to reach. In your est-suite your must reference the assembly by the mean of the xml attribute *assembly-path* and then specify the type implementing the *ICustomCondition* interface. Several types implementing this interface can sharethe same dll. 

A set of parameters can also be defined. They will used during the construction of the object and the name of the parameters must match with the name of the constructor's attribute.

{% highlight xml %}
<condition>
  <custom assembly-path="myAssembly.dll" type="myType">
    <parameter name="firstParam">2012-10-10</parameter>
    <parameter name="secondParam">102</parameter>
  </custom>
</condition>
{% endhighlight %}
---
layout: automation
title: Condition in a template
prev_section: generate-tests
next_section: format-variable
permalink: /automation/condition-template/
---
It can be useful to specify that a few xml fragments should be rendered in some specific cases and not in other cases. To achieve this, NBi uses the syntax and features provided by the external library [StringTemplate and detailed there](https://theantlrguy.atlassian.net/wiki/display/ST4/Templates#Templates-conditionals).

NBi will not assign a value to a variable in two cases:

* The value provided for this variable in the test-case is "(none)"
* The value provided as a length of 0 characters (an empty string).

## Conditional rendering of an "ignore" element

Suppose that in your test-cases you've a column named *ignoreReason* and you want to add an xml element *<ignore>* only if an *ignoreReason* has been provided.

Your test-cases are defined in a CSV file as:

{% highlight xml %}
perspective;dimension;ignoreReason
Internet Sales;Customer;(none)
Internet Sales;Geography;To be released in a next version
{% endhighlight %}

and you're expecting the following result after the execution of the generation (*note that the ignore tags are available in the second test but not on the first*!):

{% highlight xml %}
<test name="A dimension named 'Customer' exists in perspective 'Internet Sales'.">
	<system-under-test>
		<structure>
			<dimension caption="Customer" perspective="Internet Sales"/>
		</structure>
	</system-under-test>
	<assert>
		<exists/>
	</assert>
</test>
<test name="A dimension named 'Geography' exists in perspective 'Internet Sales'.">
	<ignore>To be released in a next version</ignore>
	<system-under-test>
		<structure>
			<dimension caption="Geography" perspective="Internet Sales"/>
		</structure>
	</system-under-test>
	<assert>
		<exists/>
	</assert>
</test>
{% endhighlight %}

To achieve this, you'll need to make usage of the *if* statement provided by StringTemplate. To specify that a part of a template must be rendered only if a value is assigned to a variable simply write:

{% highlight xml %}
$if(ignoreReason)$<ignore>$ignoreReason$</ignore>$endif$
{% endhighlight %}

Take care of following details:

* if and endif are surrounded by $ symbols
* in the if statement, the name of the variable is not surrounded by $ symbols
* there is no comparison in the the if statement, just the name of the variable. The if statement check if the provided variable is assigned or not.

The following template should have been provided to successfully build the test-suite described above:

{% highlight xml %}
<test name="A dimension named '$dimension$' exists in perspective '$perspective$'.">
	$if(ignoreReason)$<ignore>$ignoreReason$</ignore>$endif$
	<system-under-test>
		<structure>
			<dimension caption="$dimension$" perspective="$perspective$"/>
		</structure>
	</system-under-test>
	<assert>
		<exists/>
	</assert>
</test>
{% endhighlight %}

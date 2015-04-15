---
layout: documentation
title: Query's template-variables
prev_section: query-parameters
next_section: query-assembly
permalink: /docs/query-template/
---
You can use template-variables to dynamically build the statement of your SQL, MDX or DAX queries. Unlike the [parameters](../query-parameters/), the template-variable inputs are treated as portions of executable code.

When possible, we highly recommend to use the [parameters](../query-parameters/) and not the template-variables. The template-variables are more flexible to build dynamically your queries but are obfuscating your tests.

To create template-variables in a query, you identify them by putting some dollar signs ($) before and after the template-variable's name. For example, $Year$ would be a valid template-variables name.

In your test definition, in addition of the element *query*, you'll also need to describe your template-variable by defining its name and value within an element named *variable*.

{% highlight xml %}
<query>
	select * from Customer where CustomerKey $OperatorVar$ (1,2,3)
	<variable name="OperatorVar">
		not in
	</variable>
</query>
{% endhighlight %}

The query executed by NBi will be

{% highlight sql %}
select * from Customer where CustomerKey not in (1,2,3)
{% endhighlight %}

## Template-variables defined at the test-suite level

Sometimes, a few template-variables are used in more than one query and their values are constant through the test-suite. In this case, you can save time and define them at the test-suite level. This can be achieved by the usage of the element *variable* in the element *default* of the element *settings*.

Within the code snippet here under we're defining a template-variable named *Clause*. This *Clause* applies a slicer to limit the cube to the years 2010 to 2013.

{% highlight xml %}
<settings>
	<default apply-to="system-under-test">
		<variable name="Clause">
			([DimTime].[Year].[2010]:[DimTime].[Year].[2013])
		</variable>
	</default>
</settings>
{% endhighlight %}

This template-variable can be used in the following two queries without reapeating the definition of this variable in each test:

{% highlight xml %}
<test>
	...
	<query>
		select
			$Clause$ on 0,
			[Measures].[My Value] on 1
		from
			[my cube]
	</query>
	...
</test>
<test>
	...
	<query>
		select
			[Measures].[My Value] on 0
		from
			[my cube]
		where
			$Clause$
	</query>
	...
</test>
{% endhighlight %}

If a template-variable is defined at the test-suite level and at the query level for a test, the definition at the query level will be used for this test.

If a template-variable is not used within a query but is provided to this query, this template-variable is simply not used by NBi (so it's not a problem).

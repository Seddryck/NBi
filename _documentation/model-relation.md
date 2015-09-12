---
layout: documentation
title: Relation between objects
prev_section: model-collection-assertion
next_section: model-data-type
permalink: /docs/model-relation/
---
The concept of *relations between objects* has only been implemented for SSAS (multidimensional) at the moment. It lets you validate that a dimension is linked to a measure-group or reciprocally that a measure-group is linked to a dimension. In the future we plan to do the same for tables in a SQL model (foreign keys).

For this kind of test, NBi proposes an assertion named *linkedTo*. To use it you must define a *system-under-test* with a structure of type *dimension* or *measure-group*.

{% highlight xml %}
<system-under-test>
	<structure>
		<dimension caption="Customer" perspective="Adventure Works"/>
	</structure>
</system-under-test>
{% endhighlight %}

When done, you’ll have to define your assertion as an xml elemened named *linkedTo* and specify in the xml attribute *caption* the measure-group or dimension that you're expecting linked to the previous element. Note that inside this *linkedTo* element you don’t need to specify the perspective of the *dimension* or *measure-group*, this information will be inferred from the *system-under-test*.

{% highlight xml %}
<assert>
	<linkedTo>
		<measure-group caption="Internet Sales"/>
	</linkedTo>
</assert>
{% endhighlight %}

Here under, you’ll find a sample for both elements (dimension linked to measure-group and measure-group linked to dimension).

{% highlight xml %}
<?xml version="1.0" encoding="utf-8" ?>
<testSuite name="Acceptance Testing: members ordering" xmlns="http://NBi/TestSuite">
	<settings>
		<default apply-to="system-under-test">
			<connectionString>Provider=MSOLAP.4;Data Source=(local);Initial Catalog='Adventure Works DW 2008';localeidentifier=1033</connectionString>
		</default>
	</settings>
	<test name="Dimension 'Customer' is linked to measure-group  'Internet Sales' throw perspective 'Adventure Works'" uid="0001">
		<system-under-test>
			<structure>
				<dimension caption="Customer" perspective="Adventure Works"/>
			</structure>
		</system-under-test>
		<assert>
			<linkedTo>
				<measure-group caption="Internet Sales"/>
			</linkedTo>
		</assert>
	</test>
	<test name="Measure-group 'Internet Sales' is linked to dimension 'Internet Sales' throw perspective 'Adventure Works'" uid="0001">
		<system-under-test>
			<structure>
				<measure-group caption="Internet Sales" perspective="Adventure Works"/>
			</structure>
		</system-under-test>
		<assert>
			<linkedTo>
				<dimension caption="Customer"/>
			</linkedTo>
		</assert>
	</test>
</testSuite>
{% endhighlight %}

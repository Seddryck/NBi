---
layout: documentation
title: Existence of an object
prev_section: model-objects
next_section: model-collection-assertion
permalink: /docs/model-exists/
---
The assertion consists in a validation that the object specified in the *system-under-test* exists (by "exists", we mean "visible for the end-user browsing your cube or database").

## Assertion
Once we have [defined the object that will be validated](model-objects), we have to define the assertion of type *exists*:
{% highlight xml %}
<test>
    <assert>
        <exists/>
    </assert>
</test>
{% endhighlight %}

The whole test will look like:
{% highlight xml %}
<test>
    <system-under-test>
        <structure>
    	    <measure caption="MyMeasure" measure-group="MyMeasureGroup" perspective="MyPerspective"
        	connectionString="Provider=MSOLAP.4;Data Source=MyServer;Integrated Security=SSPI;Initial Catalog=MyCube;"/>
	</structure>
    </system-under-test>
    <assert>
        <exists />
    </assert>
</test>
{% endhighlight %}

## Display-folder for measures and hierarchies
It's not possible to check that a display-folder for measures or hierarchies exists. But anyway you can validate that a measure (or hierarchy) is effectively available in the expected display-folder. Using the attribute display-folder.
{% highlight xml %}
<test>
    <system-under-test>
        <structure>
    	    <measure caption="MyMeasure"
		display-folder="MyDisplayFolder\SubFolder"
		measure-group="MyMeasureGroup" perspective="MyPerspective"
        	connectionString="Provider=MSOLAP.4;Data Source=MyServer;Integrated Security=SSPI;Initial Catalog=MyCube;"/>
	</structure>
    </system-under-test>
    <assert>
        <exists />
    </assert>
</test>
{% endhighlight %}
If you want to specify that the measure (or hierarchy) should be in the root of the measure-group simply apply an empty value for the display-folder.
{% highlight xml %}
<test>
    <system-under-test>
        <structure>
    	    <measure caption="MyMeasure"
		display-folder=""
		measure-group="MyMeasureGroup" perspective="MyPerspective"
        	connectionString="Provider=MSOLAP.4;Data Source=MyServer;Integrated Security=SSPI;Initial Catalog=MyCube;"/>
	</structure>
    </system-under-test>
    <assert>
        <exists />
    </assert>
</test>
{% endhighlight %}
If you don't specify the attribute display-folder then NBi will not take into account the display-folder when looking for your element. It means that if your measure (or attribute) is specified in the root or in a display-folder will not influence the result of the existence test.

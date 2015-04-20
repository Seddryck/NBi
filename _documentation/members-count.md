---
layout: documentation
title: Members' count'
prev_section: members
next_section: members-ordering
permalink: /docs/members-count/
---
If you're not familiar with the way to specify a test on members of a hierarchy (or level), read first the page about [members].

## Assertion
The assertion consists in a validation of the members' count, in the hierarchy or level specified. To achieve this, you need to specify an assertion of type *count*, as described here under.
{% highlight xml %}
<test>
    <assert>
        <count/>
    </assert>
</test>
{% endhighlight %}
Then you need to specify one of the following option:
* exactly
* more-than
* less-than

It's also possible to combine more-than and less-than, as shown here under.

{% highlight xml %}
<count more-than="5" less-than="10"/>
{% endhighlight %}

If you're validating that the hierarchy named "MyHierarchy" has more than 15 members:
{% highlight xml %}
<test>
    <system-under-test>
	    <members>
		    <hierarchy caption="State-Province" dimension="Customer" perspective="Adventure Works"
			     connectionString="Provider=MSOLAP.4;Data Source=MyServer;Integrated Security=SSPI;Initial Catalog=MyCube;"
		     />
	    </members>
    </system-under-test>
    <assert>
        <count more-than="15"/>
    </assert>
</test>
{% endhighlight %}

---
layout: documentation
title: Members
prev_section: model-relation
next_section: members-count
permalink: /docs/members/
---
Once you've validated that a hierarchy (or level, or set) effectively exists, it's time to validate the content (members) of this hierarchy (or level, or set).

## System-under-test
To achieve this, you need to stipulate where you'll be looking for (in other words, on which hierarchy, level or set you'll perform your assertion). This is done by creating an xml element named *members* under the node *system-under-test*.
{% highlight xml %}
<test>
    <system-under-test>
        <members/>
    </system-under-test>
</test>
{% endhighlight %}

In the node *members*, you'll have to specify on which object your test will apply. This is done by providing an xml element named *hierarchy* or *level*. By example, here under, we're looking for a member in the *level* 'State-Province' of the *hierarchy* 'Customer Geography' in dimension 'Customer'.

[image:Dimension Customer.png]

Note: For hierarchy without level, such as *customer* in the sample above, the difference between a test executed on the hierarchy and a test on the level will be the member *All*. Indeed, this member is available in the *hierarchy* but not in the *level*.

{% highlight xml %}
<system-under-test>
	<members>
		<level
      caption="State-Province"
      hierarchy="Customer Geography"
      dimension="Customer"
      perspective="Adventure Works"
    />
	</members>
</system-under-test>
{% endhighlight %}

If you want to perform a test on all members of this hierarchy, including *coutries, states, cities, postal code and customers*, you can position your test on the hierarchy named *Customer Geography*.
{% highlight xml %}
<system-under-test>
	<members>
		<hierarchy
      caption="Customer Geography"
      dimension="Customer"
      perspective="Adventure Works"
    />
	</members>
</system-under-test>
{% endhighlight %}

Don't forget to specify the xml attribute *connectionString* to reach your cube. if you want you can also make usage of the [a default or reference connection-string](defaults-references).

{% highlight xml %}
<system-under-test>
	<members>
		<hierarchy
      caption="Customer"
      dimension="Customer"
      perspective="Adventure Works"
			connectionString="Provider=MSOLAP.4;Data Source=MyServer;
        Integrated Security=SSPI;Initial Catalog=MyCube;"
		/>
	</members>
</system-under-test>
{% endhighlight %}

## Exclude some members
When developing a cube, you're often in front of specific requirements about some members, they just don't respect the general rules. Typically common rules don't apply to members *Unknown*, *Not Applicable* etc. If you have a requirement such as *"members must be ordered alphabetically but the member 'Unknown' must be at the bottom of the list"*, it's really helpful to be able to exclude some members from your test. The way to achieve this with NBi is to add an xml element named 'exclude' at the same level of your xml element hierarchy or level.

{% highlight xml %}
<system-under-test>
	<members>
		<hierarchy .../>
    <exclude />
	</members>
</system-under-test>
{% endhighlight %}

If you know the unique-name of the member you can directly reference the item to exclude by specifying its unique-name. You can add more than one member to exclude.

{% highlight xml %}
<system-under-test>
	<members>
		<level
      caption="Country"
      hierarchy="Customer Geography"
      dimension="Customer"
      perspective="Adventure Works"
    />
    <exclude>
       <item>France</item>
       <item>Germany</item>
    </exclude>
	</members>
</system-under-test>
{% endhighlight %}

But sometimes you don't know the unique-name of the member to exclude, or you want to remove a list of members with the same caption. In this case you can make usage of the xml element named *items* and the pattern *exact*
{% highlight xml %}
<system-under-test>
	<members>
    <level
      caption="Country"
      hierarchy="Customer Geography"
      dimension="Customer"
      perspective="Adventure Works"
    />
    <exclude>
       <items pattern="exact">United Kingdom</item>
    </exclude>
	</members>
</system-under-test>
{% endhighlight %}

If you want to remove the members starting, ending or containing a specific pattern, you can also do this with the help of the xml element named *items* choosing the correct pattern (start-with, end-with or contain).
{% highlight xml %}
<system-under-test>
	<members>
		<hierarchy
      caption="Country"
      hierarchy="Customer Geography"
      dimension="Customer"
      perspective="Adventure Works"
    />
    <exclude>
       <items pattern="start-with">John</item>
       <items pattern="end-with">iams</item>
       <items pattern="contain">hson</item>
    </exclude>
	</members>
</system-under-test>
{% endhighlight %}

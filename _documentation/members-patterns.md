---
layout: documentation
title: Members' patterns
prev_section: members-source
next_section: setup-cleanup
permalink: /docs/members-patterns/
---
It's possible to validate if members are matching a pattern defined by a regular expression (regex). To achieve this, you have to specify your *system-under-test* as *members*: if you're not familiar with the way to specify a test on members of a hierarchy, level or set, read first the page about [members].

## Patterns
The xml element named *matchPattern* is needed for this kind of assertions

{% highlight xml %}
<test>
  <assert>
    <matchPattern />
  </assert>
</test>
{% endhighlight %}

The pattern must be expressed through a regular expression (regex). Each member's caption will be compared to the regex and if at least one of the members' caption doesn't validate the pattern then the test will fail.

{% highlight xml %}
<assert>
	<matchPattern>
		<regex>^\s*[a-zA-Z,\s]+\s*$</regex>
	</matchPattern>
</assert>
{% endhighlight %}

If you're validating that all the members of level *Departments* in the hierarchy named *Departments* of dimension *Department* have a literal name, you can use the following test:
{% highlight xml %}
<test name="All departments have a correct format" uid="0001">
  <system-under-test>
    <members children-of="Corporate">
      <level caption="Departments" hierarchy="Departments" 
             dimension="Department" perspective="Adventure Works"/>
    </members>
  </system-under-test>
  <assert>
    <matchPattern>
      <regex>^\s*[a-zA-Z,\s]+\s*$</regex>
    </matchPattern>
  </assert>
</test>
{% endhighlight %}

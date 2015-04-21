---
layout: documentation
title: Members' existence
prev_section: members-ordering
next_section: members-source
permalink: /docs/members-collection/
---
If you're not familiar with the way to specify a test on members of a hierarchy, level or set, read first the page about [members].

These assertions consist in validations that one, or more, members are effectively existing in a given hierarchy, level or set. The validation is performed on the caption of the members.

## Contain
This assertion consists in a check that one of the members in a hierarchy, level or set has a given caption. To achieve this goal, you need to specify an assertion of type *contain*:
{% highlight xml %}
<test>
    <assert>
        <contain/>
    </assert>
</test>
{% endhighlight %}

To stipulate the caption you're looking for, you'll have to fill the xml attribute *caption*.

{% highlight xml %}
<contain caption="MyMember"/>
{% endhighlight %}

If you want to perform this kind of assertion on several members, you’ll have to define an xml element named *item* for each expected member. The sample here under, validates that three members named *My first member*, *My second member* and *My third member*, effectively exist:
{% highlight xml %}
<assert>
  <contain>
    <item>My first member</item>
    <item>My second member </item>
    <item>My third member</item>
  </contain>
</assert>
{% endhighlight %}

The test will succeed only if all the members defined in your assertion are effectively in the hierarchy (or level). If at least one member doesn't exist then the test will fail.

## Subset of
This kind of assertion will validate that you haven't unexpected members. Imagine that you’ve a hierarchy *gender*. You know that the two valid choices are *male* and *female*, other values are not expected. If the list contains any other element then the test will fail. But what should happen if one of the two members is not loaded? It's not necessary a problem, it could happen that you’ve only loaded men (or women). In this case, the test should not fail. The assertion *subsetOf* is built for this case: you want to validate that the members are not outside a predefined set of values, but you don't want to validate that all of them are available.
{% highlight xml %}
    <assert>
        <subsetOf>
            <item>Male</item>
            <item>Female</item>
        </subsetOf>
    </assert>
{% endhighlight %}

This test will only succeed if all the members of your hierarchy are value provided in the list of item. If you’ve any member of your hierarchy not included in the two members provided in your assertion, the test will fail. The test will not fail if your hierarchy contain only one of the the members provided in the assertion.

## Equivalent to
In some case, you know exactly the content of your hierarchy or level. In this case, you’ll probably want to test that the whole hierarchy is correctly loaded in your cube. This can be achieved with the usage of the assertion *equivalentTo*. You'll need to provide a list of *item* corresponding to the list of expected members.
{% highlight xml %}
    <assert>
        <equivalentTo>
            <item>Male</item>
            <item>Female</item>
        </equivalentTo>
    </assert>
{% endhighlight %}
The test will only succeed if your hierarchy has exactly two members named *Male* and *Female*. If you’ve more or less or different items, this test will fail.
*Note that this test is equivalent to two assertions *contain* (one for *Male* and another for *Female*) and one assertion *subsetOf* (for *Male* and *Female*). It’s just a matter of readability versus reporting and investigation facilities.*

## Display the difference
If your test has failed, NBi will provide a list of missing and/or unexpected items according to the type of assertion performed
* For *contain*: missing items
* For *subsetOf*: unexpected items
* For *equivalentTo*: missing and unexpected items
By default, this list will be limited to 10 items maximum. If you want to change this behavior see the documentation about [failure report profile](profile-failure-report).

---
layout: documentation
title: Members' existence
prev*section: members-ordering
next*section: members-collection
permalink: /docs/members-collection/
---
If you're not familiar with the way to specify a test on members of a hierarchy, level or set, read first the page about [members].

The assertions consist in validations that one, or more, members are effectively existing in the hierarchy, level or set. The validation is performed on the caption of the members.

## Contain
The assertion consists in a check that one of the members in a hierarchy, level or set has a given caption. To achieve this goal, you need to specify an assertion of type *contain*:
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

If you want to perform the assertion on several members, you’ll have to define an xml element named *item* for each expected member. The sample here under, validates that three members named *My first member*, *My second member* and *My third member*, effectively exist:
{% highlight xml %}
<assert>
  <contain>
    <item>My first member</item>
    <item>My second member </item>
    <item>My third member</item>
  </contain>
</assert>
{% endhighlight %}

The test will succeed only if all the members defined in your assertion are effectively in the hierarchy (or level). If at least one member doesn't exist
!!! All members belong to a predefined list
Imagine that you’ve a hierarchy *gender*. You know that the only two choices are *male* and *female*, other values are not expected and should fail your test. You won't be able to assert the test with the assertions defined above.
First case, it could happen that you’ve only loaded men (or women). In this case, your assertion with members of hierarchy *gender* abou the fact this hierarchy effectively contains *female* will fail … but your data warehouse is correctly loaded! You probably want to check that members of this hierarchy are not different than *male* and *female* and not the *male* and *female* are available. The assertion subsetOf is there for this case.
With the assertion *subsetOf*, you can ensure that the members are not outside a predefined set of value, but you'll not ensure that all of them are available.
{% highlight xml %}
    <assert>
        <subsetOf>
            <item>Male</item>
            <item>Female</item>
        </subsetOf>
    </assert>
{code:xml}
This test will only succeed if all the members of your hierarchy are value provided in the list of item. If you’ve any member of your hierarchy not included in the two members provided in your assertion, the test will fail. The test will not fail if your hierarchy contain only one of the the members provided in the assertion.
!! You know exactly all the members
In some case, you know exactly the content of your hierarchy or level. In this case, you’ll probably want to test that the whole hierarchy is correctly loaded in your cube. This can be achieved with the usage of the assertion *equivalentTo*. You'll need to provide a list of *item* with the corresponding to the list of members expected.
{% highlight xml %}
    <assert>
        <equivalentTo>
            <item>Male</item>
            <item>Female</item>
        </equivalentTo>
    </assert>
{code:xml}
The test will only succeed if your hierarchy has exactly two members named *Male* and *Female*. If you’ve more or less or different items, this test will fail.
*Note that this test is equivalent to two assertions *contain* (one for *Male* and another for *Female*) and one assertion *subsetOf* (for *Male* and *Female*). It’s just a matter of readability versus reporting and investigation facilities.*
!! Dynamic list of members in your assertion
*New in v1.3*
Until now, we've learnt how to perform assertions against a static list of members. Since version 1.3, it's also possible to have a dynamic list of members retrieved from a query (Sql, Mdx or DAX).
This can be useful if you've a list of members stored in a relational database and that this list is in a constant evolution (customers, malls, ...). To achieve this, you'll need to provide a *one-column-query* in place of the list of *item*. This *one-column*query* is just a standard *query* xml element where only the first column of the resultSet will be used by NBi. You can define this xml element as:
{% highlight xml %}
    <assert>
        <equivalentTo>
            <one-column-query>
                select displayName from Customer
	    </one-column-query>
        </equivalentTo>
    </assert>
{code:xml}
The query will be executed and the first column of the resultSet will be used to build the list of expected members. Then, the assertion will perform exactly as previously defined for a static list of members.
!! Display the difference
*New in 1.3*
If your test has failed, NBi will provide you a list of missing and/or unexpected items according to the assertion performed
* For *equivalentTo*: missing and unexpected items
* For *contain*: missing items
* For *subsetOf*: unexpected items
This list will be limited to 10 items maximum.

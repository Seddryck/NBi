---
layout: documentation
title: Members' ordering
prev_section: members-count
next_section: members-collection
permalink: /docs/members-ordering/
---
If you're not familiar with the way to specify a test on members of a hierarchy (or level), read first the page about [members].
## Assertion
The assertion consists in a validation that members are displayed, on the client, in a specified order. To achieve this, you need to specify an assertion of type *order*, such as:
{% highlight xml %}
<test>
    <assert>
        <ordered/>
    </assert>
</test>
{% endhighlight %}

The assertion *order* supports different options:
* alphabetical
* chronological
* numerical
* specific

The three first options are pre-defined standard rules. *Alphabetical* is indicated for strings, *chronological* to date and time members and finally *numerical* to numeric values. The option *specific* lets you define your own rule (see below).

The chosen option must be specified with the help of an xml attribute *rule*.
{% highlight xml %}
<ordered rule="alphabetical"/>
{% endhighlight %}

**Important note**: if a member cannot be converted to a valid date or numeric, and you're applying a rule chronological or numerical, the test will **not fail**. The test will simply ignore the members that cannot be converted. This is not a bug but it's done by design. If NBi was failing a test when failing to convert a member, it'd mean that members such as "N/A" or "Unknown" in a numerical or date attribute will always break the test. It's probably not what you want.

## Specific order
For some hierarchies or levels, you'll want to display the members in a specific order because you cannot simply use the predefined rules *alphabetical, chronological or numerical*. To define the expected order, you can make usage of xml elements *rule-definition* and *item*.

{% highlight xml %}
<ordered rule="specific">
    <rule-definition>
        <item>My First Item</item>
        <item>My Second Item</item>
        <item>My Third Item</item>
        <item>My Fourth Item</item>
        <item>My Last Item</item>
    </rule-definition>
</ordered>
{% endhighlight %}

**Important note**: If, during execution, the test is encountering a member not specified in the rule (expl: *My Fifth Item*) then the test will **not fail**. In like manner, if the test is not encountering a member defined in your rule, the test will **not fail**. If you want to perform assertions to ensure that no expected or unexpected members exist, you should use the assertions described at the page [members' collection](/docs/members-collection).

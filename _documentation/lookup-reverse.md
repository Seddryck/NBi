---
layout: documentation
title: Reverse-lookup exists
prev_section: lookup-exists
next_section: lookup-matches
permalink: /docs/lookup-reverse/
---
A reverse-lookup must be considered when you want to be sure that the candidate result-set effectively contains each key available in your reference resut-set.

At the opposite of equivalence's tests, lookup's tests don't require the uniqueness of the rows in the reference or candidate tables. If a key from the reference result-set is not found in the candidate resut-set then the test will fail.

## System-under-test

The system under-test is any result-set representing the candidate table.

## Assertion

The assertion is defined by the xml element *lookup-exists* and the attribute *reverse* sets to *true*. The other parts of the assertion are identical to the parts defined for the [*lookup-exists* test](../lookup-exists).

{% highlight xml %}
<assert>
    <lookup-exists reverse="true">
        <join>
            <using column="#0" type="text"/>
        </join>
        <result-set>
            <query>...</query>
        </result-set>
    </lookup-exists>
</assert>
{% endhighlight %}

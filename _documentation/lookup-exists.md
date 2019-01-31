---
layout: documentation
title: Lookup exists
prev_section: connection-roles
next_section: lookup-reverse
permalink: /docs/lookup-exists/
---
Lookups are powerful methods to assert referential integrity between two result-sets.

Usage of *lookup-exists* is designed to check that a candidate result-set, defined in the system-under-test, contains foreign-keys registered in a reference result-set defined in the assertion.

At the opposite of equivalence's tests, lookup's tests don't require the uniqueness of the rows in the reference or candidate result-sets. If the foreign-key found in the candidate result-set is available in one or more rows of the reference table then the *lookup-exists* will succeed. If any foreign-key is not available in the reference table then the test will fail. Naturally, the candidate result-set can reference more than once each foreign-key.

Following cases are not considered as a failure of this test:

* If more than one row from the reference result-set validate the lookup
* If more than one row from the candidate result-set match with a same row from the reference result-set.
* If some rows of reference result-set have no corresponding rows in the candidate result-set.

Globally, in terms of algebra it means that the relation must be 1..n/0..n.

To work-around the two first limitations, or if you want to validate a more restrictive algebra relation between two result-sets, make usage of the test about rows-uniqueness on the candidate and/or reference result-sets. To work-around the third bullet, make use of an additional test with the feature *reverse* defined at the bottom.

## System-under-test

The system under-test is any result-set. More info [here](../compare-equivalence-resultsets).

## Assertion

The assertion is defined by the xml element *lookup-exists*. The two parts of the assertion are the *join* element, explaining how the two result-sets must be joined and the *result-set* element describing how to build the reference result-set.

### Jointure

To define a *lookup-exists* assertion, you must define the conditions on which the jointure between the reference and the candidate result-sets will be executed. This is done in the *join* element. Currently, NBi only supports strict-equality in the jointure. It means that the values of the keys in the candidate and reference result-sets must be equal to validate the jointure. This equivalence take into account the type of column (*text*, *numeric*, *dateTime* ...).

{% highlight xml %}
<assert>
    <lookup-exists>
        <join>
            ...
        </join>
        ...
    </lookup-exists>
</assert>
{% endhighlight %}

To define which column in the reference result-set should be compared to which column in the candidate result-set, you can rely on column positions (ordinal) or column names. If the two columns are respectivelly at the same place or have the same name, the you can use the *using* element.

{% highlight xml %}
<assert>
    <lookup-exists>
        <join>
            <using column="#0" type="text"/>
        </join>
        ...
    </lookup-exists>
</assert>
{% endhighlight %}

If the two columns are not named the same way and the not positionned at the same place, then you can use the element *mapping*.

{% highlight xml %}
<assert>
    <lookup-exists>
        <join>
            <mapping candidate="#2" reference="#0" type="numeric"/>
        </join>
        ...
    </lookup-exists>
</assert>
{% endhighlight %}

It's possible to define that the jointure must be executed on several columns. Just use on or more *mapping*/*using* elements.

{% highlight xml %}
<assert>
    <lookup-exists>
        <join>
            <mapping candidate="#2" reference="#0" type="numeric"/>
            <mapping candidate="#1" reference="#5" type="text"/>
        </join>
        ...
    </lookup-exists>
</assert>
{% endhighlight %}

### Reference result-set

The reference result-set contains the list of keys that can be referenced in the reference result-set (system-under-test). This result-set must be defined with a *result-set* element.

{% highlight xml %}
<assert>
    <lookup-exists>
        <join>
            ...
        </join>
        <result-set>
            <query>...</query>
        </result-set>
    </lookup-exists>
</assert>
{% endhighlight %}

More info about the definition of a result-set: [here](../compare-equivalence-resultsets).
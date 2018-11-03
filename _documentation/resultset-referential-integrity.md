---
layout: documentation
title: Referential integrity
prev_section: resultset-rows-count-advanced
next_section: resultset-format
permalink: /docs/referential-integrity/
---
This assertion checks that all rows of a result-set are matching with the rows of another dataset. In this case a match is a lookup or the equivalent of the notion of referential integrity. If you define a few columns of the system-under-test as keys in then the values of these keys must be available in the result-set defined in the assertion. The mapping between the keys in the two result-sets is also defined in the assertion.

Following cases are not considered as a failure of this test:

* If more than one row from the result-set defined in the assertion validate the lookup
* If more than one row from the result-set defined in the system-under-test match with the same row from the result-set defined in the assertion.
* If some rows of result-set defined in the assertion are never used to execute the the lookup

Globally, in terms of algebra it eans that the relation must be 1..n/0..n.

To work-around the two first limitations, or if you want to validate a more restrictive relation between two tables, make usag of the test about rows-uniqueness in the candidate and/or reference tables. To validate the third bullet, make use of an additional test with the feature *reverse* defined at the bottom.

## System under test

The system-under-test is a result-set, please check to other tests for more information about how to define a result-set. More info [here](../compare-equivalence-resultsets).

## Assertion

The assertion consists in an xml element named *lookup-exists*.
{% highlight xml %}
<assert>
  <lookup-exists />
</assert>
{% endhighlight %}

### Keys

The assertion is retrieving the keys of the system-under-test and try to find the equivalent keys in the result-set defined the assertion. The explanation about how to perform the join between the two result-sets is defined in an xml element *join*.

If the column's name are equivalent in both results-set you can use the *using* syntax and specify the column's name.

{% highlight xml %}
<assert>
  <lookup-exists>
    <join>
      <using>myKey<using>
    </join>
  </lookup-exists>
</assert>
{% endhighlight %}

If the column's names are different in the two result-sets, then you must use the *mapping* syntax and define a name in the *reference* (the result-set defined in the assert) and the *candidate* (the result-set defined in the system-under-test).

{% highlight xml %}
<assert>
  <lookup-exists>
    <join>
      <mapping candidate="myKey" reference="myColumn"/>
    </join>
  </lookup-exists>
</assert>
{% endhighlight %}

It's possible to define more than one key and you can combine the two syntaxes

{% highlight xml %}
<assert>
  <lookup-exists>
    <join>
      <using>myKey<using>
      <mapping candidate="myCol" reference="myColumn"/>
      <mapping candidate="myC" reference="myX"/>
    </join>
  </lookup-exists>
</assert>
{% endhighlight %}

### Reference Result-set

The reference result-set is defined in the assertion and could be any result-set More info [here](../compare-equivalence-resultsets).

{% highlight xml %}
<assert>
  <lookup-exists>
    <join>
      ...
    </join>
    <result-set>
       <query> select foo, bar from myOtherTable </query>
    </result-set>
  </lookup-exists>
</assert>
{% endhighlight %}

## Reverse the test

The standard definition of the test checks that all rows of result-set defined in the system-under-test have some matches in the result-set defined in the assertion. In some case it could be interesting to reverse the test and check that all rows of result-set defined in the assertion have some matches in the result-set defined in the system-under-test.

To achieve that use the *reverse* attribute and set it to *true*.

{% highlight xml %}
<assert>
  <lookup-exists reverse="true">
    ...
  </lookup-exists>
</assert>
{% endhighlight %}
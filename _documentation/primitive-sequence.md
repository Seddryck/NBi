---
layout: documentation
title: Sequence
prev_section: primitive-scalar
next_section: primitive-result-set
permalink: /docs/primitive-sequence/
---
One-dimensional array. All the values contained in a sequence are [scalars](../primitive-scalar) of the same type. A sequence contains a finite number of element but can also contain zero element.

## Definition

To specify a sequence, you can use any of the following options:

### List of values

The easiest way to define a sequence is to specify each member. This can be done using the *item* element one or more times.

{% highlight xml %}
<sequence type="text">
  <item>be</item>
  <item>fr</item>
</sequence>
{% endhighlight %}

### Sentinel loops

Usage of a *loop-sentinel* element is targetting the definition of sequence containing elements developed one by one with the help of a recursive calculation. The first element is the defined by the *seed* attribute and is always returned by the loop (meaning that a loop-sentinel has a minimum of one element). The next element of the sequence is calculated based on the seed and the addition of the *step* attribute. The third element will be calculated based on the second and the step. The sequence is over when the next element is greater or equal to the *terminal* attribute.

{% highlight xml %}
<sequence type="dateTime">
    <loop-sentinel seed="2015-01-01" terminal="2017-01-01" step="1 year"/>
</sequence>
{% endhighlight %}

Expl:

* A sequence having for seed the value 1, for step the value 1 and for terminal the value 5 will have 5 elements having for values 1,2,3,4,5.
* A sequence having for seed the value 1 for step the value 2, and for terminal the value 5 will have 3 elements having for values 1,3,5.
* A sequence having for seed the value 1 for step the value 3, and for terminal the value 5 will have 2 elements having for values 1 and 4.

Due to its definition, a loop-sentinel can only be used for the definition of a sequence containing a *numeric* type or a *dateTime* type.

For a *numeric* type, the *seed*, *step* and *terminal* attributes are defined as *numeric* scalars. In the case of a sequence containing scalar with a *dateTime* type, then the attributes *seed* and *terminal* are *dateTime* but the *step* is a *duration*.
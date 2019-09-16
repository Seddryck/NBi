---
layout: documentation
title: Lookup matches
prev_section: lookup-reverse
next_section: resultset-all-no-rows
permalink: /docs/lookup-matches/
---
A *lookup-matches* test must be considered when you want to be sure that some values found in a the candidate result-sets are the same than values found in the reference result-sets.

At the opposite of equivalence's tests, lookup's tests don't require the uniqueness of the rows in the reference or candidate result-sets. If a row from the candidate result-set has one or more matching keys in the reference table then the values associated to one of these matches must be equal in the reference result-set and candidate result-set. If a row from the candidate result-set has no matching key in the reference result-set, it's not considered as an issue for the test and this row will be considered as successful.

## System-under-test

The system under-test is any result-set representing the candidate table.

## Assertion

The assertion is defined by the xml element *lookup-matches*. Some parts of the assertion are identical to the parts defined for the [*lookup-exists* test](../lookup-exists). See [jointure](../lookup-exists#Jointure) and [Reference result-set](../lookup-exists#Reference%20result-set) for more information.

## Inclusion

In addition to the keys defined in the *join* element, the test is expecting to define the values that must be compared int the two result-sets. At the moment, NBi is supporting strict equality and is not supporting tolerance when comparing the values of the reference and candidate result-sets.

The definition of the mapping between the columns of the two result-sets are defined with the help of the *mapping* or *using* elements.

{% highlight xml %}
<assert>
  <lookup-matches>
    <join>
      <mapping candidate="DepartmentID" reference="Id" type="numeric"/>
    </join>
    <inclusion>
      <mapping candidate="DepartmentName" reference="Name" type="text"/>
    </inclusion>
    <result-set>
      <query>
        select [DepartmentID] as Id, [Name] from [HumanResources].[Department]
      </query>
    </result-set>
  </lookup-matches>
</assert>
{% endhighlight %}

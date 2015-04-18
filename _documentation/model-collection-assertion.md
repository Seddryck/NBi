---
layout: documentation
title: Collection of objects
prev_section: model-exists
next_section: model-relation
permalink: /docs/model-collection-assertion/
---
In the previous section we learnt how to validate the existence of an object. Sometimes, it's more helpful to perform tests on a collection of objects as the list of dimensions or the list of tables.

Tests on collection of objects support three assertions: *contain*, *subsetOf*, *equivalentTo*.

## System-under-test
At the difference of the [test of existence](model-exists), we'll define our *system-under-test* as a list of objects. So, in place of creating a system-under-test as *a dimension*, we'll have to instantiate a test on *the dimensions*. The available xml elements are
* perspectives
* dimensions
* hierarchies
* levels
* properties
* sets
* measure-groups
* measures
* schemas
* tables
* columns

At the difference of their respective singular xml element, the pluralized elements have no xml attribute named *caption*.

{% highlight xml %}
<system-under-test>
    <structure>
         <dimensions
             perspective="my perspective"
         />
    </structure>
</system-under-test>
</test>
{% endhighlight %}

## Assertions
### Contain
This assertion consists in a validation that one of the elements in a list of objects has a given caption. The corresponding xml element is named *contain* and has an attribute named *caption* specifying the *caption* of the object that you want to validate.

{% highlight xml %}
<contain caption="MyMember"/>
{% endhighlight %}

You can also check in a single test that a list of objects are part of the dimensions, or any other class of objects. From a unit testing point of view, you can argue that it’s probably not a good idea (for the granularity of the reporting) but NBi supports this option.

Your system-under-test stays unmodified but in your assert you’ll need to define one xml element named *item* by object that you’ll assert:
{% highlight xml %}
    <assert>
        <contain>
            <item>My first dimension</item>
            <item>My second dimension</item>
            <item>My third dimension</item>
        </contain>
    </assert>
{% endhighlight %}

This test will turn green (succeed) only if all the members defined in your assertion are effectively part of your structure.

## Objects belong to a predefined list
This kind of test is useful to ensure that you've no unexpected object in your collection of object. The traditional case is that you're expecting two dimensions named *My first dimension* and *My second dimension*, you want to assert that no third dimension is visible in your perspective.

{% highlight xml %}
    <assert>
        <subsetOf>
            <item>My first dimension</item>
            <item>My second dimension</item>
        </subsetOf>
    </assert>
{% endhighlight %}

This test will succeed only if all the elements of your structure are values provided in the list of item. If one of the dimensions listed in the *subsetOf* xml element doesn't exist in your structure, this will **not** fail.

## Objects match exactly to a predefined list
In some case, you know exactly the list of objects of your structure. In this case, you’ll probably want to test that the whole structure is correctly defined in your cube. From a unit testing point of view, you can argue that it’s probably not a good idea (for the granularity of the reporting) but NBi supports this option.

{% highlight xml %}
    <assert>
        <equivalentTo>
            <item>My first dimension</item>
            <item>My second dimension</item>
        </equivalentTo>
    </assert>
{% endhighlight %}

The test will succeed only if your structure has exactly the two elements which are named "My first dimension" and "My second dimension" (not more, not less).

Note that this test is equivalent to two assertions “contain” (one for "My first dimension" and another for "My second dimension") and one assertion “subsetOf” (for "My first dimension" and "My second dimension"). It’s just a matter of readability versus reporting facilities.

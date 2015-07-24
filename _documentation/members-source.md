---
layout: documentation
title: Members' source
prev_section: members-collection
next_section: members-patterns
permalink: /docs/members-source/
---
During the previous chapter, the assertions contained a list of predefined static values. But NBi supports to compare to a dynamic list of values.

## From a query
The first way to have a dynamic list of values is to retrieve the members from a query (Sql, Mdx or DAX). This can be useful if you've a list of members stored in a relational database and that this list is in a constant evolution (customers, malls, ...). To achieve this, you'll need to provide a *one-column-query* in place of the list of *item*. This *one-column-query* is just a standard *query* xml element where only the first column of the result-set will be used by NBi. You can define this xml element as:

{% highlight xml %}
<assert>
  <equivalentTo>
    <one-column-query>
      select displayName from Customer
    </one-column-query>
  </equivalentTo>
</assert>
{% endhighlight %}

The query will be executed and the first column of the result-set will be used to build the list of expected members. Then, the assertion will perform exactly as previously defined for a static list of members.

This xml element *one-column-query* supports the xml attribute *connectionString* but you're free to use the [defaults and references](/docs/config-defaults-references/).

## From another instance of the cube
This feature lets you compare your members to the members of a hierarchy/level/set from another instance of the cube. The main purpose of this feature is to let you compare members between different environments or between releases.

The syntax is straightforward, inside the xml element defining your assertion (*equivalenTo*, *contain*, *subsetOf*), you'll need to define an element *members* containing an element *level, hierarchy or set* exactly like in a system-under-test.

{% highlight xml %}
<test name="Members of department bellow 'Corporate' are in a subset of themselves" uid="0001">  
  <system-under-test>  
    <members children-of="Corporate">  
      <hierarchy
        caption="Departments"
        dimension="Department"
        perspective="Adventure Works"
      />  
    </members>  
  </system-under-test>  
  <assert>  
    <subsetOf>  
      <members children-of="Corporate" >  
        <hierarchy
          caption="Departments"
          dimension="Department"
          perspective="Adventure Works"
          connectionString="Provider=MSOLAP.4;Data Source=(local)\SQL2012;
            Initial Catalog='Adventure Works DW 2012';localeidentifier=1033"  
        />  
      </members>  
    </subsetOf>  
  </assert>  
</test>  
{% endhighlight %}

## From a range of members (integers/dates)
This feature is dedicated to some specific cases where a dimension is a list of dates or integers. In those cases the list of expected members can be defined easily with the help of of some xml elements. *Ranges* are dedicated to compare members' list to huge list of dates or integers without defining them one by one.

### Ranges

#### Integer
In the sample bellow, you will assert that your hierarchy's members are effectively a list of integer between 0 and 99. Without the range feature, you'd have to create 100 xml elements *item* contained in an *equivalentTo* element such as

{% highlight xml %}
<assert>
  <equivalentTo>
    <item>0</item>
    <item>1</item>
      ...
    <item>99</item>
  </equivalentTo>
</assert>
{% endhighlight %}

A bit daunting to write. The xml element named *range* lets you describe this list of members in a more readable and sustainable way. You have to create a **range-integer** xml element with two xml attributes *start* and *end*. Note that the value specified for *start* and *end* are included in the list of items built by NBi.
{% highlight xml %}
<assert>
  <equivalentTo>
      <range-integer start="0" end="99"/>
  </equivalentTo>
</assert>
{% endhighlight %}

#### Step

A *step* is the same concept as the step in a for loop. You can specify a *step* as an additional xml attribute. If you want to only have the even numbers between 0 and 99, you just need to define a **step** of 2.
{% highlight xml %}
<assert>
  <equivalentTo>
    <range-integer start="0" end="99" step="2"/>
  </equivalentTo>
</assert>
{% endhighlight %}

#### Attach constant string to your range of integers
This feature is helpful when you need to generate members such as "CY 2009", "CY 2010" and "CY 2011" or "Store 1", "Store 2", ... Using the basics of the *range-integer* element described above you can quickly build the **range-integer-pattern** attribute. In addition to the previously defined attributes *start*, *end* and *step*, you must also specify a *pattern* and a *position*.

The **pattern** states the constant string that you'll insert next to your integer. In the sample described above the patterns are respectively "CY " and "Store ".

The **position** attribute states where the pattern will be inserted. Two options are available *suffix* and *prefix*. In both samples above the correct value would be "prefix".

{% highlight xml %}
<assert>
  <equivalentTo>
    <range-integer-pattern
      start="2005"
      end="2010"
      pattern="CY "
      position="prefix"
    />
  </equivalentTo>
</assert>
{% endhighlight %}

### Dates

The same kind of feature is available for dates. In addition, to the *start* and *end* attributes, you must also specify a *culture* and a *format* attributes (the *step* attribute is not supported) and use the element **range-date**

The **format** attribute will let you describe how the date will be formatted. The supported formats are those described by Microsoft at [here](http://msdn.microsoft.com/en-us/library/az4se3k1.aspx) and [there](http://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx).

The attribute named **culture** let you specify in which language the weekdays and months will be rendered. The culture is described as a two letter code representing the language. A complete description of the culture supported is provided by Microsoft at [there](http://msdn.microsoft.com/en-us/goglobal/bb896001.aspx). NBi is expecting a value from this table, more specifically a 2 or 4 letters code available in the column named "Culture Name".

*Some samples:* The 19th of September 1995 will render differently according to the selected format and culture

* with a format equal to "dd/MM/yy" independently of the culture selected, the output will be "19/09/95"
* with a format equal to "d-MMM-yyyy" and a culture equivalent to "English", the output will be "19-SEP-1995"
* with a format equal to "dddd, dd MMMM" and a culture equivalent to "English", the output will be "Tuesday, 19 September"
* with a format equal to "dddd, dd MMMM" and a culture equivalent to "French", the output will be "Mardi, 19 Septembre"
* with a format equal to "d" and a culture equivalent to "English", the output will be "9/15/1995"

{% highlight xml %}
<assert>
  <equivalentTo>
    <range-date
      start="2005-01-01"
      end="2010-12-31"
      culture="en"
      format="MMMM d, yyyy"
    />
  </equivalentTo>
</assert>
{% endhighlight %}

Pay attention, that the *start* and *end* attributes are expecting dates with the universal format "yyyy-mm-dd" independently of the *format* attribute!

## From built-in predefined lists
Oups! not documented :-s

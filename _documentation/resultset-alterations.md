---
layout: documentation
title: Alterations
prev_section: syntax-2-0
next_section: 
permalink: /docs/resultset-alterations/
---
Using the [new syntax](../syntax-2-0/), it's possible to define alterations on a result-set. It gives you the possibility to alter the result-set without modifying the query retrieving it. It's especially useful when the alteration is complex to write in the query language or when it's not possible to modify the query (stored procedure, assembly, report-dataset ...). The two alterations supported by NBi are the filters and the converts.

## Filters

See [filters for row-count](../resultset-rows-count-advanced/#filter)

{% highlight xml %}
<resultSet>
  <query>
    ...
  <query>
  <alteration>
    <filter>
      <predicate operand="#0">
        <matches-date culture="fr-fr"/>
      </predicate>
    </filter>
  </alteration>
<resultSet>
{% endhighlight %}

## Converts

This alteration is useful when you want to convert a column of type *text* to a *dateTime* or *numeric*. This kind of translation is usually transparent for the test-writer and is performed with the help of an implicit casting. But implicit castings are limited to a predefined culture! It means that the textual value *2017-01-06* will be translated to the equivalent dateTime value but the textual value *06.01.2017* (6th of January 2017 in japanese culture) can't be translated to a dateTime column with an implicit casting. To achieve this translation, you'll need to apply an explicit conversion.

{% highlight xml %}
<resultSet>
  <query>
    ...
  <query>
  <alteration>
    <convert column="#0">
       <text-to-date culture="jp-jp"/>
    </convert>
  </alteration>
<resultSet>
{% endhighlight %}

The column to convert can be defined by its position (to achieve this, precede the zero-based position by a *#*) or by its name. 
The attribute culture is defined as one of the *Language Culture Name* at [this page](https://msdn.microsoft.com/en-us/library/ee825488(v=cs.20).aspx)

An optional *default-value* attribute will let you define the value returned if the conversion doesn't succeed. If this attribute is not set, the *null* value will be used. When the attribute is set, it must be conform to the correct notation of the expected type (*yyyy-MM-dd hh:mm:ss* or *yyyy-MM-dd* for dateTime or *###.###* for numeric)

{% highlight xml %}
<text-to-date culture="jp-jp" default="2000-01-01"/>
{% endhighlight %}

The different possibilities for the conversion are

* **text-to-date**: will use the *Short Date Pattern* of the specified culture  to try the conversion from text to a dateTime.
* **text-to-dateTime**: will use the concatenation of the *Short Date Pattern* and the *Long Time Pattern* of the specified culture to try the conversion from text to a dateTime.
* **text-to-numeric**: will use the *Decimal Separator* of the specified culture to try the conversion from text to a numeric.
---
layout: documentation
title: Alterations
prev_section: syntax-2-0
next_section: 
permalink: /docs/resultset-alterations/
---
Using the [new syntax](../syntax-2-0/), it's possible to define alterations on a result-set. It gives you the possibility to alter the result-set without modifying the query retrieving it. It's especially useful when the alteration is complex to write in the query language or when it's not possible to modify the query (stored procedure, assembly, report-dataset ...). The two alterations supported by NBi are the filters and the converts.

## Renamings

This alteration is useful when you want to rename a column. This kind of alteration is usually not needed because this kind of operation can be handled by the query. On the other hand, when dealing with flat files, it could save you!

To identify the original column to be renamed, you can use a column identifier of type ordinal such as *#3* or of type name such as *[myColumn]*. The new name of this column is a [scalar](../primitive-scalar/), it means that you can use a literal value but also variables, native transformations or formatting.

In the following example, the first column is renamed *keyField* and the column named *f1* is renamed based on the content of the variable *newName* upper-cased.

{% highlight xml %}
<result-set>
  <query>
    select 'a' as f0, 'FOO' as f1, null as f2 union all select 'B', 'bar', 'quark'
  </query>
  <alteration>
    <rename identifier="#0" new-name="keyField"/>
    <rename identifier="[f1]" new-name="@newName | text-to-upper"/>
  </alteration>
</result-set>
{% endhighlight %}

## Extensions

This alteration is useful when you want to create a new column based on the content of some other columns. At the moment, you cannot use variables or values from other rows. The definition of the content of the new column is performed with the help of the *NCalc* language, using column identifications (ordinal or names) as input parameters of the NCalc function.

In the following example, two new columns are created. The first one will be positioned as the first column (due to the identifier #0) and the second one will be added at the end of the result-set and named *myNewColumn*.

{% highlight xml %}
<result-set>
  <query>
    select 10 as ColA, 20 as ColB, 30 as ColC union all select 1, 5, 9
  </query>
  <alteration>
    <extend identifier="#0">
       <script language="ncalc">[#1] * Max([#2], [#3])</script>
    </extend>
    <extend identifier="[myNewColumn]">
       <script language="ncalc">[colA] * Max(ColB, ColC)</script>
    </extend>
  </alteration>
</result-set>
{% endhighlight %}

When using an index identifier the newly created column will be available at the expected position. If the expected position is unreachable (less columns that expected), the alteration will put the new column as the latest column. In case of a name identifier, if the newly created column has the same name than an existing column this column will be replaced.

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
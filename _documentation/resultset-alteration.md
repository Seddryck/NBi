---
layout: documentation
title: Alterations
prev_section: scalar-native-transformation
next_section: query-syntax
permalink: /docs/resultset-alteration/
---
Using the [new syntax](../syntax-2-0/), it's possible to define alterations on a result-set. It gives you the possibility to alter the result-set without modifying the query retrieving it. It's especially useful when the alteration is complex to write in the query language or when it's not possible to modify the query (stored procedure, assembly, report-dataset ...). The two alterations supported by NBi are the filters and the converts.

## Projections

This alteration is useful when you want to perform your test on a subset of the columns of your result-set.

To identify the columns to be include in the altered result-set, you must use the operation *project* and list the columns with a column identifier of type ordinal such as *#3* or of type name such as *[myColumn]*. 

The order of the columns in the result is specified by the order of the arguments. Only the columns specified in the arguments are included in the result: any others in the input are dropped.

In the following example, the first column and the column named *f2* will be available in the altered result-set, other columns will be discarded.

{% highlight xml %}
<result-set>
  <query>
    select 'a' as f0, 'FOO' as f1, null as f2 union all select 'B', 'bar', 'quark'
  </query>
  <alteration>
    <project>
      <column identifier="#0">
      <column identifier="[f2]">
    </project>
  </alteration>
</result-set>
{% endhighlight %}

At the opposite, the operator *project-away* expects the list of columns to be excluded in the altered result-set.

The order of the columns in the result is determined by their original order in the table. Only the columns that were specified as arguments are dropped. The other columns are included in the result.

{% highlight xml %}
<result-set>
  <query>
    select 'a' as f0, 'FOO' as f1, null as f2, getdate() as f3 union all select 'B', 'bar', 'quark', getdate()
  </query>
  <alteration>
    <project-away>
      <column identifier="#0">
      <column identifier="[f3]">
    </project-away>
  </alteration>
</result-set>
{% endhighlight %}

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

### Strategies for renamings

You also have the possibility to define a *missing* strategy to specify the behaviour of the renaming alteration when the column cannot be found in the original result-set. By defaut the behaviour is *failure*.

* ```failure```: The test executing this alteration will fail.
* ```skip```: The alteration is skipped without failure and a warning is raised

## Extensions

This alteration is useful when you want to create a new column based on the content of some other columns.

You'll have to identify the newly created column by its name or by its position in the result-set. When using an ordinal identifier the newly created column will be available at the expected position. If the expected position is unreachable (less columns that expected), the alteration will put the new column as the latest column. In case of a name identifier, if the newly created column has the same name than an existing column this column will be replaced.

The definition of the content of the new column is performed with the help of the *NCalc* language using column identifications (ordinal or names) or variables as input parameters of the NCalc function.

In the following example, two new columns are created. The first one will be positioned as the first column (due to the identifier #0) and the second one will be added at the end of the result-set and named *myNewColumn*.

{% highlight xml %}
<result-set>
  <query>
    select 10 as ColA, 20 as ColB, 30 as ColC union all select 1, 5, 9
  </query>
  <alteration>
    <extend identifier="#0">
       <script language="ncalc">[#1] * Max([#2], [#3]) - [@myNumericVariable]</script>
    </extend>
    <extend identifier="[myNewColumn]">
       <script language="ncalc">[colA] * Max(ColB, ColC)</script>
    </extend>
  </alteration>
</result-set>
{% endhighlight %}

Another engine supported is the [native transformations](../scalar-native-transformation). You can also use column's name or ordinal and variables, as initial value or as native transformation parameters, to define the initial value of the functions' parameters.

{% highlight xml %}
<result-set>
  <query>
    select 10 as ColA, 20 as ColB, 30 as ColC, 'alpha', '*' union all select 1, 5, 9, 'beta', '#'
  </query>
  <alteration>
    <extend identifier="#0">
      <script language="native">[ColA] | numeric-to-multiply([ColB]) | numeric-to-clip(0, [ColC])</script>
    </extend>
    <extend identifier="[myNewColumn]">
      <script language="native">[ColD] | text-to-upper | text-to-pad-left(@Count, [ColE])</script>
    </extend>
    <extend identifier="[myNewColumn2]">
      <script language="native">@MyNumericVariable | numeric-to-clip(0, [ColC])</script>
    </extend>
  </alteration>
</result-set>
{% endhighlight %}

## Lookup-replaces

This alteration is useful when you want to change the content of a column based on a dictionary.

The syntax is simiar to the syntax for lookup-exists and lookup-matches. The jointure must be performed on a single column. The content of this column will be replaced by the associated value in the reference result-set. The associated column of the reference result-set is defined in the *replacement* element.

In the example here under, the value of the column *myForeignKey* in the original result-set will be looking for in the reference result-set defined in the element *lookup-replace*. This search will be executed on column *myKey*. If a value is found, the original value in the candidate result-set is replaced by the content *myValue* of the reference result-set.

{% highlight xml %}
<result-set>
  <query>
    select 'a' as f0, 'FOO' as f1, null as f2 union all select 'B', 'bar', 'quark'
  </query>
  <alteration>
    <lookup-replace>
      <join>
        <mapping candidate="[myForeignKey]" reference="[myKey]" />
      </join>
      <result-set>
         ...
      </result-set>
      <replacement identifier="[myValue]" />
    </lookup-replace>
  </alteration>
</result-set>
{% endhighlight %}

### Strategies for lookup-replace

You also have the possibility to define a *missing* strategy to specify the behaviour of the lookup-replace alteration when the value of the candidate result-set cannot be found in the reference result-set. By defaut the behaviour is *failure*.

* ```failure```: The test executing this alteration will fail.
* ```original-value```: The original value of the candidate result-set is maintained.
* ```default-value```: The value of the candidate result-set is replaced by a default value. This value must be specify as a text in the *missing* element.
* ```discard-row```: The row of the candidate result-set is discarded.

{% highlight xml %}
<alteration>
  <lookup-replace>
    <missing behavior="default-value">Not found!</missing>
  </lookup-replace>
</alteration>
{% endhighlight %}

## Filters

### filter with predicate

Filters will let you remove some rows of the result-set based on the validation of one or more predicates. If more than one filter is specified, they will be applied one after the other and so will logically combine with an *and* operator.

For more info about the supported syntax check the page about [filters for row-count](../resultset-rows-count-advanced/#filter). For more info about the predicates supported, check the page about [predicates](../resultset-predicate).

{% highlight xml %}
<result-set>
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
<result-set>
{% endhighlight %}

### filter with ranking

It's also possible to filter a result-set based by only selecting the first/last rows of the result-set or of a group of rows within the result-set.

To apply a filter with ranking, you must specify the xml element *ranking* containing the element *top* or *bottom*. These two elements can be overriden with the xml attribute *count* to specify that you want to return more than one row.

The following example will return the two last rows of the whole result-set.

{% highlight xml %}
<alteration>
  <filter>
    <ranking>
      <bottom count="2"/>
    </ranking>
  </filter>
</alteration>
{% endhighlight %}

It's possible to create sub-groups of rows and only hold the first/last rows of these groups. To achieve this add a *group-by* element in the *ranking* element. You've two ways to define how to create groups. The first option, is to group rows sharing the same values of a set of columns (identical to SQL clause *group by*).

{% highlight xml %}
<alteration>
  <filter>
    <ranking>
      <bottom count="2"/>
      <group-by>
        <column identifier="#0" type="numeric"/>
        <column identifier="[country]" type="text">
      </group-by>
    </ranking>
  </filter>
</alteration>
{% endhighlight %}

Another option group rows is to group them depending on the evaluation of a predicate. For each group of row, you must define a predicate that will be evaluated for each row. Each definition of a group is handled by a xml element *case* containing an xml element *predicate*. As soon as an evaluation is positive for one of the group, the row is added to this group! The evaluation are always evaluated from the first definition of the group to the last. It means that if a group has a broad definition that could include elements of another group, this broad group should be defined at the bottom of the cases.

{% highlight xml %}
<alteration>
  <filter>
    <ranking>
      <bottom count="2"/>
      <group-by>
        <case>
          <predicate operand="#2" type="text">
            <equal>CUST0003</equal>
          </predicate>
        </case>
        <case>
          <predicate operand="#3" type="numeric">
            <more-than or-equal="true">100</more-than>
          </predicate>
        </case>
      </group-by>
    </ranking>
  </filter>
</alteration>
{% endhighlight %}

## Converts

This alteration is useful when you want to convert a column of type *text* to a *dateTime* or *numeric*. This kind of translation is usually transparent for the test-writer and is performed with the help of an implicit casting. But implicit castings are limited to a predefined culture! It means that the textual value *2017-01-06* will be translated to the equivalent dateTime value but the textual value *06.01.2017* (6th of January 2017 in japanese culture) can't be translated to a dateTime column with an implicit casting. To achieve this translation, you'll need to apply an explicit conversion.

{% highlight xml %}
<result-set>
  <query>
    ...
  <query>
  <alteration>
    <convert column="#0">
       <text-to-date culture="jp-jp"/>
    </convert>
  </alteration>
<result-set>
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

## Summarize

This alteration is useful when you want to produces a table that aggregates the content of the input result-set.

{% highlight xml %}
<result-set>
  <query>
    select 'supplier X' as supplier, 'apple' as fruit, 10.2 as price, '2019-01-01' as priceDate
    union all 'supplier Y', 'apple' , 10.6, '2019-01-01'
    union all 'supplier Y', 'orange' , 10.9, '2019-01-01'
    union all 'supplier Y', 'orange' , 10.5, '2019-01-02'
    union all 'supplier X', 'apple' , 10.7, '2019-01-02'
  <query>
  <alteration>
    <summarize>
      <average column="price" type="numeric"/>
      <group-by>
        <column identifier="fruit"/>
        <column identifier="supplier"/>
      </group-by>
    </summarize>
  </alteration>
<result-set>
{% endhighlight %}

A result-set that shows the average price of each fruit from each supplier. There's a row in the output for each distinct combination of fruit and supplier. The output columns show the average price, fruit and supplier. All other input columns are ignored.

|fruit|supplier|price
|-----|-----|-----
|Apple|Supplier X| 10.45
|Apple|Supplier Y| 10.6
|Orange|Supplier Y| 10.7

The different aggregations supported are

|aggregation|Numeric|DateTime|Text
|-----|-----|-----|-----
|min|Yes|Yes|No
|max|Yes|Yes|No
|average|Yes|No|No
|sum|Yes|No|No
|concatenation|No|No|Yes

Note that the aggregation *concatenation* is expecting an xml attribute *separator* defining the characters to place between two instances to concatenate.

## Reshaping

Reshaping alterations are useful when you want to change a column into a row or other transformations similar to this one.

### Unstack

Reshaping the data using the *unstack* operation converts the data into unstacked format .i.e. the row is unstacked column wise. The SQL "pivot" is similar to the unstack operator.

You must specify the column containing the distinct values that must be converted into columns into the element *header*. Suppose that a column contains the distinct values *Import* and *Export*. These two values will create two new columns named *import* and *export*.

Most of the time, you'll group the rows into buckets. Each bucket will result in a single row in the altered result-set. You need to specify the columns identifying the buckets into the element *group-by*.

The remaining column will be used to fill the content of the newly created columns.

{% highlight xml %}
<result-set>
  <query>
    select 'apple' as fruit, 'import' as direction, 10.2 as price
    union all 'apple' , 'export', 10.6, '2019-01-01'
    union all 'orange' , 'export', 10.5, '2019-01-02'
  <query>
  <alteration>
    <unstack>
      <header>
        <column identifier="direction"/>
      </header>
      <group-by>
        <column identifier="price"/>
      </group-by>
    </summarize>
  </alteration>
<result-set>
{% endhighlight %}

This alteration Will result in the following result-set

|fruit|import|export
|-----|-----|-----
|Apple|10.2| 10.6
|Orange|```(null)```| 10.5

Often, you'll need to ensure that some columns are available into the resulting result-set. This could be for defining columns to compare in an `equal-to` or to refer to these columns in additional alterations.

You can ensure that the columns will be there by defining `enforced-value` elements in the header. In the following example, the resulting result-set will have the columns *alpha*, *omega* (independantly of the fact that these values are contained into the column *Header*) and all the distinct values available in *Header*.

{% highlight xml %}
<header>
  <column identifier="Header"/>
  <enforced-value>alpha</enforced-value>
  <enforced-value>omega</enforced-value>
</header>
{% endhighlight %}
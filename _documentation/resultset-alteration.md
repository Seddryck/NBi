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

## Transformations

This alteration is useful when you want to slightly change the value of a column without creating a new one.

You'll have to identify the column to update by its name or by its position in the result-set and specify the original-type of this column (text, numeric, bool, 
)

The definition of the content of the new column is performed with the help of the ```native``` language (using [native transformations](../scalar-native-transformation)) using column identifications (ordinal or names) or variables as function parameters.

In the following example, all the values within the column in position 0 and the column named *ColD* are updated. The first column is multiplied by the content of second column and clipped between 0 and the content of the column *ColC*. The values of the column *ColdD* are simply transformed to the upper-case version.

{% highlight xml %}
<result-set>
  <query>
    select 10 as ColA, 20 as ColB, 30 as ColC, 'alpha' as ColD, '*' union all select 1, 5, 9, 'beta', '#'
  </query>
  <alteration>
    <transform identifier="#0" original-type="numeric">
      <script language="native">numeric-to-multiply(#1) | numeric-to-clip(0, [ColC])</script>
    </extend>
    <extend identifier="[ColD]" original-type="text">
      <script language="native"> text-to-upper</script>
    </extend>
  </alteration>
</result-set>
{% endhighlight %}

Transformations also support the ```c-sharp``` language. Principle applied to the native language are also applicable for C#. The initial value of cell is passed to the c# variable named *value* in the script.

{% highlight xml %}
<alteration>
  <transform column-index="0" language="c-sharp" original-type="text">
    "AA" + value;
  </transform>
  <transform column-index="1" language="c-sharp" original-type="text">
    value.Substring(value.Length - 4);
  </transform>
  <transform column-index="2" language="c-sharp" original-type="text">
    value.Substring(value.LastIndexOf("0")+1)
  </transform>
</alteration>
{% endhighlight %}

## Merging and concatening

This alteration is useful when you want to combine two result-sets to create a new one.

### Union

This alteration simply append the rows of the second results to the rows of the first result-set, returning the rows of all of them.

{% highlight xml %}
<result-set>
  <query>
    select 'Apple' as Fruit, 10 as Qty union all select 'Orange', 15
  </query>
  <alteration>
    <union>
      <result-set>
        <row>
          <cell>Peer</cell>
          <cell>5</cell>
        </row>
      </result-set>
    </union>
  </alteration>
</result-set>
{% endhighlight %}

You can specify an attribute *column-identity* to define the strategy to match columns of the first result-set with columns of the second dataset. By default the value is set to *ordinal* meaning that the first column of the first result-set is considered as the same column than the first column of the second result-set (and so on). If the second result-set has more columns than the first one, they are added at the end.

The value *name* for the attribute *column-identity* means that the column matching is based on the name. The second result-set will suffer a reordering of the columns to match with the column order of the first result-set. If the second result-set has more columns than the first one, they are added at the end.

{% highlight xml %}
<result-set>
  <query>
    select 'Apple' as Fruit, 10 as Qty union all select 'Orange', 15
  </query>
  <alteration>
    <union column-identity="name">
      <result-set>
        <query>
          select 5 as Qty, 'Apple' as Fruit, 'Fall' as Season
        </query>
      </result-set>
    </union>
  </alteration>
</result-set>
{% endhighlight %}

The content of the cells included in columns not existing in the first or second result-set are always set to ```(null)```.

### Cartesian product

This alteration is simply combining each row from the first result-set with each row of the second result-set.

To specify this alteration, just specify the *merge* element containing a *result-set*.

{% highlight xml %}
<result-set>
  <query>
    select 'Apple' as Fruit, 10 as Qty union all select 'Orange', 15
  </query>
  <alteration>
    <merge>
      <result-set>
        <row>
          <cell>Supplier X</cell>
          <cell>Foo</cell>
        </row>
        <row>
          <cell>Supplier Y</cell>
          <cell>Bar</cell>
        </row>
      </result-set>
    </merge>
  </alteration>
</result-set>
{% endhighlight %}

Previous alteration will return the following result-set:

|Fruit|Qty|Column0|Column1
|-----|-----|-----|------
|Apple|10|Supplier X|Foo
|Orange|15|Supplier X|Foo
|Apple|10|Supplier Y|Bar
|Orange|15|Supplier Y|Bar

NB: if any result-set is empty (no rows) then the resulting result-set will also be empty.

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

### Filters unique rows

In some cases, it's interesting to discard duplicated rows. You can achieve with a filter named *unique*. This filter uses a *group-by* element to specify the keys of the result-sets that will determine uniqueness. The groupings by-columns and by-cases are supported. If a group of rows contains more than one row then the **whole** group is discarded, else the group is hold. The resulting result-set after application of the filter will contain all the initial columns.

{% highlight xml %}
<alteration>
  <filter>
    <unique>
      <group-by>
        <column identifier="#0"/>
        <column identifier="#1"/>
      </group-by>
    </unique>
  </filter>
</alteration>
{% endhighlight %}

## Duplications

This alteration duplicates the rows of a result-set. How many times the rows are duplcated is specified with the help of the *times* attribute, expecting a static value (integer) or a variable or a column identification. The value of the attribute ```times``` specifies how many copies are applied to the original rows, meaning that a value of 0 will return the original rows but also that a value of 1 will return a result-set with two times the count of rows from the original result-set.

{% highlight xml %}
<result-set>
  <query>
    ...
  <query>
  <alteration>
    <duplicate>
      <times>1</times>
    </convert>
  </alteration>
<result-set>
{% endhighlight %}
  
It's possible to define a predicate to filter the rows from the original result-sets that must be duplicated. Only the original rows validating the predicate will be duplicated. Original rows not validating the predicate will still be present in the returned result-set but won't be duplicated.

{% highlight xml %}
<result-set>
  <query>
    ...
  <query>
  <alteration>
    <duplicate>
      <predicate operand="Period" type="text">
        <equal>Year</equal>
      <predicate>
      <times>2</times>
    </convert>
  </alteration>
<result-set>
{% endhighlight %}
  
It's possible to add additional columns to the returned result-set. To achieve this you must specify these additional columns in *output* elements. Each *output* element must specify an identifier in the attribute *identifier* which could be the name of the new column or its position (to achieve this, precede the zero-based position by a *#*).

You must also define the class of the *output* element. Following classes are supported:

* *is-original*: returns a boolean specifying if the row was part of the original result-set or is a duplication.
* *is-duplicable*: returns a boolean specifying if the row is validating the predicate. In case there is no predicate, the value is always ```true```. This value is identical for the original row or for all these duplications.
* *index*: returns a zero-based index of the duplication. If a row is duplicated 10 times, each duplicated row will have a different index starting at 0 and ending at 9.
* *total*: returns the total of duplication of the original row (identical to the value of the element *times*). This value is interesting when times is dynamically evaluated. 
* *static*: returns a static value. The value is defined in a *value* element.
* *script*: returns a value provided by a script dynamically evaluated for each row. The script is defined in a *script* element, the supported languages are *native* and *NCalc*. This script can use the ouput columns defined before the output containing the scripts, as such it's possible to use the *index* and *total* values in your script.

The column identified as an output by the means of the attribute *identifier* could be a new column or an existing column. In the case of an existing column, the original value of the column will be replaced by the returned value of the output but only for duplicated rows. It means that the original row is not impacted by the outputs, new columns created by the ouputs will received a value of ```(null)```. This rule is not applicable if the class is *is-original* or *is-duplicabale*, which will apply the boolean value even for the original row (overriding the value of the cell if the column is already existing)!

In the following examples, row of the original result-set are duplicated two times only if the content of the second column is equal to *year*. For the duplicated values, the output includes the index and the total, the existing value of the column named *Value* is replaced by the original value of this column divided by the total count of duplications and multiplied by ythe index of duplication incremented by one unit. The value of the existing column *Period* is replaced by the concatenation of the original value a *H* and the index (incremented). Value of the column *TimeSpan* is replaced by the static value *Semester*. Original rows are not affected by all these changes and will return ```(null)``` for the total and index and original values are preserved for all the other columns.

{% highlight xml %}
<duplicate>
  <predicate operand="#1" type="text">
    <equal>Year</equal>
  </predicate>
  <times>2</times>
  <output identifier="Total" class="total"/>
  <output identifier="Index" class="index"/>
  <output identifier="Value" class="script">
    <script language="ncalc">[Value] / [Total] * ([Index]+1)</script>
  </output>
  <output identifier="Period" class="script">
    <script language="native">[Index] | numeric-to-increment | text-to-prefix(H) | text-to-prefix([Period])</script>
  </output>
  <output identifier="TimeSpan" class="static">
    <value>Semester</value>
  </output>
</duplicate>
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
|count|Yes|Yes|Yes

Note that the aggregation *concatenation* is expecting an xml attribute *separator* defining the characters to place between two instances to concatenate. 

{% highlight xml %}
<summarize>
  <concatenate column="supplier" type="text" separator=", "/>
  <group-by>
    <column identifier="fruit"/>
  </group-by>
</summarize>
{% endhighlight %}

|fruit|supplier
|-----|-----
|Apple|Supplier X, Supplier Y, Supplier X
|Orange|Supplier Y, Supplier Y

The aggregation *count* is not expecting a column, it will count the instances of rows available in each group. The new column created is named *count*. If this column is already existing an ordinal suffix will be added.

{% highlight xml %}
<summarize>
  <count type="numeric"/>
  <group-by>
    <column identifier="fruit"/>
    <column identifier="supplier"/>
  </group-by>
</summarize>
{% endhighlight %}

|fruit|supplier|count
|-----|-----|-----
|Apple|Supplier X| 2
|Apple|Supplier Y| 1
|Orange|Supplier Y| 2

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
        <column identifier="fruit"/>
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
  <enforced-value>import</enforced-value>
  <enforced-value>export</enforced-value>
</header>
{% endhighlight %}

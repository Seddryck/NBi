---
layout: documentation
title: Expressions and predicates
prev_section: resultset-all-no-rows
next_section: resultset-rows-count
permalink: /docs/resultset-predicate/
---
A predicate is a condition that will be tested for each row of the result-set. If this condition is positively evaluated then the row will validate the predicate else not. Constraints such as *all-rows* or *no-rows* will check that respectively all and no rows are validating the predicate.

## content of cells for each row.

For this kind of test, you'll assert the value of one cell (or a combination of cells) of each row with a predicate (see bellow). This cell is named the *operand* and must be specified to the predicate by the means of the *operand* attribute. Before version 1.17, this attribute was named *name*, this notation is now deprecated.

{% highlight xml %}
<assertion>
    <no-rows>
        <predicate operand="myColumn"/>
    </no-rows>
</assertion>
{% endhighlight %}

To identify a column, you can refer to it by its name but you've more options:

* By its index: To apply this strategy, use the syntax *#3* where the number 3 identifies the fourth column (index equal to 3). The *#* specifies to NBi that you'll be using a column index.
* By its name: To apply this strategy, just use the name of column. If this name is containing a space or is starting by a figure just surround it with square brackets. For example the following syntax identifies *[1 col]* a column name '1 col'.
* By an alias: To apply this strategy you must use the element *alias* (before the predicate) and specify a column index with the attribute *column*. The name of the alias is specified as the inner text of this element.

{% highlight xml %}
<assertion>
    <no-rows>
        <alias column="1">MyColumnAlias</alias>
        <predicate operand="MyColumnAlias"/>
    </no-rows>
</assertion>
{% endhighlight %}

If the name of an alias is conflecting with the name of a column, the alias has the precedence.

Before version 1.16, only the third option was available and the element was named *variable* and not *alias*. The notation *variable* is now deprecated. 

## Expressions

You can use an *expression* to combine two cells of the same row. For example, if you've two columns *UnitPrice* and *Quantity*, you can calculate the *TotalPrice* with an expression. To achieve this, you must define en element *expression* and set its formula. To reference a cell in your formula, use one of the three strategies above. You can also combine the content of the cells with fixed value. If you want to calculate a *TotalPriceWithVAT* based on the columns *Quantity* and *UnitPrice*, you can define an expression equal to *UnitPrice * Quantity * 1.21*

In this kind of test, a cell can be used later in the *predicate* or in an *expression*. For each row, the value contained in the different columns will be assigned to the variable. If you want to assert that the column with index 1 is greater than 1000 then the first step is to create a variable for the column with a column-index equal to 1 and give it a name.

{% highlight xml %}
<assertion>
    <all-rows>
        <expression name="TotalPriceWithVAT">UnitPrice * Quantity * 1.21</variable>
    </all-rows>
</assertion>
{% endhighlight %}

It's possible to use an expression in an expression (nested expressions). The previous example could be written:

{% highlight xml %}
<assertion>
    <all-rows>
        <expression name="TotalPrice">UnitPrice * Quantity</variable>
        <expression name="TotalPriceWithVAT">TotalPrice * 1.21</variable>
    </all-rows>
</assertion>
{% endhighlight %}

The functions supported in an *expression* are these supported by [NCalc](https://ncalc.codeplex.com/wikipage?title=functions&referringTitle=Home)

## Different predicates

The predicate can be used with the previously defined assertions: *no-rows* and *all-rows*. They supports many different operators, see the table here under for the full list. The two options *more-than* and *less-than* also supports the variant *or-equal* moreover the option *empty* supports the variant *or-null*. Some of the text specific operators (*starts-with*, *ends-with*, *contains*, *matches-regex*) supports the variant *ignore-case*. The operator *modulo* is expecting a second operand (the divisor) that you can specify in the attribute *second-operand*.

In addition to this operator, you must also define [the column or expression](../resultset-all-no-rows/) that you want to validate with this predicate. This indication is provided by identifying the column or the expression in the attribute *name*. Once again, you can use the three strategies described above to identify a column and for an expression, you can use its name.

Each predicate is not valid for each data type. The list of possible combinaison is described here under:

| Predicate | Text | Numeric | DateTime | Boolean 
|-------------|:-----------------:|:-------------------:|
| equal  | Yes | Yes | Yes | Yes
| more-than  | Yes | Yes | Yes | No
| less-than  | Yes | Yes | Yes | No
| null  | Yes | Yes | Yes | Yes
| empty  | Yes | No | No | No
| starts-with  | Yes | No | No | No
| ends-with  | Yes | No | No | No
| contains  | Yes | No | No | No
| lower-case  | Yes | No | No | No
| upper-case  | Yes | No | No | No
| matches-regex  | Yes | No | No | No
| matches-numeric  | Yes | No | No | No
| matches-date  | Yes | No | No | No
| matches-time  | Yes | No | No | No
| within-list  | Yes | No | No | No
| within-range  | No | Yes | Yes | No
| integer  | No | Yes | No | No
| modulo | No | Yes | No | No
| on-the-day  | No | No | Yes | No
| on-the-minute | No | No | Yes | No
| on-the-second | No | No | Yes | No
| true | No | No | No | Yes
| false | No | No | No | Yes

{% highlight xml %}
<assertion>
    <all-rows>
        ...
        <predicate operand="FirstName">
           <upper-case>
        <predicate>
    </all-rows>
</assertion>
{% endhighlight %}

Some of the predicates, require to specify a reference. For example if you want to check that the content of a column is greater than 1000 then your reference is 1000. This value must be specified in the inner text of the predicate element.

The predicate *within-list* is not expecting a unique reference but a list of items as reference. Use the xml element *item* to delimitate each item.

The predicates *equal*, *more/less-than*, *starts/ends-with*, *contains*, *matches-regex* and *within-list* are supporting two kinds of comparison for textual: case-sensitive (default) and case-insensitive. To change this behaviour adapat the value of the xml attribute *ignore-case* (default is false).

The following predicates are expecting a culture: *matches-numeric*, *matches-date*, *matches-time*. The culture is a group of 4 letters separated in two groups of two by the means of a dash. The list of valid cultures is defined at [MSDN](https://msdn.microsoft.com/en-us/library/ee825488(v=cs.20).aspx). If this culture is not provided then an invariant culture is applied with the pattern *yyyy-MM-dd* for the date format,  *HH:mm* for the time format and a dot as decimal separator (no thousand seperator) for the numeric format.



{% highlight xml %}
<assertion>
    <all-rows>
        ...
        <predicate operand="TotalPriceWithVAT">
           <more-than or-equal="true">1000<more-than>
        <predicate>
    </all-rows>
</assertion>
{% endhighlight %}

### Negation of a predicate

It could be useful to use the negation of a predicate. By specifying the attribute *not* available for each predicate, the result of the predicate's evalution will be inverted (false will become true and true will become false).

{% highlight xml %}
<assertion>
    <all-rows>
        ...
        <predicate operand="Name">
           <lower-case not="true"/>
        <predicate>
    </all-rows>
</assertion>
{% endhighlight %}

In the example above, the test will succeed if at least one of the charachter of each *Name* is not a lower-case char.

## Variables for predicate's reference

Sometimes, the reference must be dynamic. One of the most famous examples is the need to check that all rows returned by the query are for the days before today. When you've this kind of issues, you can use a *[variable](..docs/variable-define)*. These items are described at the top of the test-suite and are dynamically evaluated. To reference them in the predicate you must use the name of the variable prefixed by an arrobas (@)

{% highlight xml %}
<variables>
   <variable name="maxAmount">
      <script language="c-sharp">10*10*10</script>
   </variable>
</variables>
...
<assertion>
    <all-rows>
        <alias column-index="1">Quantity</variable>
        <expression name="TotalPriceWithVAT">[UnitPrice] * Quantity * [#3]</variable>
        <predicate name="TotalPriceWithVAT">
           <more-than or-equal="true">@maxAmount<more-than>
        <predicate>
    </all-rows>
</assertion>
{% endhighlight %}

## Combination of predicates

Since version 1.17, it's possible to combine predicates with one of the three operators *and*, *or* and *xor*. To achieve this you must specify the element *combination* and specify the operator in the attribute *operator*. This operator will be used between each operator. To specify that the *TotalPriceWithVAT* must be greater or eaqual to *@maxAmount* or that the column with index 0 must be in upper-case, apply the following guidelines.

{% highlight xml %}
<assertion>
    <all-rows>
        <alias column-index="1">Quantity</variable>
        <expression name="TotalPriceWithVAT">[UnitPrice] * Quantity * [#3]</variable>
        <combination operator="or">
            <predicate operand="TotalPriceWithVAT">
                <more-than or-equal="true">@maxAmount<more-than>
            <predicate>
            <predicate operand="#0">
                <upper-case/>
            <predicate>
        </combination>
    </all-rows>
</assertion>
{% endhighlight %}

To be able to identify quickly the root cause of your bugs, we do not recommend the usage of the *and* operator. In place, create two tests.
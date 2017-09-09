---
layout: documentation
title: All/no rows validate a predicate
prev_section: connection-roles
next_section: resultset-rows-count
permalink: /docs/resultset-all-no-rows/
---
This feature is intended to support the assertion that all rows (or no rows) of a result-set validate a predicate. For example, you want to test that a query effectively returns all the sales with a cost higher or equal to 1000$. A check could be that the value returned by the query for *TotalAmountWithVAT* is greater or equal than 1000 for each row!

For more complex testing conditions than all/no rows, check the [advanced row-count](../resultset-rows-count-advanced/) page.

## System under test

The system-under-test is a query, please reports to other tests for more information about how to define a query. More info [here](/docs/compare-equivalence-resultsets).

## Assertion

To specify this kind of test, you need to define an assertion with the xml elements *all-rows* or *no-rows*. *all-rows* will passes the test only if all the rows of the result-set validate the predicate, if at least one row doesn't validate the predicate, the test will fail. At the opposite, *no-rows* will fail if at least one row validate the predicate (and succeed in the other case). If the result-set is empty, both assertions will succeed.

{% highlight xml %}
<assertion>
    <all-rows>
        ...
    </all-rows>
</assertion>
{% endhighlight %}
or
{% highlight xml %}
<assertion>
    <no-rows>
        ...
    </no-rows>
</assertion>
{% endhighlight %}

## Variables and expressions.

In this kind of test a *variable* is mapped to a column and can be used later in the *predicate* or in an *expression*. For each row, the value contained in the column will be assigned to the variable. If you want to assert that the column with index 1 is greater than 1000 then the first step is to create a variable for the column with a column-index equal to 1 and give it a name.

{% highlight xml %}
<assertion>
    <all-rows>
        <variable column-index="1">Quantity</variable>
        <variable column-index="2">UnitPrice</variable>
    </all-rows>
</assertion>
{% endhighlight %}

An expression let you define a mathematical calculation based on one or more variable and fixed value. If you want to calculate a TotalPriceWithVAT based on the variables Quantity and UnitPrice, you can define an expression equal to UnitPrice*Quantity*1.21

{% highlight xml %}
<assertion>
    <all-rows>
        <variable column-index="1">Quantity</variable>
        <variable column-index="2">UnitPrice</variable>
        <expression name="TotalPriceWithVAT">UnitPrice*Quantity*1.21</variable>
    </all-rows>
</assertion>
{% endhighlight %}

## Predicate

The predicate supports nine different operators: *equal*, *more-than*, *less-than*, *empty*, *null*, *starts-with*, *ends-with*, *contains*, *matches-regex*. The two options *more-than* and *less-than* also supports the variant *or-equal* moreover the option *empty* supports the variant *or-null*. The text specific operators (*starts-with*, *ends-with*, *contains*, *matches-regex*) supports the variant *ignore-case*.

In addition to this operator, you must also define the variable or expression that you want to validate this predicate. This indication is provided by the name of the variable or the expression.

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
| within-range  | No | Yes | Yes | No

{% highlight xml %}
<assertion>
    <all-rows>
        ...
        <predicate name="TotalPriceWithVAT">
           <more-than or-equal="true">1000<less-than>
        <predicate>
    </all-rows>
</assertion>
{% endhighlight %}

Full example:

{% highlight xml %}
<assertion>
    <all-rows>
        <variable column-index="1">Quantity</variable>
        <variable column-index="2">UnitPrice</variable>
        <expression name="TotalPriceWithVAT">UnitPrice*Quantity*1.21</variable>
        <predicate name="TotalPriceWithVAT">
           <more-than or-equal="true">1000<less-than>
        <predicate>
    </all-rows>
</assertion>
{% endhighlight %}

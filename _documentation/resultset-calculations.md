---
layout: documentation
title: Calculations in a result-set
prev_section: resultset-format
next_section: transform-column
permalink: /docs/resultset-calculations/
---
In some result-sets, it's possible to check the result of one field based on the values of other fields **of the same row**. To illustrate this, imagine a result-set with the columns _UnitPrice_ and the _Quantity_, for each product bought on a shop. An existing third field returns the _Price to pay_. This last field is equal to _UnitPrice_ multiplied by _Quantity_ ... well at least you hope that it's the case for each line but you're not sure ... and you probably want to test it!

This test will let you define an expression which must be validated by each result-set's row. If at least one row of this result-set doesn't validate one of the expression defined in the test, the test is failed.

## System-under-test

The system-under-test is a query, please reports to other tests for more information [here](../../docs/compare-equivalence-resultsets)..

## Assert

The assertion consists of an xml element _evaluate-rows_ embedding two xml elements defining the columns to check and the columns with pertinent values for these checks.
{% highlight xml %}
<assert>  
  <evaluate-rows>  
    <variable ... />
		...
    <expression ... />
  </evaluate-rows>  
</assert>  
{% endhighlight %}

The xml element _variable_ contains information about a column containing values that will be used in the calculation. You must define for this column the column-index (reminder: the first column has an index of 0) and the name of the variable (this name will be used in the calculations). This name is independent of the column's name returned by the query and should contain no space or hyphen.
{% highlight xml %}
 <assert>  
	<evaluate-rows>  
		<variable column-index="2">OrderQuantity</variable>
		<variable column-index="3">UnitPrice</variable>
		<variable column-index="4">UnitDiscount</variable>
                ...
	</evaluate-rows>  
</assert>  
{% endhighlight %}

The xml element _expression_ contains the expression that will be evaluated and the operator.

The operator defines the comparison that will be executed between the result of the evaluation of an expression and the corresponding cell in the result set. Currently you've the choice between two operators:

* = means that to validate the expression the result of the calculation must match with the value retrieved in the result set.
* != means that to validate the expression the result of the calculation must differ with the value retrieved in the result set.

The operator must be followed by a valid expression. An expression makes usage of the variables defined previously in your test and combines them with standard mathematical operators such as +,-,\*,/ but also more advanced functions such as Abs, Cos, Sin, ... The list of functions supported in an expression is defined in the documentation of the library used by NBi to cover this feature: documentation NCalc

In addition, to the expression and the operator, you must specify the column-index. See above for more information.

If you want you can also specify the type returned by the expression (more info about types: Column’s types). It's also possible to specify a tolerance (more info about tolerance: Tolerance)

{% highlight xml %}
 <assert>  
	<evaluate-rows>  
		...
    <expression column-index="5" type="numeric" tolerance="0.01">
      = OrderQuantity*(UnitPrice-(UnitPrice*UnitDiscount))
    </expression>
	</evaluate-rows>  
</assert>  
{% endhighlight %}


## Full Example

{% highlight xml %}
<test name="Validation calculation of LineTotal" uid="0001">
  <system-under-test>
    <execution>
      <query>
        select top 100
          SalesOrderID
          , [CarrierTrackingNumber]
          , OrderQty
          , UnitPrice
          , UnitPriceDiscount
          , LineTotal
        from
          Sales.SalesOrderDetail
        where
          UnitPriceDiscount>0
      </query>
    </execution>
  </system-under-test>
  <assert>
    <evaluate-rows>
      <variable column-index="2">OrderQuantity</variable>
      <variable column-index="3">UnitPrice</variable>
      <variable column-index="4">UnitDiscount</variable>
      <expression column-index="5" type="numeric" tolerance="0.01">
         = OrderQuantity*(UnitPrice-(UnitPrice*UnitDiscount))
      </expression>
    </evaluate-rows>
  </assert>
</test>
{% endhighlight %}

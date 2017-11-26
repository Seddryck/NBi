---
layout: documentation
title: Define variables
prev_section: config-traces-debugging
next_section: metadata-concept
permalink: /docs/variable-define/
---
Version 1.17 has introduced the notion of *variable*. A variable is a scalar-value (a unique value, not a list of a result-set) that can be reused in different places of your test-suites. another big advantage of variables is that they are evaluated during the test-suite execution. Suppose that you have a query expecting a date as a parameter and that you want to specify the current date: without a variable, it's not possible!

# Definition

The variables are defined at the top of the test-suite (after *settings* but before the first *test*) in an element named *variables*.

{% highlight xml %}
<variables>
    <variable name="FirstOfCurrentMonth">
      <script language="c-sharp">
        DateTime.Now.AddDays(1 - DateTime.Now.Day)
      </script>
    </variable>
    <variable name="CurrencyCode">
      <query-scalar>
        <![CDATA[select top(1) CurrencyCode from [Sales].[Currency] where Name like '%Canad%']]>
      </query-scalar>
    </variable>
  </variables>
{% endhighlight %}

As you can understand fom the fragment above, a variable can be evaluated based on two engines: *C#* and *query-scalar*

## C# engine

This engine evaluates one unique sentence of C# and returns the corresponding value. In order to specify this engine use the element *script* and specify the attribute *language* to the value *c-sharp*. Then you'll be ableto specify your c# sentence in the inner text of this element. Note that thsi sentence shouldn't start by *return* and neither end by a semi-column (;).

## Query engine

This engine evaluates a query and returns the first cell of the first row returned by this query. In order to specify this engine use the element *query-scalar* and specify. Then you'll be able to specify a query with the different methods available in the [NBi syntax 2.0 to define a query](../docs/syntax-2-0).

# Usage

In this first release, you can't use the variables at many places. the usage is strictly limited to the following places:

* *[Parameter](..docs/query-parameter)* (of a query)
* In the [comparisons (*equal*, *more-than*, *less-than*)](../docs/resultset-rows-count) for a *row-count*
* In the [predicates](../docs/resultset-predicate) of the assertions *row-count*, *all-rows*, *no-rows*, *some-rows* and *single-row*

If you've other places, where you think that a variable would be helpful, report it by creating an [issues](http://github.com/Seddryck/nbi/issues)

# Notes about the future of variables

Variables will be extented in the next releases. The goal is to let you define them at different places (*groups* and perhaps *tests*). The goal is also to evaluate them only when needed (execution) and not during the load of the test-suite (currently depending of the place where the variable is used). A variable should not be evaluated twice, once it has been evaluated, the value will be cached. In these early releases, relying on these features is hazardous.
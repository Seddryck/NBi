---
layout: documentation
title: Define variables
prev_section: config-traces-debugging
next_section: variable-override
permalink: /docs/variable-define/
---
A variable is a scalar-value (a unique value, not a list of a result-set) that can be reused in different places of your test-suites. Another big advantage of variables is that they are evaluated during the test-suite execution. Suppose that you have a query expecting a date as a parameter and that you want to specify the current date: without a variable, it's not possible! A variable is only evaluated once: after it has been evaluated, the value is cached and never updated.

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

As you can understand fom the fragment above, a variable can be evaluated based on different engines.

## C# engine

This engine evaluates one unique sentence of C# and returns the corresponding value. In order to specify this engine use the element *script* and specify the attribute *language* to the value *c-sharp*. Then you'll be ableto specify your c# sentence in the inner text of this element. Note that thsi sentence shouldn't start by *return* and neither end by a semi-column (;).

In this example, the variable named *FirstOfCurrentMonth* is set to the value returned by the C# script:

{% highlight xml %}
<variable name="FirstOfCurrentMonth">
  <script language="c-sharp">
    DateTime.Now.AddDays(1 - DateTime.Now.Day)
  </script>
</variable>
{% endhighlight %}

## Query engine

This engine evaluates a query and returns the first cell of the first row returned by this query. In order to specify this engine use the element *query-scalar* and specify. Then you'll be able to specify a query with the different methods available in the [NBi syntax 2.0 to define a query](../syntax-2-0).

In this example, the variable named *CurrencyCode* is set to the single value returned by the query here under:

{% highlight xml %}
<variable name="CurrencyCode">
  <query-scalar>
    <![CDATA[select top(1) CurrencyCode from [Sales].[Currency] where Name like '%Canad%']]>
  </query-scalar>
</variable>
{% endhighlight %}

## Environment variable

This engine retrieves the value of an environment variable.

In this example, the variable named *myVar* is set to the value of the environment variable named *MyEnvVar*:

{% highlight xml %}
<variable name="myVar"/>
  <environment name="MyEnvVar"/>
</variable>
{% endhighlight %}

## Custom variable

This solution retrieves the value from an external C# assembly. This assembly must contain one or more types implementing the interface *IScalarResolver*.

In this example, the variable named *myVar* is set to the value returned by the type *MyCustomClass* of the assembly *myassembly.dll* when executing the method *Execute()* . Optionaly, you can pass some parameters to the type *MyType* when instantiating it. In this example, the class *MyCustomClass* has a constructor accepting two parameters (*foo*, *bar*).

{% highlight xml %}
<variable name="myVar"/>
  <custom assembly="myAssembly.dll" type="MyCustomClass">
    <parameter name="bar">10</parameter>
    <parameter name="foo">@myValue</parameter>
  </custom>
</variable>
{% endhighlight %}

{% highlight csharp %}
using NBi.Core.Scalar.Resolver;
using System;

namespace NBi.Testing.Core.Scalar.Resolver.Resources
{
    public class MyCustomClass : IScalarResolver
    {
        private int Foo { get; }
        private DateTime Bar { get; }

        public MyCustomClass(DateTime bar, int foo)
            => (Bar, Foo) = (bar, foo);

        public object Execute() => Bar.AddDays(Foo);
    }
}
{% endhighlight %}

## Usage

You can't use the variables at all places. The usage is limited to the following places:

* [Parameter](../query-parameter)* (of a query)
* In the [comparisons (*equal*, *more-than*, *less-than*)](../resultset-rows-count) for a *row-count*
* In the operators of [predicates](../resultset-predicate) of the assertions *row-count*, *all-rows*, *no-rows*, *some-rows* and *single-row*
* In the [empty](../primitive-result-set/#empty) and [flat file](../primitive-result-set/#external-definition) definitions of a result-set
* In the [json-source](../primitive-result-set/#json-source) and [xml-source](../primitive-result-set/#xml-source) elements
* In the [rest-api](../primitive-result-set/#rest-api) attributes
* In the [renaming](../resultset-alteration/#renamings) alteration
* In the [data engineering](../setup-data-engineering/), [IO](../setup-io/) and [process](../setup-process/) tasks
* In the [list](../primitive-sequence/#list-of-values) elements of a sequence
* In [native transformations](../scalar-native-transformation/)
* In the definition of [custom scalars](#custom-scalar) and [custom sequences](../primitive-sequence/#custom-sequence/)

If you've other places, where you think that a variable would be helpful, report it by creating an [issue](http://github.com/Seddryck/nbi/issues)

## Notes about the future of variables

Variables will be extented in the next releases. The goal is to let you define them at different places (*groups* and perhaps *tests*) and use them at more places.

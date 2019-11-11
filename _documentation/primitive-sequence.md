---
layout: documentation
title: Sequence
prev_section: primitive-scalar
next_section: primitive-result-set
permalink: /docs/primitive-sequence/
---
One-dimensional array. All the values contained in a sequence are [scalars](../primitive-scalar) of the same type. A sequence contains a finite number of element but can also contain zero element.

## Definition

To specify a sequence, you can use any of the following options:

### List of values

The easiest way to define a sequence is to specify each member. This can be done using the *item* element one or more times.

{% highlight xml %}
<sequence type="text">
  <item>be</item>
  <item>fr</item>
</sequence>
{% endhighlight %}

### Sentinel loops

Usage of a *loop-sentinel* element is targetting the definition of sequence containing elements developed one by one with the help of a recursive calculation. The first element is defined by the *seed* attribute and is always returned by the loop (meaning that a loop-sentinel has a minimum of one element). The next element of the sequence is calculated based on the seed and the addition of the *step* attribute. The third element will be calculated based on the second element and the step. The sequence is over when the next element is greater than the *terminal* attribute.

{% highlight xml %}
<sequence type="dateTime">
    <loop-sentinel seed="2015-01-01" terminal="2017-01-01" step="1 year"/>
</sequence>
{% endhighlight %}

Expl:

* A sequence having for seed the value 1, for step the value 1 and for terminal the value 5 will have 5 elements having for values 1,2,3,4,5.
* A sequence having for seed the value 1 for step the value 2, and for terminal the value 5 will have 3 elements having for values 1,3,5.
* A sequence having for seed the value 1 for step the value 3, and for terminal the value 5 will have 2 elements having for values 1 and 4.

Due to its definition, a loop-sentinel can only be used for the definition of a sequence containing a *numeric* type or a *dateTime* type.

For a *numeric* type, the *seed*, *step* and *terminal* attributes are defined as *numeric* scalars. In the case of a sequence containing scalar with a *dateTime* type, then the attributes *seed* and *terminal* are *dateTime* but the *step* is a *duration*.

#### Half-open

It's possible to define a sentinel loop where the terminal value shouldn't be included in the sequence. This is done by setting the xml attribute *interval* to the value *half-open*.

Expl:

* A sequence, with the interval set to half-open, having a seed of 1 and a step of 1 and a terminal of 5 will have 4 elements having for values 1,2,3 and 4.
* A sequence, with the interval set to half-open, having a seed of 1 and a step of 2 and a terminal of 5 will have 2 elements having for values 1 and 3.
* A sequence, with the interval set to half-open, having a seed of 1 and a step of 3 and a terminal of 5 will have 2 elements having for values 1 and 4.

### File loops

It's possible to define a file loop to get the name of all the files within a directory and for which the filename matches a given pattern. The elements of the sequence returned by a *loop-file* are composed of the filename and the extension.

{% highlight xml %}
<loop-file path="..\csv\" pattern="MyData*.csv"/>
{% endhighlight %}

### Custom sequence

This solution retrieves the values from an external C# assembly. This assembly must contain one or more types implementing the interface *ISequenceResolver*.

In this example, the sequence named *myVar* is set to the values returned by the type *MyCustomClass* of the assembly *myassembly.dll* when executing the method *Execute()* . Optionaly, you can pass some parameters to the type *MyType* when instantiating it. In this example, the class *MyCustomClass* has a constructor accepting two parameters (*foo*, *bar*).

{% highlight xml %}
<sequence name="myVar"/>
  <custom assembly="myAssembly.dll" type="MyCustomClass">
    <parameter name="bar">10</parameter>
    <parameter name="foo">@myValue</parameter>
  </custom>
</sequence>
{% endhighlight %}

{% highlight csharp %}
using NBi.Core.Scalar.Resolver;
using System;

namespace NBi.Testing.Core.Scalar.Resolver.Resources
{
    public class MyCustomClass : ISequenceResolver
    {
      private int Foo { get; }
      private DateTime Bar { get; }

      public MyCustomClass(DateTime bar, int foo)
        => (Bar, Foo) = (bar, foo);

      public IList Execute() 
        => new DateTime[] { Bar.AddDays(-Foo), Bar.AddDays(Foo) }.ToList();

      object IResolver.Execute() => Execute();
    }
}
{% endhighlight %}

## Filtering

From time to times, it's useful to perform an additional filtering to remove a few values. Typically when using *loop-file*, you'll remove files with a size less than a few bytes or updated more than 10 days ago.

To achieve this, you'll define a *filter* implementing a [predicate](../resultset-predicate/#list-of-predicates
). The predicate will be applied to each of the values contained in the sequence. Each value will be hold if the predicate is successful and will be removed in case of failure to validate the predicate.

The attribute *operand* is containing the native transformations that will be applied to each value of the sequence. If no transformation is expected, you must specify the value *value* for the *operand*.

In the example here under, the *loop-file* will define all the files matching the pattern *MyData*.csv* in the directory *..\csv*. Then a filter will be applied to only hold the files that have been updated less than 10 days ago (defined in a variable *@TenDaysAgo*). Keep in mind that *loo-file* doesn't return the full path but just the filename with the extension so you'll have to rebuild the full path before applying a native transformation such as *file-to-update-dateTime*. Note also that the predicate is of type *dateTime* in that case and not *text*. This is because the predicate will compare two *dateTime* and not a *dateTime* and a *text*.

{% highlight xml %}
<sequence name="myVar"/>
  <loop-file path="..\csv\" pattern="MyData*.csv"/>
  <filter>
    <predicate 
       operand="text-to-prefix(..\csv\) | file-to-update-dateTime"
       type="dateTime"
    >
      <more-than>@TenDaysAgo<more-than>
    </predicate>
  </filter>
</sequence>
{% endhighlight %}

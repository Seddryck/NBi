---
layout: documentation
title: Scalar
prev_section: resultset-format
next_section: primitive-sequence
permalink: /docs/primitive-scalar/
---
A scalar is the most atomic object considered by NBi. A scalar is equivalent to an atomic value. A scalar has a type that can be defined as *text*, *numeric*, *dateTime* or *boolean*. The default type of a scalar is *text*.

## Supported types

### Text

This is the most common type for a scalar. If no type is specified then this type is effectively used. The exact content of the cell is used during comparison. It means that values “10.0”, “010” and “10” are considered as different when using the type *text*. This is usually what you’ll use when specifying *Key* columns. Pay attention that the default comparison for this type is case-sensitive.

### Numeric

To avoid comparison of textual content, you can use the *numeric* type. The content of the cell is first converted to a numeric (decimal) value using the international format (a dot to separate the decimal part). It means that values *10.0*, *010* and *10* are considered as equal when using this type. This type is useful when you’ve *Value* columns.

### Date and time

The content of the cell is first converted to a DateTime value using the international format (yyyy-mm-dd hh:mm:ss).

### Boolean

The content of the cell is first converted to a boolean value. NBi understands “0” or “false” and “1” or “true” as boolean values.

## Definitions

You've many options to define a scalar but not all of them are supported everywhere. Refer to the documentation of the different xml elements and attributes to know which kind of definition are supported where. Only the most common (and basic) options are enlisted here, others are detailled where they are available.

### Literal

This is the most straightforward way to define a scalar, just input its value.

{% highlight xml %}
<parameter name="myParam">145</parameter>
{% endhighlight %}

Keep into account that the value should be castable to the type expected by the parameter. It means that *dates* should have the internaltional format and *numerics* should have a dot to separate the decimal parts. 

### Reference to a variable

The value of the scalar is identical the value of the variable. It could be a global-variable or an instance-variable.

{% highlight xml %}
<parameter name="myParam">@myVar</parameter>
{% endhighlight %}

#### Inline transformations

From time to time, you'll need to use a variable and slightly transform it to get what you really want. Defining additional variables, supporting these transformations, has a negative impact on readiness of your test-suite. You can achieve the same result with inline transformations. The list of transformations supported is defined at [this page](../transform-column#Native).

Inline transformations make usage of a pipe ```|``` to list them. They are applied from left to right and the result of the previous evaluation is always the input of the next evaluation.

{% highlight xml %}
<parameter name="myParam">@myVar | dateTime-to-first-of-month</parameter>
{% endhighlight %}

In the case above, the variable *@myVar* will be transformed using the native transformation *dateTime-to-first-of-month*. If the variable *@myVar* is not a valid date, it will raise an exception.

#### Formatting

A *text* scalar can be dynamically evaluated based on one or several variables and some literal parts. To enable this feature, you must precede the sclara value by a tilt ```~``` and mix static parts of the filename with dynamic parts. The dynamic parts must be contained between curly barces ```{}``` and start by the variable's name to consider.

{% highlight xml %}
<parameter name="myParam">~File_{@myDate}.csv</parameter>
{% endhighlight %}

Using the previous notation, if the value of *myDate* is *25th October 2018* then the filename *File_2018.csv* will be considered for loading the result-set.

In case the variable is a numeric or dateTime, it can be useful to format it. This formatting must be specified after a column (```:```).

{% highlight xml %}
<parameter name="myParam">~File_{@myDate:yyyy}_{@myDate:MM}.csv</parameter>
{% endhighlight %}

Using the previous notation, if the value of *myDate* is *25th October 2018* then the filename *File_2018_10.csv* will be considered for loading the result-set.

The formatting syntax is the one supported by .Net and explained in MSDN for the [numerics](https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-numeric-format-strings) and [dateTimes](https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
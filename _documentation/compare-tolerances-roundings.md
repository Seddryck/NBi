---
layout: documentation
title: Tolerances and roundings
prev_section: compare-special-generic-values
next_section: compare-intervals
permalink: /docs/compare-tolerances-roundings/
---
## Tolerances
A tolerance can only be assigned to column defined as a *value* (meaning that a tolerance can't be applied to *key* columns) and, for the moment, is only supported for column defined with types *numeric* or *dateTime*.

### Absolute tolerance
If you apply a tolerance of 0.001 to a numeric column, two rows (with same keys) will be considered as equal if the absolute difference between them is less or equal to 0.001.

This is especially helpful in Business Intelligence when you need to compare two queries and you don’t bother about small differences. Another case in Business Intelligence is also when you customers give you some hints about values expected in reports, they are usually a bit imprecise.

{% highlight xml %}
<column  index="2" tolerance="0.001" />
{% endhighlight %}

It’s also possible to take some short-cuts and write directly in the *equalTo* tag the columns’ definition. This xml element has the following meaning: all columns are keys except the last one. This last column is a value column where a tolerance of 0.001 must applied.
{% highlight xml %}
<equalTo values="last" tolerance="0.001">
{% endhighlight %}

If you want you can combine both notations. The definition provided in an xml element named column has always the precedence to the tolerance provided the xml element named *equalTo*. It means that following notation (for a result-set with 6 columns):
{% highlight xml %}
<equalTo keys="all-except-last" tolerance="10">
	<column  index="2" role="ignore" />
	<column  index="3" tolerance="0.001" />
</equalTo>
{% endhighlight %}
must be interpreted as: columns 0, 1 and 4 are keys, column 2 must be ignored, column 3 is a value with a tolerance of 0.001 and column 5 is also a value but with a tolerance of 10.

### Tolerance for type "date and time"
If you want to specify the tolerance for *dateTime* columns, you must express the tolerance in days, hours, minutes, seconds and milliseconds. The correct syntax for two days and an half is
{% highlight xml %}
<column  index="3" role="value" type="dateTime" tolerance="2.12:00:00" />
{% endhighlight %}
If the tolerance must be set to 15 minutes, you will write:
{% highlight xml %}
<column  index="3" role="value" type="dateTime" tolerance="00:15:00" />
{% endhighlight %}
Reminder, tolerances are only applied to *value* columns and never to *key* columns!

### Relative tolerance
You can express a tolerance, relative to the expected value, by the means of the *%* symbol. This only applies to *numeric* values. When comparing the expected and actual value, the comparer will apply a tolerance of the percentage defined.

To illustrate this, if you've two rows with values of 40 and 100, an absolute tolerance of 10 will allow the actual values to be respectively in the intervals [30;50] and [90;110]. A relative tolerance of 10% will change the intervals to [36;44] and [90;110].

The xml syntax requires a % in the attribute *tolerance*.
{% highlight xml %}
<column  index="3" role="value" type="numeric" tolerance="10%" />
{% endhighlight %}

### Absolute and relative tolerance
Sometimes, you want to use a relative tolerance but you also want to bound thsi tolerance and express that this tolerance cannot be more (or less) than a specific value. It's possible to achieve this by specifying the value in percentage and also the bound (min or max) between brackets for the xml attribute *tolerance*.

{% highlight xml %}
<column  index="3" role="value" type="numeric" tolerance="10% (min 0.001)" />
{% endhighlight %}

### One-sided tolerance
Sometimes, you want to specify that the tolerance should only be applied on the right or on the left of the expected value. In this case, you must specify the symbols *+* or *-* before the percentage of the absolute value.

{% highlight xml %}
<column  index="3" role="value" type="numeric" tolerance="+10%" />
{% endhighlight %}

### Tolerance for type "text"
You can define a tolerance for a text. Following algorithms are supported:
* Hamming Distance
* Jaccard Distance
* Jaro Distance
* Jaro-Winkler Distance
* Levenshtein Distance
* Longest Common Subsequence
* Longest Common Substring
* Overlap Coefficient
* Ratcliff-Obershelp Similarity
* Sorensen-Dice Distance
* Tanimoto Coefficient

Use the attribute *tolerance* and specify the name of the algorithm and the thershold value.
{% highlight xml %}
<column index="1" role="value" type="text" tolerance="Levenshtein(5)"/>
{% endhighlight %}

## Roundings
The roundings are another set of tools to express that two values are equal if they are close to each other. At the opposite of *tolerance*,  *rounding* is applied to both expected and actual values. If after the rounding's operation, the two values are strictly equal then the comparison will be positive (and else negative).

The roundings are related to .Net methods [Round](http://msdn.microsoft.com/en-us/library/wyk4d9cy.aspx), [Floor](http://msdn.microsoft.com/en-us/library/e0b5f0xb.aspx) and [Ceiling](http://msdn.microsoft.com/en-us/library/zx4t0t48.aspx). The rounding rules will be the same that their corresponding equivalent in .Net.

The method used must be specified in the column xml definition by the means of values: round, ceiling and floor in the attribute *rounding-style*.
{% highlight xml %}
<column index="3" role="value" rounding-style="floor" ... />
{% endhighlight %}

### Numeric columns
Nevertheless, the rounding methods are different in same points to their .Net equivalent. Each of them require a _step_. If the step is less than 1, the rounding will be applied to the decimal part of the value.

**Example 1**: For a value of 10.52912 with a step of 0.1, the rounding will return

* with a _floor_ style : 10.5  
* with a _round_ style : 10.5
* with a _ceiling_ style : 10.6

**Example 2**: For a value of 10.52912 with a step of 0.01, the rounding will return

* with a _floor_ style : 10.52  
* with a _round_ style : 10.53
* with a _ceiling_ style : 10.53

If the step is greater than 1, the rounding will be applied to the integer part of the value.

**Example 3**: For a value of 10529.12 with a step of 10, the rounding will return

* with a _floor_ style : 10520  
* with a _round_ style : 10530
* with a _ceiling_ style : 10530

**Example 4**: For a value of 10529.12 with a step of 20, the rounding will return

* with a _floor_ style : 10520  
* with a _round_ style : 10520
* with a _ceiling_ style : 10540

{% highlight xml %}
<column  index="3" role="value" rounding-style="floor" rounding-step="20" />
{% endhighlight %}

### Date and time columns
It's also possible to apply the same kind of roundings for dateTime columns. The rounding must be less than or equal to one day.

**Example 5**: For a value of 2013-10-17 14:47:00 with a step of "1" (day), the rounding will return

* with a _floor_ style : 2013-10-17 00:00:00  
* with a _round_ style : 2013-10-18 00:00:00
* with a _ceiling_ style : 2013-10-18 00:00:00

**Example 6**: For a value of 2013-10-17 14:47:00 with a step of "00:15:00" (15 minutes), the rounding will return

* with a _floor_ style : 2013-10-17 14:45:00
* with a _round_ style : 2013-10-17 14:45:00
* with a _ceiling_ style : 2013-10-17 15:00:00

---
layout: documentation
title: Special and generic values
prev_section: compare-configuration
next_section: compare-tolerances-roundings
permalink: /docs/compare-special-generic-values/
---
## Special values
When comparing two result-sets, you have to compare some special values such as *null*, *empty strings* or *true/false*. NBi uses alias to understand these values from Csv files or from embedded result-sets.

### Null
If you want to specify a cell with a **null value**, simply write an empty tag or specify (null) in your tag.
{% highlight xml %}
<cell/>
or
<cell></cell>
or
<cell>(null)</cell>
{% endhighlight %}

### Empty string
If you want to specify that **an empty value** (string with length of 0 characters) will be a valid result, you can specify "(empty)" in your xml element.
{% highlight xml %}
<cell>(empty)</cell>
{% endhighlight %}

### Boolean values
The values *true*, *false*, *yes* and *no* are valid to express a boolean. Case is not sensitive, meaning that *True* or *YES* are also valid values.
{% highlight xml %}
<cell>true</cell>
<cell>false</cell>
<cell>yes</cell>
<cell>no</cell>
{% endhighlight %}

## Generic values
A generic value is a substitute for a set of values. With generic values, NBi offers some flexibility to compare your result-set's cells to a range of values.

### Not null
If you want to specify that **any non-null value** will be a valid result, you can specify "(value)" in your xml element
{% highlight xml %}
<cell>(value)</cell>
{% endhighlight %}

### Null or not null
If you want to specify that **any value** will be a valid result, you can specify "(any)" in your xml element
{% highlight xml %}
<cell>(any)</cell>
{% endhighlight %}

If you want to go further with imprecise values, you should also read the documentation for [intervals].

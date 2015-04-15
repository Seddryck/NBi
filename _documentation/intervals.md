---
layout: documentation
title: Intervals of values
prev_section: tolerances-roundings
next_section: query-syntax
permalink: /docs/intervals/
---
For the moment the intervals are only defined for numerical values (not for date and time values).

An interval can only be assigned to a cell belonging to a column defined as a value (so an interval can't be assigned to key columns).

An interval is a range of possible values for a *cell*. If you know that the value of you cell for Calendar Year 2006 must be somewhere between 100 and 500, you can specify it in NBi with the following syntax:

{% highlight xml %}
<row>
	<cell>CY 2006</cell>
	<cell>[100;500]</cell>
</row>
{% endhighlight %}

You can use open or closed left and right bounds to specify if the bounds are contained or not in your interval. The sample here under assigns a range starting just after 100 and ending at 500 (included).

{% highlight xml %}
<row>
	<cell>CY 2006</cell>
	<cell>]100;500]</cell>
</row>
{% endhighlight %}

## Infinite

Some intervals cover all the values greater than one million or lower than -200. In this case, the lower (or upper) bound is defined as the positive or negative infinite. To represent an infinite value in NBi, you can use the syntax *-INF* or *+INF*

Following intervals are valid:

{% highlight xml %}
<row>
	<cell>CY 2006</cell>
	<cell>[1000000;+INF]</cell>
</row>

<row>
	<cell>CY 2006</cell>
	<cell>[-INF;-200]</cell>
</row>
{% endhighlight %}

Note that for infinite the notion of open or closed bound is not relevant.

## Special intervals

For readability of your test it's sometimes useful to use a more readable syntax to define a few specific intervals. Following short-cuts are understood by NBi

| Description | Standard interval | literal short-cut | numeric short-cut
|-------------|:-----------------:|-------------------|:----------------:|
| Greater than 0  | ]0;+INF] | (absolutely-positive) | (+)
| Greater or equal to 0 | [0;+INF] | (positive) | (0+)
| Less than 0 | [-INF;0[ | (absolutely-negative) | (-)
| Less or equal to 0 | [-INF;0] | (negative) | (-0)

In an embedded result-set or an external CSV file, you can freely mix the previous notations.
{% highlight xml %}
<row>
	<cell>CY 2006</cell>
	<cell>(positive)</cell>
</row>
<row>
	<cell>CY 2007</cell>
	<cell>(0+)</cell>
</row>
<row>
	<cell>CY 2008</cell>
	<cell>[0;+INF]</cell>
</row>
{% endhighlight %}

You can also avoid the usage of the *-INF* or *+INF* by using the short-cut version making usage of symbols greater and less than.

| Description | numeric short-cut
|-------------|:-----------------:|
| Greater than 500  | (>500)
| Greater or equal to 500 | (>=500)
| Less than 500 | (<500)
| Less or equal to 5000 | (<=500)

The two notations bellow are identical to represent all values greater than 500.

{% highlight xml %}
<row>
	<cell>CY 2006</cell>
	<cell>]500;+INF]</cell>
</row>
<row>
	<cell>CY 2006</cell>
	<cell>(>500)</cell>
</row>
{% endhighlight %}

Usage of following symbols (between brackets) is also understood by NBi: < , <= , > , >= . Note that for symbols < , <= you'll need to make usage of the CDATA container to avoid misunderstanding with an opening xml element.

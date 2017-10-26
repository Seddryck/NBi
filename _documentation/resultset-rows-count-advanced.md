---
layout: documentation
title: Rows' count (advanced)
prev_section: resultset-rows-count
next_section: resultset-rows-uniqueness
permalink: /docs/resultset-rows-count-advanced/
---
It's possible to go further with the assertion row-count and define a filter. Suppose that you know that you've made less than 100 sales with a *TotalAmountWithVAT* higher than 1000$. To assert this create a query returning all your sales, define a filter to hold the rows with a  *TotalAmountWithVAT* greater than 1000 and assert that the count is not greater than 100. You could also have asserted that this set of rows represent less than 1% of your sales. All these scenarios are supported by *row-count*.

## Filter

To define a filter, you must specify it in the element *row-count* by the means of the tags *filter*.

{% highlight xml %}
<assert>
  <row-count>
    <filter>
      ...
    </filter>
    ...
  </row-count>
</assert>
{% endhighlight %}

This filter is defined by the means of *variables*, *expressions* and *predicates*. For more info about these elements check the page about [them](../resultset-all-no-rows/).

{% highlight xml %}
<assert>
  <row-count>
    <filter>
      <variable column-index="1">Quantity</variable>
      <variable column-index="2">UnitPrice</variable>
      <expression name="TotalPriceWithVAT">UnitPrice*Quantity*1.21</variable>
      <predicate name="TotalPriceWithVAT">
         <more-than or-equal="true">1000<less-than>
      <predicate>
    </filter>
    ...
  </row-count>
</assert>
{% endhighlight %}

## Fixed value and percentage

It's possible to use the standard behaviour of row-count and compare the remaining rows (after filter's application). To achieve this, just specify the expected values in the xml element corresponding to one of the following operators *equal*, *more-than*, *less-than* (the two last element accepts the overload of the attribute *or-equal*).

{% highlight xml %}
<assert>
  <row-count>
    <filter>
      ...
    </filter>
    <more-than or-equal="true">10<more-than>
  </row-count>
</assert>
{% endhighlight %}

If you want to assert the ratio of filtered rows, you can specify percentage. The count of filtered rows will be divided by the count of rows in the unfiltered result-set. To specify this kind of behaviour, you need to add the symbol percentage (%) after the value in the operator tag.

{% highlight xml %}
<assert>
  <row-count>
    <filter>
      ...
    </filter>
    <less-than>25%<less-than>
  </row-count>
</assert>
{% endhighlight %}

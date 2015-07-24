---
layout: documentation
title: Rows' count for a result-set
prev_section: compare-intervals
next_section: resultset-format
permalink: /docs/resultset-rows-count/
---
This assertion counts rows of a result-set and compares it to an expectation. The possible comparisons are equal, _equal_, _more-than_ and _less-than_.

## System under test

The system-under-test is a query, please reports to other tests for more information about how to define a query. More info [here](/docs/compare-equivalence-resultsets).

## Assertion

The assertion consists in an xml element named row-count.
{% highlight xml %}
<assert>
  <row-count>
    ...
  </row-count>
</assert>
{% endhighlight %}

In this element, you must also specify an the comparison that you want to apply _equal_, _more-than_ and _less-than_. You must also specify the reference to compare. Currently NBi only supports a fixed value.
{% highlight xml %}
<assert>
    <row-count>
         <less-than>10</less-than>
    </row-count>
</assert>
{% endhighlight %}

For the comparisons _more-than_ and _less-than_, you can slightly change the behavior by adding the attribute or-equal and setting its value to true.
{% highlight xml %}
<assert>
    <row-count>
         <less-than or-equal="true">10</less-than>
    </row-count>
</assert>
{% endhighlight %}

## Full example
{% highlight xml %}
<test name="Count of SalesTerritory is less-than or equal to 3" uid="0001">
   <system-under-test>
      <execution>
        <query connectionString="Data Source=mhknbn2kdz.database.windows.net;Initial Catalog=AdventureWorks2012;User Id=sqlfamily;password=sqlf@m1ly">
          select
            [Name], [CountryRegionCode]
          from
            [Sales].[SalesTerritory]
          where
            [Group]='Europe'
        </query>
      </execution>
   </system-under-test>
   <assert>
      <row-count>
        <less-than or-equal="true">3</less-than>
      </row-count>
   </assert>
</test>
{% endhighlight %}

---
layout: documentation
title: Rows' count for a result-set
prev_section: resultset-all-no-rows
next_section: resultset-rows-count-advanced
permalink: /docs/resultset-rows-count/
---
This assertion counts rows of a result-set and compares it to an expectation. The possible comparisons are equal, _equal_, _more-than_ and _less-than_.

## System under test

The system-under-test is a query, please reports to other tests for more information about how to define a query. More info [here](/docs/compare-equivalence-resultsets).

## Assertion

### Predefined value

The assertion consists in an xml element named row-count.
{% highlight xml %}
<assert>
  <row-count>
  ...
  </row-count>
</assert>
{% endhighlight %}

In this element, you must also specify an the comparison that you want to apply _equal_, _more-than_ and _less-than_. You must also specify the reference to compare.
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

### Variable

It's possible to dynamically define the value that is used in the comparison. To achieve this you must use a global [variable](../docs/variable-define) as explained [there](../docs/resultset-predicate/#variables-for-predicates-reference).

{% highlight xml %}
<variables>
   <variable name="maximum">
     <script language="c-sharp">10*10*10</script>
   </variable>
</variables>

<assert>
  <row-count>
    <less-than or-equal="true">@maximum</less-than>
  </row-count>
</assert>
{% endhighlight %}

Note that if this variable is a percentage it must be returned as a string (double quotes)!

{% highlight xml %}
<variables>
   <variable name="maximum">
     <script language="c-sharp">"50%"</script>
   </variable>
</variables>
{% endhighlight %}

### Row-count of another result-set

It's possible to compare the row-count of the first result-set defined in the system-under-test to the row-count of a second result-set defined in the assertion to achieve this, use the projection *row-count* and define your second result-set with the [syntax 2.0](../docs/syntax-2-0).

{% highlight xml %}
<row-count>
  <more-than or-equal="true">
    <projection type="row-count">
      <result-set>
        <query connection-string="@conn-Other">
          <![CDATA[
           select Age, *
           from Employee
           where Age=50
          ]]>
        </query>
      </result-set>
    </projection>
  <more-than>
</row-count>
{% endhighlight %}

## Full example
{% highlight xml %}
<test name="Count of SalesTerritory is less-than or equal to 3" uid="0001">
   <system-under-test>
    <result-set>
      <query connection-string="...">
        select
          [Name], [CountryRegionCode]
        from
          [Sales].[SalesTerritory]
        where
          [Group]='Europe'
      </query>
    </result-set>
   </system-under-test>
   <assert>
    <row-count>
      <less-than or-equal="true">3</less-than>
    </row-count>
   </assert>
</test>
{% endhighlight %}

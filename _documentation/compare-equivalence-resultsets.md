---
layout: documentation
title: Equivalence of two result-sets
prev_section: run-alterative
next_section: compare-configuration
permalink: /docs/compare-equivalence-resultsets/
---

NBi is able to execute **SQL**, **MDX** or **DAX** queries and capture the result-set to compare it with another resource. For this kind of test, you'll need to define your system-under-test as a *query* and your assertion as an *equalTo* constraint. You've the choice to assert the result-set of your query against three types of resources:

* An embedded result-set
* An external csv file
* Another query (or the same query on another connection)

During test's execution, NBi will compare the two result-sets and provide a brief analysis if they differe. To guide NBi during this comparison, you've the possibility to specify the role played by each column and the tolerance that you'll allow on them.

## system-under-test

Let's start with the definition of your system-under-test:

You must create an xml element named *execution* under the element *system-under-test*. Inside this element *execution*, you must create another xml element named *query*. You can specify a connection string to this query or rely on the [default connection string](/docs/config-connection-strings).
{% highlight xml %}
<system-under-test>
  <execution>
    <query connectionString="..."/>
  </execution>
</system-under-test>
{% endhighlight %}

This query can be specified directly inside the test suite.
{% highlight xml %}
<query connectionString="...">
<![CDATA[
  SELECT
    {[Measure].[MyMeasure]} ON 0,
    {[MyDimension].[MyHierarchy].Members} ON 1
  FROM
    MyCube
]]>
</query>
{% endhighlight %}
Usage of tags <![CDATA[ and ]]> is not mandatory but highly recommended. This allows you to write the characters *&*, *<* and *>* in your query with no need to translate them in their xml equivalent (&amp;amp; &amp;lt; and &amp;gt;)

An alternative is to reference your query from an **external file**
{% highlight xml %}
<query file="C:\myFile.sql" connectionString="..."/>
{% endhighlight %}
## Assert
Once your system-under-test is defined, you'll need to specify what you want to assert. In this case, you'll want to compare your system-under-test with another result-set and check the equivalence of the two result-sets. This done by specifying an xml element _equalTo_:
{% highlight xml %}
<assert>
  <equalTo />
</assert>
{% endhighlight %}

### Embedded result-set

The easiest way to define a result-set is to define it in the test. You must create a new xml element *row* for each row of the result-set. For each row, you must specify the expected values in different xml elements named *cell*.
{% highlight xml %}
<equalTo>
  <resultSet>
    <row><cell>First Member's value</cell><cell>100.05</cell></row>
    <row><cell>Last Member's value</cell><cell>77.7</cell></row>
  </resultSet>
</equalTo>
{% endhighlight %}

### External CSV file

You also have the opportunity to specify that a result-set is defined in an external file (useful for large result-sets)
{% highlight xml %}
<equalTo>
  <resultSet file="C:\myResult.csv">
</equalTo>
{% endhighlight %}
If needed, you can also specify an alternative [CSV profile](/docs/config-profile-csv) in the settings. Note that for the embedded result-set and for the external result-set, the *numeric values* must be written with an international format (a dot (".") to separate the integer part of the decimal part).

### Another query

Finally, the third choice is to compare the result-set of the system-under-test to the result-set of another query. This can be useful to ensure a non-regression between two systems or to compare the data warehouse data and the corresponding olap data. To do this, the expression here under must be applied.
{% highlight xml %}
<equalTo>
  <query connectionString="...">
    SELECT MyHierarchy, MyMeasure FROM MyTable
  </query>
</equalTo>
{% endhighlight %}

## Advanced features

With NBi, you can also check a cell's value against special values such as *null*, *empty* or *any value*. This is detailed in the article about [special and generic values](/docs/compare-special-generic-values/).

By default, NBi will take the assumption that the first column is a key (text) and the other columns are values (numeric without tolerance). This will influence the comparison's result. If you want to override this configuration, you should read the documentation about [result-set's comparison configuration](/docs/compare-configuration/).

NBi is also able to manage dateTime and boolean formats without tricks in SQL queries, more info at the [same page](/docs/compare-configuration/)

The comparison can also include [tolerances and roundings](/docs/compare-tolerances-roundings/) methods when comparing numeric and dateTime types.

## Improve performance with queries' parallelization

By default, NBi will run the queries (system-under-test and assertion) in parallel. This usually improves the performances because the two queries are usually on different systems.

When parallelization is activated, this directly influences the output in the console or the output tab. The two result-sets will be intermixed and probably not interpretable. Note that this output is only visible when the trace level is set to 4 (see [enable and display trace messages](/docs/config-traces-debugging/)). The output of the two result-sets could be mixed. This will not influence the test execution but your debugging experience can suffer. We recommend to desactivate the queries' parallelization when the trace level is set to 4.

If you want to desactivate this feature, you must specify it in the _settings_ of your test-suite and it will affect the whole test-suite.
{% highlight xml %}
<settings>
    <parallelize-queries>false</parallelize-queries>
</settings>
{% endhighlight %}

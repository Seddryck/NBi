---
layout: documentation
title: Performance
prev_section: query-syntax
next_section: query-timeout
permalink: /docs/query-performance/
---
For this kind of test, you'll need to define your system under test as a *query* and your assertion as a *fasterThan* constraint.

NBi will **effectively execute** your query. It means that if your query is a truncate or an insert, this will be executed and the content of your database will be updated. No transaction will be roll-backed or anything.

The query will also be executed until its end. It means that if your query was expected to run faster than 10 seconds but takes effectively more than 20 seconds, your test will take 20 seconds to execute (and not 10). Nevertheless, you can override this behavior by using a [timeout](#timeout)

This kind of test is surely no ambition to replace *load tests* or *Stress tests*. With this test, you’ll only test the performances of a unique query in isolation. The goal is really to ensure that this query is not suddenly become really slow (because of a missing index, changed in the underlying objects, ...)

## System Under Test

If you’re not familiar with how to setup a *query* as a *system-under-test*, please read first the documentation about [equivalence of two result-sets](../compare-equivalence-resultsets/)

## Assert

Once your *system-under-test* is defined, you'll need to specify that you want to assert the performances of your query. This is done through the xml element named *fasterThan* . You must specify the maximum time for this query in **milli-seconds** in the xml attribute *max-time-milliSeconds*.

The following exemple illustrates a constraint defined to 1 second.

{% highlight xml %}
<assert>
	<fasterThan max-time-milliSeconds="1000"/>
</assert>
{% endhighlight %}

A full test, would be:

{% highlight xml %}
<test name="A fast MDX query">
    <system-under-test>
        <execution>
            <query connectionString="...">
              SELECT
                [Measures].[Reseller Order Count] ON 0,
              EXCEPT(
                {[Date].[Calendar Year].Children}
                ,{[Date].[Calendar Year].[CY 2006]}
              ) ON 1
            FROM
              [Adventure Works]
            </query>
        </execution>
     </system-under-test>
     <assert>
        <fasterThan max-time-milliSeconds="1000"/>
     </assert>
</test>
{% endhighlight %}

### Clean the cache

Until version 1.13, this feature was only available for SQL queries. Since  1.13-beta-2, NBi is supporting the cleaning of the cache for MDX/DAX queries.

It’s possible to specify that the cache must be cleaned before the execution of the test. The time needed to clean the cache is not included in the measurement of your query’s performance but, on the other hand, the time elapsed during the creation of the execution plan is included in the performances' measurement.

### Timeout

By default, the query will continue to execute even after the failure of the test. If you've created a test with a *fasterThan* constraint set to 1000ms and your query is lasting more than 5000ms, your test will be running during 5s and report that your expectation was 1000ms but the actual was 5000ms.
This can be really embarrasing when you've some queries much slower than expected. Imagine that some of your queries last during more than 20 minutes when you was expecting less than 20 seconds? These failures will considerably slow down your whole test-suite. With the attribute *timeOut-MilliSeconds*, you stop query's execution after a specified elapsed time using the attribute. The test will be reported has failed as soon as the timeout is triggered and your test-suite execute faster.

{% highlight xml %}
<test name="A fast MDX query">
    <system-under-test>
        <execution>
            <query name="MDX" connectionString="...">
                SELECT
                    [Measures].[Reseller Order Count] ON 0,
                    EXCEPT(
                      {[Date].[Calendar Year].Children}
                      ,{[Date].[Calendar Year].[CY 2006]}
                    ) ON 1
                FROM
                    [Adventure Works]
            </query>
        </execution>
     </system-under-test>
     <assert>
        <fasterThan
            max-time-milliSeconds="1000"
            timeOut-MilliSeconds="5000"
        />
     </assert>
</test>
{% endhighlight %}

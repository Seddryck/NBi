---
layout: documentation
title: All/no rows validate a predicate
prev_section: connection-roles
next_section: resultset-predicate
permalink: /docs/resultset-all-no-rows/
---
This feature is intended to support the assertion that all rows (or no rows) of a result-set validate a predicate. For example, you want to test that a query effectively returns all the sales with a cost higher or equal to 1000$. A check could be that the value returned by the query for *TotalAmountWithVAT* is greater or equal than 1000 for each row!

For more complex testing conditions than all/no rows, check the [advanced row-count](../resultset-rows-count-advanced/) page.

## System under test

The system-under-test is a query, please reports to other tests for more information about how to define a query. More info [here](/docs/compare-equivalence-resultsets).

## Assertion

To specify this kind of test, you need to define an assertion with the xml elements *all-rows* or *no-rows*. *all-rows* will passes the test only if all the rows of the result-set validate the predicate, if at least one row doesn't validate the predicate, the test will fail. At the opposite, *no-rows* will fail if at least one row validate the predicate (and succeed in the other case). If the result-set is empty, both assertions will succeed.

{% highlight xml %}
<assertion>
    <all-rows>
        ...
    </all-rows>
</assertion>
{% endhighlight %}
or
{% highlight xml %}
<assertion>
    <no-rows>
        ...
    </no-rows>
</assertion>
{% endhighlight %}



The predicates are explained at [this page](../resultset-predicate/)

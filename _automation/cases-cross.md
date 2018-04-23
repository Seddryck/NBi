---
layout: automation
title: Cross two sets of test-cases to combine them
prev_section: cases-merge
next_section: column-add
permalink: /automation/cases-cross/
---
This action lets you enrich a set of test-cases by combining it with another one. You should compare this action to a *cross join* or a *inner join* in SQL.

## Full Cross

A full cross will execute a cartesian product (a *cross* join in SQL). It means that if your first set has 5 rows and the second set has 3 rows, the result of this action will be a set of 15 (5x3) rows.

Sample:
{% highlight xml %}
case cross 'first set' with 'second set';
{% endhighlight %}

## Cross with jointure on a column

A *cross on column* will select all rows from both sets as long as there is a match between the values of the column specified. You should compare this feature to a *inner join* in SQL.

The name of the column on which you apply your matching condition must be the same on the two tables. To rename a column, use the action [Rename (case)](../column-rename/).

The syntax is the same than for a full cross but you must also specify the name of the column after the keyword *on*.

Sample:
{% highlight xml %}
case cross 'first set' with 'second set' on 'column-name';
{% endhighlight %}

## Cross with a vector

Sometimes, you want to quickly multiply the test-cases on base of a simple set of test-cases containing a unique column. Traditionally it could be a list of partitions. In this case, it's boring to have to create a new set of test-cases just for this multiplication. In place, you can create a vector direcltly in genbiL and perform your cross with this vector. Each value associated to the vector correspond to a new test-case (row).

In the sample, here under, the set of test-cases named alpha will be combined with a set composed on one column (named beta) with two rows (the first evaluated to *value1* and the second to *value2*)

{% highlight xml %}
case cross 'alpha' with vector 'beta' values 'value1', 'value2'
{% endhighlight %}

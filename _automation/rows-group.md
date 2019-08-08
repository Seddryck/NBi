---
layout: automation
title: Group rows
prev_section: rows-distinct
next_section: rows-reduce
permalink: /automation/rows-group/
---
This command reduce the set of test-cases by grouping some rows. A group of rows is created when consecutive rows have the same value for all the columns *not* specified as parameters of this command. The values in the specified columns will be grouped into an array of values.

Following set of test-cases:

|First|Second|Third
|-----|------|-----
| A | B | C
| A | B | D
| A | B | E
| A | F | G
| A | F | H

will be reduced to two rows with following command:
{% highlight xml%}
case group column 'Third';
{% endhighlight %}

The first three rows having the same value for columns **First** and **Second**, it will result into one row with a third column being an array with values C, D, E. The next two rows will also be grouped and the third column will contain the array G, H.

|First|Second|Third
|-----|------|-----
| A | B | {C, D, E}
| A | F | {G, H}

Groups let the user create tests with several values for one variable. More info available [here](../generate-tests/#use-grouping-option).

It's possible to stipulate more than one column by separating each column by a comma.
{% highlight xml%}
case group columns 'foo', 'bar';
{% endhighlight %}

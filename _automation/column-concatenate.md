---
layout: automation
title: Concatenate content
prev_section: column-substitute
next_section: rows-filter
permalink: /automation/column-concatenate/
---
This command lets you merge the content of multiples columns into one column. This merge is executed by a concatenation of the different contents of each columns involved in the action and its executed row by row.

The correct definition of this command is the name of the first column (its content will be replaced by the result of the action) and the name of the others columns to concatenate.

{% highlight xml%}
case concatenate column 'alpha' with column 'beta', 'gamma';
{% endhighlight %}

Note that the columns beta and gamma will continue to exist after the command (they are not removed).

## Concatenate with none

If the content of one cell involved in the concatenation is equivalent to *none* then the result of the concatenation will be *none*.

## Concatenate with a value

It's possible to add a suffix to the content without creating a new column. To achieve this you must execute a concatenate based on a column with a fixed value
{% highlight xml%}
case concatenate column 'alpha' with value ' foo';
{% endhighlight %}
In this sample, the content of column 'alpha' for each row, will now be ending by ' foo'.

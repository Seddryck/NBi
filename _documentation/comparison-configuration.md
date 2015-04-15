---
layout: documentation
title: Configuration for comparison
prev_section: equivalence-resultsets
next_section: special-generic-values
permalink: /docs/comparison-configuration/
---
NBi tries to be smart when comparing two result-sets. This analysis, performed by NBi, will help you to identify the differences (if any) between your result-sets.

## Column’s roles
For each column you can specify a role. Currently, three roles are existing (Key, Value and Ignore).

### Key
The key columns are useful to find corresponding rows between two result-sets. Note that the key columns in a result-set must be unique (especially in the result-set defined in your constraint). If it’s not possible for you to specify a unique key in your expected result-set, NBi will not help you too much finding the difference with another result-set.
Based on key columns, NBi will identify how many (and which) rows are expected but not found on the actual (system-under-test) result-set –Missing Rows– and how many (and which) rows are not expected but found on the actual (system-under-test) result-set –Unexpected rows–.
To illustrate this, if you’re expecting to find two buyers A and B having respectively  bought for 100$ and 25$ then you should specify that the Supplier is the key. If your actual result-set is A (100$) and C (55$), NBi will tell you that you’ve an unexpected row (C) and a missing row (B).

### Value
The value column let NBi tells you that two rows matching keys have a difference but limited only the some values. So the row is available in both result-sets but is not identical.
To illustrate this, if you’re expecting to find two buyers A and B having respectively  bought for 100$ and 25$ then you should specify that the Supplier is the key and the amount is the value. If your result-set is A (100$) and B (55$), NBi will tell you that you’ve no unexpected row, no missing row but two matching rows. Then NBi will also add that the second row has no matching values (55$ <> 25$).
You can also apply a tolerance to value columns.

### Ignore
The column is simply ignored during the comparison. It means this column doesn’t influence the result of the comparison.
This type can be useful with MDX queries returning a default measure if you don’t care about this value. Some queries also have a Timestamp column attached as last column and this kind of column is not relevant when comparing two result-sets.

## Column’s types

### Text
This is the most common type for a column. The content of the cell is used during comparison. It means that values “10.0”,  “010” and “10” are considered as **different** when using this type. This is usually what you’ll use when specifying “Key”columns. Pay attention that the comparison is case-sensitive.

### Numeric
To avoid comparison of textual content, you can use the “Numeric” type. The content of the cell is first converted to a numeric (decimal) value using the international format (a dot to separate the decimal part). It means that values “10.0”, “010” and “10” are considered as **equal** when using this type. This type is useful when you’ve “Value” columns.

### DateTime
The DateTime type has the same role than the “Numeric” type. The content of the cell is first converted to a DateTime value  using the international format (yyyy-mm-dd hh:mm:ss).

To specify that your column is a dateTime column, just add the attribute 'type' with the value 'dateTime'

### Boolean
The boolean type has the same role than the “Numeric” type. The content of the cell is first converted to a boolean value. NBi understands "0" or "false" and "1" or "true" as boolean values.

To specify that your column is a boolean column, just add the attribute 'type' with the value 'boolean'

## Xml syntax
NBi’s xml syntax  is to define *columns* tags in your equal constraint:

{% highlight xml %}
<assert>
	<equalTo>
		<column ... />
		<column ... />
		<resultSet ... />
	</equalTo>
</assert>
{% endhighlight %}

For each column you must specify the index of this column. Note that the first column has an **index of 0 (and not 1)**. The index is a zero-based Index.

{% highlight xml %}
<column  index="4" ... />
{% endhighlight %}

Once the index is set, you must specify the role, the type and optionally the [tolerance or rounding](Tolerances and roundings) of the column.

{% highlight xml %}
<column  index="0" role="key" type="text"/>
<column  index="1" role="ignore" />
<column  index="2" role="value" type="numeric" tolerance="0.001" />
<column  index="3"
         role="value"
         type="numeric"
         rounding-style="floor"
         rounding-step="1000"
/>
{% endhighlight %}

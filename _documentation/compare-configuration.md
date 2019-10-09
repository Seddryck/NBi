---
layout: documentation
title: Configuration for comparison
prev_section: compare-equivalence-resultsets
next_section: compare-special-generic-values
permalink: /docs/compare-configuration/
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

### Default behaviour
By default, if nothing is specified, NBi will consider that all columns are keys except the last which is a value.

You can use the attribute *keys* to specify that the keys are all the columns except last (default) or just the first or even all the columns. 

You can also use the attribute *values** to specify the columns that should be treated as values. *all-except-first* and *last* respectively with the options *first* and *all-except-last* of the attribute *keys*. Since v1.16, if you select *none* for the attribute *values*, each column that is not treated as a key will be be ignored.

## Column’s types

### Text
This is the most common type for a column. The content of the cell is used during comparison. It means that values “10.0”,  “010” and “10” are considered as **different** when using this type. This is usually what you’ll use when specifying “Key”columns. Pay attention that the comparison is case-sensitive.

### Numeric
To avoid comparison of textual content, you can use the “Numeric” type. The content of the cell is first converted to a numeric (decimal) value using the international format (a dot to separate the decimal part). It means that values “10.0”, “010” and “10” are considered as **equal** when using this type. This type is useful when you’ve “Value” columns.

### DateTime
The DateTime type has the same role than the “Numeric” type. The content of the cell is first converted to a DateTime value  using the international format (yyyy-mm-dd hh:mm:ss).

To specify that your column is a dateTime column, just add the attribute *type* with the value *dateTime*

### Boolean
The boolean type has the same role than the “Numeric” type. The content of the cell is first converted to a boolean value. NBi understands "0" or "false" and "1" or "true" as boolean values.

To specify that your column is a boolean column, just add the attribute *type* with the value *boolean*

### Default behaviour
By default, if nothing is specified, NBi will consider that all key columns are *text* and all value columns are *numeric*. 

You can override this setting for value columns with the xml attribute *values-default-type* of the xml element *equal-to*. It lets you define the default type of your result-set. If your result-set contains lot of boolean, in place of specifying for each column that the type is boolean, you can simply define that the *values-default-type* is *boolean*. The possible option for this attribute are *text*, *numeric*, *dateTime*, *boolean*.

## Specification column by column (index-based)

If the attributes above are not enough to correctly define your settings, you can configure the role, type and tolerance of a specific column by adding an element *column*.

NBi’s xml syntax  is to define *column* tags in your *equal-to* (or *subset-of* and *superset-of*) constraint:

{% highlight xml %}
<assert>
  <equal-to>
    <column ... />
    <column ... />
    <result-set ... />
  </equal-to>
</assert>
{% endhighlight %}

For each column you must specify the index of this column. Note that the first column has an **index of 0 (and not 1)**. The index is a zero-based Index.

{% highlight xml %}
<column  index="4" ... />
{% endhighlight %}

Once the index is set, you must specify the role, the type and optionally the [tolerance or rounding](/docs/compare-tolerances-roundings) of the column.

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

## Specification column by column (name-based)

Since v1.15, it's possible to compare two result-sets based on columns' name. The name of the columns in both result-sets must be identical.

For each column you must specify the name of this column. 

{% highlight xml %}
<column  name="myKey" ... />
{% endhighlight %}

Then, you must specify the role, the type and optionally the [tolerance or rounding](/docs/compare-tolerances-roundings) of the column. See the section above for more information

{% highlight xml %}
<column  name="myKey" role="key" type="text"/>
<column  index="myFirstValue" role="value" type="numeric" tolerance="0.001" />
<column  index="mySecondValue"
  role="value"
  type="numeric"
  rounding-style="floor"
  rounding-step="1000"
/>
{% endhighlight %}

To speed up the definition, you can list all the key by specifying the attribute *keys-names*. In this tag, you must list all the keys separeted by a comma. The same attribute exists for values with the name *values-names*. All the column not specified as keys or values will be ignored.

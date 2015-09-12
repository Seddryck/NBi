---
layout: documentation
title: Data types
prev_section: model-relation
next_section: members
permalink: /docs/model-data-type/
---

**Since version 1.11**, NBi offers the opportunity to validate the data-types of the attributes of a relational database. This feature will be extended in the next releases.

## System-under-test
The system-under-test must be define with a parent element named *data-type*. Within this element, you must specify an element *column* with the same information that for a test of *structure* (see previous chapters).

Example:
{% highlight xml %}
<system-under-test>
  <data-type>
    <column caption="column" table="table" perspective="dwh" connectionString="ConnectionString"/>
  </data-type>
</system-under-test>
{% endhighlight %}

## Assertion
The unique valid assertion for a system-under-test *data-type* is the assertion *is*. This assertion expects a value equivalent to a data-type. The data-type could be vague or precise. To illustrate this, inside the element *is* you can define a value *varchar* or *varchar(50)*. In case the result is a *varchar(20)*, the first test will succeed but the last one will fail. This feature is also active for *decimal* or *datetime* types.

{% highlight xml %}
<assert>
  <is>
    varchar(50)
  </is>
</assert>
{% endhighlight %}

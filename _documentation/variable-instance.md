---
layout: documentation
title: Instance variables
prev_section: variable-override
next_section: metadata-concept
permalink: /docs/variable-instance/
---
Version 1.20 introduces the notion of *instance variable*. This option offers the opportunity to define a single test and execute it several time with small details changing. These small details are handled by *variables*.

## Instance-settling

To define that a test must be executed several times, you must specify an xml element named *instance-settling*. This element is expecting a single variable (element *local-variable*) having a *name* and a *type*. The different values that this variable will take can be defined by the means of a [sequence](../primitive-sequence). A *local-variable* is always evaluated at *load-time*.

{% highlight xml %}
<instance-settling>
  <local-variable name="firstDayOfMonth" type="dateTime">
    <loop-sentinel seed="2016-01-01" terminal="2016-03-01" step="1 month"/>
  </local-variable>
</instance-settling>
{% endhighlight %}

## Derived variables

Once you've created a *local-variable*, it's possible to derive some variables from this variable. This will offer the opportunity to repeat complex transformations all over your test by providing a new variable. It's also possible to create derived variables from derived variables. A *derived-variable* is always evaluated at *load-time*.

To create a derived variable you must use the element *derived-variable* and provide the name and type of the new variable in the respective attributes *name* and *type*. The attribute *based-on* let's you specify the variable on which this derivation will happen. Finally a script let's you specify how the base variable should be transformed.

{% highlight xml %}
<instance-settling>
  <local-variable name="firstDayOfMonth" type="dateTime">
    <loop-sentinel seed="2016-01-01" terminal="2016-03-01" step="1 month"/>
  </local-variable>
  <derived-variable name="secondDayOfMonth" based-on="firstDayOfMonth" type="dateTime">
    <script language="native">
      dateTime-to-next-day
    </script>
  </derived-column>
  <derived-variable name="age" based-on="secondDayOfMonth" type="numeric">
    <script language="native">
      dateTime-to-age
    </script>
  </derived-column>
</instance-settling>
{% endhighlight %}

In the example above, two derived variables are created. the second is based on the first when the first is based on the local-variable.

## Customize categories, traits and name

Based on the value of the local-variable, you can customize the name of the test but also the [*categories*](../metadata-category) and [*traits*](../metadata-trait).

To achieve this add some xml elements in the *instance-settling* such as *category* and *trait*.

{% highlight xml %}
<test name="~Instance also defines the test's name and categories and traits for {@firstDayOfMonth:MMMM}" uid="0002">
  <instance-settling>
    <local-variable name="firstDayOfMonth" type="dateTime">
      <loop-sentinel seed="2016-01-01" terminal="2016-03-01" step="1 month"/>
    </local-variable>
    <category>~{@firstDayOfMonth:MMMM}</category>
    <trait name="Year">~{@firstDayOfMonth:yyyy}</trait>
    <trait name="Month">~{@firstDayOfMonth:MM}</trait>
    <trait name="Date">@firstDayOfMonth</trait>
  </instance-settling>
  ...
</test>
{% endhighlight %}
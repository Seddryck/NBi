---
layout: documentation
title: Edition
prev_section: metadata-not-implemented
next_section: metadata-category
permalink: /docs/metadata-edition/
---
It can be really interesting to know who has created or updated a specific test case (especially when working with a team). NBi has defined a few elements to help you to achieve this. Naturally these elements are facilitative.

## Creation

To specify who is the original author of a specific test-case you can use the xml element named *edition* and its attribute *author*.

{% highlight xml %}
<test name="test's name" uid="0001">
  <edition author="Cédric L. Charlier"/>
  <system-under-test>
    ...
  </system-under-test>
  <assert>
    <exists/>
  </assert>
</test>
{% endhighlight %}

## Updates

You can also specify who has updated the test and when. For this add an inner xml element named *update* to your xml element *edition*. This new xml element has two attributes, the first one, *contributor*, let your record the name of the updater and the second one, *timestamp*, let you specify when it has been updated.

{% highlight xml %}
<test name="test's name" uid="0001">
  <edition author="Cédric L. Charlier">
    <update contributor="Your co-worker" timestamp="2012-10-16T09:55:00"/>
    <update contributor="Cédric L. Charlier" timestamp="2013-02-16T17:11:16"/>
  </edition>
  <system-under-test>
    ...
  </system-under-test>
  <assert>
    <exists/>
  </assert>
</test>
{% endhighlight %}

Note that the format of the timestamp attribute is DateTime and this should be expressed with the format YYYY-MM-DDTHH:mm:SS according to [xml  standards](http://www.w3schools.com/schema/schema_dtypes_date.asp)

## At runtime

Currently, NBi doesn't care about these elements at runtime but in the future we hope to be able to define a few [categories](/docs/metadata-category/) based on them.

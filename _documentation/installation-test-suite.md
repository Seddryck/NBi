---
layout: documentation
title: Test-suite file
prev_section: installation-environment
next_section: installation-config
permalink: /docs/installation-test-suite/
---
The test-suite file contains the definition of the tests and has an extension *.nbits*. The tests are defined in an xml format, meaning that you can edit this file with any xml or text editor.

Start by creating a new xml file with your favorite xml editor. If you want to validate the syntax of your file during its creation, you can reference, in your editor, the XSD available in file NBi-TestSuite.xsd available in the folder *framework* of the NBi zip file.

Your xml file should start with standard metadata information
{% highlight xml %}
<?xml version="1.0" encoding="utf-8"?>
{% endhighlight %}

Then, you must create an xml element testSuite and fill the attribute name. This name will be the name of your test-suite. The attribute *xmlns* is there to reference the xml namespace in the xsd (validation of the structure of your xml): don't modify it.

{% highlight xml %}
<testSuite name="My first test suite" xmlns="http://NBi/TestSuite">
{% endhighlight %}

In your the *testSuite* element, you need to specify your first test and its name. This is done through the xml element *test* and its attribute *name*. This name will be used by the underlying framework (NUnit) and so will be displayed in reports and UI.
{% highlight xml %}
<test name="my first test"/>
{% endhighlight %}

For each test, you need to specify the *system-under-test* and the *assertion* that will be executed on this system. These two parts of the test are defined by the following xml elements:
{% highlight xml %}
<system-under-test />
{% endhighlight %}
and
{% highlight xml %}
<assert />
{% endhighlight %}

In the example, here under, we'll test that the results of two queries are equal. The first query is the *system-under-test* and is defined in the xml element *system-under-test*. More info about this kind of system-under-test can be found [there](../equivalence-resultsets).

First, we need to specify which kind of test we'll perform. Here we'll perform a test of structure. For this, we're using the xml element named *structure* inside the xml element
{% highlight xml %}
<system-under-test>
 <execution>
  <query connectionString="...">
   <![CDATA[
   SELECT
    {[Measure].[MyMeasure]} ON 0,
    {[MyDimension].[MyHierarchy].Members} ON 1
   FROM
    MyCube
   ]]>
 </execution>
</system-under-test>
{% endhighlight %}

After the definition of the *system-under-test*, you need to define what will be asserted on this *system*. Here, we will assert that the result-set of this query is equivalent to the result-set of another query.
{% highlight xml %}
<assert>
 <equalTo>
  <query connectionString="...">
   SELECT MyHierarchy, MyMeasure FROM MyTable
  </query>
 </equalTo>
</assert>
{% endhighlight %}

The full listing for this test is available here under:
{% highlight xml %}
<?xml version="1.0" encoding="utf-8"?>
<testSuite name="My first test suite" xmlns="http://NBi/TestSuite">
 <test name="My first test">
  <system-under-test>
   <execution>
    <query connectionString="...">
     <![CDATA[
     SELECT
      {[Measure].[MyMeasure]} ON 0,
      {[MyDimension].[MyHierarchy].Members} ON 1
     FROM
      MyCube
     ]]>
   </execution>
  </system-under-test>
  <assert>
   <equalTo>
    <query connectionString="...">
     SELECT MyHierarchy, MyMeasure FROM MyTable
    </query>
   </equalTo>
  </assert>
 </test>
</testSuite>
{% endhighlight %}

Naturally, you can specify additional tests in your test-suite by creating more than one *test* elements.

Your next steps are to create your [config file](../installation-config) and the [nunit project file](../installation-nunit-project).

![image:Test-Suite XML.png]

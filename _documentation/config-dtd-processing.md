---
layout: documentation
title: DTD processing
prev_section: config-profile-failure-report
next_section: config-traces-debugging
permalink: /docs/config-dtd-processing/
---
The purpose of a DTD is to define the legal building blocks of an XML document. It defines the document structure with a list of legal elements. A DTD can be declared inline in your XML document, or as an external reference.

By default, NBi doesn't execute DTD commands in the XML file describing the test-suite. The main reason is security. But if you trust the content of your test-suite, it's possible to activate DTD processing. To enable the DTD processing, you must change the *config* file and set the attribute *allowDtdProcessing* to *true* (default value is *false*).

{% highlight xml %}
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <configSections>
        <section name="nbi" type="NBi.NUnit.Runtime.NBiSection, NBi.NUnit.Runtime"/>
    </configSections>
    <nbi
        testSuite="..."
        allowDtdProcessing="true"
    />
</configuration>
{% endhighlight %}

## Including a file in the test-suite

A good use-case for the DTD processing is the inclusion of some tests defined in an external file to your main test suite. A good reason to have this scenario is when you want to include a set of tests into two distinct test-suites.

First, in your test suite, you must specify a short name (here under "includeSecondTest") and the filename of the file to include (here under TestSuiteIncludedTestSuite.xml).

{% highlight xml %}
<!DOCTYPE testSuite [
<!ENTITY includeSecondTest SYSTEM "TestSuiteIncludedTestSuite.xml">
]>
{% endhighlight %}

Then at the position you want to include your file you must precede the short-name given above by an "&" and add a semi-colon ";". (See last lines). The DTD processing will include your external file at the specified position.

{% highlight xml %}
<testSuite name="The TestSuite" xmlns="http://NBi/TestSuite">
  <test name="My first test case" uid="0001">
    <system-under-test>
      <execution>
        <query name="Select first product" connectionString="Data Source=.;Initial Cataloging;Integrated Security=True">
          SELECT TOP 1 * FROM Product;
        </query>
      </execution>
    </system-under-test>
    <assert>
      <syntacticallyCorrect />
    </assert>
  </test>
  &includeSecondTest;
</testSuite>
{% endhighlight %}

You can use the same concept to include result-sets or any other idea that you could have. DTD processing has other features that you should investigate.

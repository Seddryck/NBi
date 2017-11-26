---
layout: documentation
title: Setup for Data Quality Rules engine
prev_section: metadata-group
permalink: /docs/data-quality-setup/
---
It’s possible to use NBi as a data quality rules engine. The assertions *equal-to* (*subset-of*, *superset-of*), *unique-rows*, *row-count* (*all/no/single/some-rows*) are supported in this context.

The gap between a data quality rule engine and an automated test framework is especially in the reporting. When using an automated testing tool, you usually don’t care of details when a test is successful. At the opposite, a data quality rules engine will need to report (at the minimum) how many rows were successful. To achieve this, NBi lets you configure a few specific attributes that you won’t use in when you're targeting an automated testing tool.

A valid configuration, to use NBi as a data quality rules engine, is to set the *format* to *JSON* and the *mode* to *Always* (in place of respectively *Markdown* and *OnFailure* which are the default values).

{% highlight xml %}
<nbi>
    <failure-report-profile
        threshold-sample-items="50"
        max-sample-items="25"
        expected-set="None"
        actual-set="None"
        analysis-set="Sample"
        format="Json"
        mode="Always"
    />
</nbi>
{% endhighlight %}

These two settings will enable NBi to always add to the result file some information about the tests in a JSON format.

The JSON content is embedded in the element *message* of the xml file containing the results. This JSON document has the following structure: 3 items named

* *actual*: a table information for the actual result-set
* *expected*: a table information for the expected result-set [Only for *equal-to*, *subset-of*, *superset-of*]
* *analysis*: some tables information for the analysis

Depending on the type of the constraint, the item *analysis* could contain tables *unexpected-rows*, *missing-rows*, *duplicated-rows*, ...

Each table information contains the row-count, if sampled the row-count of the sampled result-set, the structure of the result-set (for each column, the properties such as role, type, tolerance, roundings, ...) and the rows. The representation of the value of each cell is in a text format independently of the underlying type. This is a choice made to let you capture specific values such as *(any)*, *(value)* or *(empty)*.

{% highlight xml %}
<test-case name="NBi.NUnit.Runtime.TestSuite.Simple equalTo with Failure" description="" executed="True" result="Error" success="False" time="1.351" asserts="1">
  <categories>
    <category name="Execution" />
  </categories>
  <failure>
    <message><![CDATA[NBi.NUnit.Runtime.CustomStackTraceAssertionException : {"expected":{"total-rows":2,"table":{"columns":[{"position":0,"name":"Column1","role":"KEY","type":"Text"},{"position":1,"name":"Column2","role":"VALUE","type":"Numeric"}],"rows":[["Alpha","1"],["Beta","3"]]}},"actual":{"total-rows":2,"table":{"columns":[{"position":0,"name":"Column1","role":"KEY","type":"Text"},{"position":1,"name":"Column2","role":"VALUE","type":"Numeric"}],"rows":[["Alpha","1"],["Beta","2"]]}},"analysis":{"unexpected":{"total-rows":0},"missing":{"total-rows":0},"duplicated":{"total-rows":0},"non-matching":{"total-rows":1,"table":{"columns":[{"position":0,"name":"Column1","role":"KEY","type":"Text"},{"position":1,"name":"Column2","role":"VALUE","type":"Numeric"}],"rows":[[{"value":"Beta"},{"value":"2","expectation":"3"}]]}}}}]]></message>
    <stack-trace><![CDATA[<?xml version="1.0" encoding="utf-16"?>...]]></stack-trace>
  </failure>
</test-case>
{% endhighlight %}

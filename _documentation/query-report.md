---
layout: documentation
title: Get a query from a report
prev_section: query-assembly
next_section: etl-define
permalink: /docs/query-report/
---
NBi is able to extract queries from SSRS reports (more precisely from report's datasets) and use them in your tests. Other features related to queries' execution are fully applicable to queries extracted from reports: you can definitively make usage of [query's parameters](../query-parameters/) or [query's template-variables](../query-template/).

To extract the queries from a report, you've two options: connect to a ReportingServer database through sql or parse to a rdl file on a disk. In both cases, you need to specify the xml element *report* in place of the xml element named *query*

{% highlight xml %}
<system-under-test>
  <execution>
    <report ... />
  </execution>
</system-under-test>
{% endhighlight %}

### Dataset
Before trying to achieve this, you must know that a report may contain more than one dataset. In consequence, it's always needed to specify from which dataset you want to extract from a query. This can be done by using the xml attribute named *dataset* in the xml element *report*.

{% highlight xml %}
<system-under-test>
  <execution>
    <report dataset="SalesQuota" />
  </execution>
</system-under-test>
{% endhighlight %}

### Shared dataset
NBi supports to extract queries from shared datasets. Independently of the way the dataset is created in the report (embedded or shared), NBi will retrieve the corresponding query. It's not needed to adapt the test syntax, it's transparent.

## From ReportingServer database
To extract from a ReportingServer database, you must specify the *source* as the connection-string to connect to this ReportingServer database.

{% highlight xml %}
<report source="Data Source=(local)\SQL2012;Initial Catalog=ReportServer;Integrated Security=True;" .../>
{% endhighlight %}

Don't be confused with the attribute *connectionString* defining the database on which you will apply your query. This connection-string is optional and can be replaced by a *default* value or a *reference* specified in the *settings*.

{% highlight xml %}
<report connectionString="..." />
{% endhighlight %}

Finally, you must define the report's name by the means of the attributes *path* and *name*. The *path* is referencing the folder and sub-folder of the report and *name* it's display name on the portal. Note that the leading and final slashes ("/") on the path are mandatory.
{% highlight xml %}
<report 
    source="Data Source=(local)\SQL2012;Initial Catalog=ReportServer;Integrated Security=True;"
    path="/AdventureWorks 2012/"
    name="Store-Contacts"
    dataset="Stores"
/>
{% endhighlight %}

## From a rdl file on a disk
The difference with the ReportingServer database is expressed on the attribute *source* which must be unspecified. The *path* will be expressed from the test-suite file and will have a final back-slash ("\").
{% highlight xml %}
<report
    path="AdventureWorks Sample Reports\"
    name="Store*Contacts"
    dataset="Stores"
/>
{% endhighlight %}

## From SSRS Web Service
Not supported for the moment, will be introduced with a future release.

## Defaults and references
Since version 1.9, you can define in the *defaults* and *references* values for the attributes *source* and *path*. For defaults, these values will be used if no other value is provided in a test and for *references*, they will be used if the value provided in the test is equivalent to the name of the *reference* with an @.

## Full sample
The following code extracts the query from a report named *Store\*Contacts*, in directory */AdventureWorks 2012/* hosted on a ReportingServer database. The query is available in the dataset named *StoreContacts* and NBi applies a value of 300 to the parameter named *StoreID* when executing the query on a database with a connectionString referenced in the default settings applying to a system-under-test.
{% highlight xml %}
<system-under-test>
  <execution>
    <report
      source="Data Source=(local)\SQL2012;Initial Catalog=ReportServer;Integrated Security=True;"
      path="/AdventureWorks 2012/"
      name="Store*Contacts"
      dataset="StoreContacts"
    >
      <parameter name="StoreID">300</parameter>
    </report>
  </execution>
</system-under-test>
{% endhighlight %}

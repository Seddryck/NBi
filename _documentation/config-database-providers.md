---
layout: documentation
title: Database providers
prev_section: config-roles-query
next_section: config-profile-csv
permalink: /docs/config-database-provider/
---
When dealing with standard Microsoft data sources, NBi automatically creates the correct connections and commands objects to query these data sources. The choice performed by NBi to create an object or another is dependent of the connection string.

More precisely, NBi extract the information "provider" from the connection string and tries to match this information with a known provider and instantiate the corresponding objects. If no information is provided for the *provider* in the connection-string then objects from the *System.Data.SqlClient* namespace will be created. If the provider defined in the connection-string starts by *msolap*, the namespace *Microsoft.AnalysisServices.AdomdClient* will be chosen. For a provider starting by *sqlncli* or *oledb* then the namespace *System.Data.OleDb* will be the chosen.

## Register additional providers
Since version 1.11, it's possible to register additional patterns to ensure that NBi correctly detects the provider specified in the connection-strings.

In the example here under, you'll add the provider *Microsoft.ACE.OLEDB.12.0* and specify that this provider will be make usage of objects from the namespace *System.Data.OleDb*. To achieve this goal, you must edit the config file. Add an element *providers* under the element *nbi* in your config file. Then under this new element, you add and *add* element where you define the name of the provider (attribute *id*) and the corresponding namespace to use (attribute *invariant-name*).

{% highlight xml %}
<nbi testSuite="...">
  <providers>
    <add id="Microsoft.ACE.OLEDB.12.0" invariant-name="System.Data.OleDb"/>
  </providers>
</nbi>
{% endhighlight %}

At this moment, the list of valid namespace is strictly limited to *System.Data.OleDb*, *System.Data.SqlClient* and *Microsoft.AnalysisServices.AdomdClient*. Future versions should enable more choice and offer the opportunity to extend this list by yourself.

If you've added this to your config file as soon as one of your connection-string is defined with the provider *Microsoft.ACE.OLEDB.12.0*, NBi will be able identify that this provider should use the *System.Data.OleDb* namespace.

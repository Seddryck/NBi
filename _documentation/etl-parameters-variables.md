---
layout: documentation
title: Etl's parameters and variables
prev_section: etl-define
next_section: etl-successful
permalink: /docs/etl-parameters-variables/
---
Parameters (Since SQL Server 2012)
----------------------------------
An SSIS package is usually developed with parameters for variables (such as connection strings or folders, file names, …) that you’d like to define at runtime. NBi supports to define these package **and project (since version 1.9)** parameters in your test definition by providing an xml element named “parameter” and available as a child of the xml element *etl*.

You’ll need to define the name (xml attribute) and the value (xml text) of the parameter that you want to supply to the etl at runtime.

{% highlight xml %}
<etl path="Etl\" name="Sample.dtsx" password="p@ssw0rd" timeout="10000">
    <parameter name="DestinationPath">C:\toto-timeout.txt</parameter>
    <parameter name="TopRows">4000</parameter>
</etl>
{% endhighlight %}

Environment (Since SQL Server 2012)
----------------------------------
If you don't want to define all the parameters and variables of your *etl*, you can specify an environment. To achieve this, you’ll need to define the attribute *environement*.

{% highlight xml %}
<etl
   path="Etl\"
   name="Sample.dtsx"
   password="p@ssw0rd"
   environment="acceptance"
/>
{% endhighlight %}

Variables (not available for SSIS Catalog)
------------------------------------------
Note that this feature is exclusively supported for *DTS package* (so not a *Catalog package*). This means that you should configure it as *Hosted on a file folder* or *Hosted on SQL Server* but not as *Hosted on SQL Server (SSIS Catalog)*.

The usage and the syntax is exactly the same than for package parameters. Meaning that you don't need to adapt your test if you transform a variable into a parameter. Note, that unfortunately, for the moment, you must use the full name of the variable (including the namespace, usually User::), but it will be eventually fixed in a next release.

{% highlight xml %}
<etl path="Etl\" name="Sample.dtsx" password="p@ssw0rd" timeout="10000">
    <parameter name="User::DestinationPath">C:\toto-timeout.txt</parameter>
    <parameter name="User::TopRows">4000</parameter>
</etl>
{% endhighlight %}

---
layout: documentation
title: Apply roles to a query
prev_section: connection-powerbi-desktop
next_section: resultset-all-no-rows
permalink: /docs/connection-roles/
---
It's a common practice in Business Intelligence to restrict views and data according to the user connected. The views are linked to the *roles* defined in Analysis Services. When testing an environment, it's useful to play with the different *roles* for which different results are expected. Changing the role is a feature available for any query targeting an SSAS environment.

NBi supports this case by providing an xml attribute named *roles* on the xml element *query* (or derivates). You can specify the role in which the query will be executed in this xml attribute. NBi will connect to your cube with this role and execute the query.

{% highlight xml %}
<query ref="OlapQA" roles="minimal">
     ...
<query>
{% endhighlight %}

Note that the user executing the test needs to be in each role that the tests are targeting or the test won't execute and will raise a failure.

If you want, you can define more than one role for the context of execution. You just need to separate them by a semi-column (;).

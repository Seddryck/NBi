---
layout: automation
title: Source your test-cases from a query
prev_section: format-variable
next_section: genbil
permalink: /automation/source-test-cases-from-query/
---
You can source your test cases from a query in place of a CSV file. The query can be a MDX or SQL query. Each column will be converted to a variable.

To use this feature, first click on the tab Query, to switch to the panel for the definition of your query.

![Click on query tab]({{ site.baseurl }}/img/automation/01 - Click on query tab.png)

You must define the connection-string to your source system. For this click on the icon to add a new connection-string.

![Click to add a connection-string]({{ site.baseurl }}/img/automation/02 - Click to add a connection-string.png)

fill the connection-string and click on apply

![Write connection-string and click apply]({{ site.baseurl }}/img/automation/03 - write connection-string and click apply.png)

You can repeat these operations as often as you need, to register a few connection-strings. When done, select one of the defined connection-strings

![Select connection-string]({{ site.baseurl }}/img/automation/04 - Select connection-string.png)

Write your sql query in the text area and click on the icon to execute your query.

![Fill your query]({{ site.baseurl }}/img/automation/05 - Fill your query.png)

The result will be displayed in the classical table.

![Result-displayed]({{ site.baseurl }}/img/automation/06 - Result-displayed.png)

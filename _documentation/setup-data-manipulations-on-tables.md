---
layout: documentation
title: Data Manipulation on tables
prev_section: setup-cleanup
next_section: setup-sql-batch
permalink: /docs/setup-data-manipulations-on-tables/
---
The following commands are only available on a **Sql Server** instance.

* *table-reset*: this command delete all the rows of a table (truncation).
* *table-load*: this command fill a table on base of a csv file (bulk load). The xml attribute named *file* expects the name of the csv file to load. The content of the csv file will be appended to the existing rows in the table.

For all of them, The xml attribute named *name* expects the name of the impacted (truncated or loaded) table.

{% highlight xml %}
<setup>
	<table-reset  name="NewUsers"
		connectionString="..."
	/>
	<table-load   name="NewUsers"
		file ="NewUsers.csv"
		connectionString="..."
	/>
</setup>
{% endhighlight %}

If you want, you can also reference a connection-string in the xml attribute named *connectionString*. If you want to avoid to repeat this connection-string within all your commands, you can create a default value for the *connectionString* in the [settings](/docs/config-connection-strings) at the top of your test-suite where the value for the xml attribute *apply-to* must be set *setup-cleanup*.

{% highlight xml %}
<settings>
	<default apply-to="setup-cleanup">
		<connectionString>Data Source=(local)\SQL2012;Initial Catalog=AdventureWorksDW2012;Integrated Security=true</connectionString>
	</default>
</settings>
{% endhighlight %}

If you want to perform more complex manipulations with your data tables, you can also check the command [sql-run](/docs/setup-sql-batch).

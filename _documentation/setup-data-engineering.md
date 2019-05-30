---
layout: documentation
title: Data engineering
prev_section: setup-cleanup
next_section: setup-io
permalink: /docs/setup-data-engineering/
---

These commands, from the data engineering family group, cover the need to setup (or cleanup) the content of some tables of a relational database before (or after) executing a tests. To use them you're required to proceed to the installation of an [extension](../extension-support/) specific to the RDBMS targetted by the tests.

Except the attribute *connection-string* all the attributes accept [literal values](../primitive-scalar/#literal), and [variables](../primitive-scalar/#reference-to-a-variable) including [formatting](../primitive-scalar/#formatting).

## Bulk loading or truncating a table

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

**Important note:** This command is executed on the server running the test-suite (not on a remote server).

## Executing a SQL script

* _sql-run_: this command run a batch of sql commands on your Sql Server instance.

The xml attribute named *name* expects the name of the file containing the Sql commands. The attribute *path* is the path for the folder containing this file. This path must be relative to the test-suite. The attribute *connectionString* lets you define the connection-string for the targeted database. This connection-string but me a SqlClient connection-string and cannot be an OleDb or ODBC connection-string (avoid provider or driver parameters).

{% highlight xml %}
<setup>
  <sql-run
    name="MyCommands.Sql"
    path="SQL\"
    connectionString="..."
  />
</setup>
{% endhighlight %}

## Running ETL

* *etl-run*: this command let you start a new run for your Etl

The syntax to define an etl is defined at the page [define an Etl](/docs/etl-define/).

Sample:
{% highlight xml %}
<test name="...">
  <setup>
    <etl-run name="Sample.dtsx" path="Etl\">
      <parameter name="DataToLoadPath">C:\data.csv</parameter>
    </etl-run>
  </setup>
…
</test>
{% endhighlight %}

## Referencing a connection-string

If you want, you can also reference a connection-string in the xml attributes named *connectionString*. Useful, to avoid to repeat this connection-string within all your commands, you can create a default value for the *connectionString* in the [settings](/docs/config-connection-strings) at the top of your test-suite where the value for the xml attribute *apply-to* must be set *setup-cleanup*.

{% highlight xml %}
<settings>
  <default apply-to="setup-cleanup">
    <connectionString>Data Source=(local)\SQL2012;Initial Catalog=AdventureWorksDW2012;Integrated Security=true</connectionString>
  </default>
</settings>
{% endhighlight %}
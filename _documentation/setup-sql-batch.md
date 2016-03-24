---
layout: documentation
title: Run a sql batch file
prev_section: setup-data-manipulations-on-tables
next_section: setup-exe-or-batch
permalink: /docs/setup-sql-batch/
---
The following command is only available on a **Sql Server** instance.

* _sql-run_: this command run a batch of sql commands on your Sql Server instance.

The xml attribute named _name_ expects the name of the file containing the Sql commands. The attribute _path_ is the path for the folder containing this file. This path must be relative to the test-suite. The attribute _connectionString_ lets you define the connection-string for the targeted database.

{% highlight xml %}
<setup>
  <sql-run
    name="MyCommands.Sql"
    path="SQL\"
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

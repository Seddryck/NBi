---
layout: documentation
title: Query's parameters
prev_section: query-timeout
next_section: query-template
permalink: /docs/query-parameters/
---
Parameterized queries can also improve query execution performance, because they help the database server accurately match the incoming command with a proper cached query plan. This also helps guard against *SQL injection* attacks, in which an attacker inserts a command that compromises security on the server into an SQL statement. Parameters provide type checking and length validation. In addition to the security and performance benefits, parameterized commands provide a convenient method for organizing values passed to a data source. You can use parameters for passing values to sql or mdx statements.

Unlike a [template-value](../query-template), a parameter input is treated as a literal value, not as executable code.

**Note that this feature is only available if you're using System.Data.SqlClient or System.Data.AdomdClient.**

When creating a parameterized query, you identify the parameter name by prefixing the name with the at sign (@). For example, @Year would be a valid parameter name. Parameters must be named parameters, NBi doesn't support the question mark syntax.

## SQL parameters
In your test definition, in addition of the element *query*, you'll also need to describe your *parameter* by defining its name, sql-type and its value within an xml element named *parameter*.

{% highlight xml %}
<query>
	select * from Customer where CustomerKey=@CustKeyParam
	<parameter
		name="CustKeyParam"
		sql-type="Int"
	>
		145
	</parameter>
</query>
{% endhighlight %}

The query executed by NBi will be similar (but not strictly identical) to
{% highlight sql %}
select * from Customer where CustomerKey=145
{% endhighlight %}

More precisely, NBi will execute a [sp_executesql](http://msdn.microsoft.com/en-us/library/ms188001.aspx) with the query statement and the parameters provided in the xml element *query*.

### Sql-type

With an **SqlClient connection (SQL)**, the attribute sql-type is optional but it's highly recommended to use it. It will save you from pitfalls with SQL Server trying to guess by itself the type of your parameters (and failing). When specifying the value of the sql-type, you'll need to provide the SQL type such as varchar(50) or int or bit (and not the corresponding C# type string, byte or boolean). For sql-types such as varchar or char or decimal the additional parameters (size, precision, ...) between brackets will also be considered by NBi.
{% highlight xml %}
<parameter
	name="MyDate" sql-type="Decimal(10,3)"
>
	100.42
</parameter>
{% endhighlight %}

### Value for a date

To provide a value for a date parameter, we recommend the universal format (YYYY-MM-DD). Bellow the 26th of December 2013 is provided as a parameter value for the parameter @
{% highlight xml %}
<parameter
	name="MyDate" sql-type="Date"
>
	2013-12-26
</parameter>
{% endhighlight %}

## Parameters and Adomd

With an **AdomdClient connection (DAX or MDX)**, you don't need to specify the sql-type. Note that the usage of parameters for MDX queries is not straightforward and the documentation not really exhaustive. MDX only supports parameters for literal or scalar values.

{% highlight xml %}
<query>
	select [Measures].members on 0,
       Filter(Customer.[Customer Geography].Country.members,
              Customer.[Customer Geography].CurrentMember.Name =
              @CountryName) on 1
    from [Adventure Works]
	<parameter name="CountryName">
		'United Kingdom'
	</parameter>
</query>
{% endhighlight %}

To create a parameter that reference a member, set, or tuple, you would have to use a function such as StrToMember or StrToSet.
{% highlight xml %}
<query>
	SELECT
		NON EMPTY [Dim Unit].[All Units].[Category Name].Members ON 0
	FROM
		[MY CUBE]
	WHERE
		(StrToMember(@CompanyId),StrToMember(@Location))
	<parameter name="CompanyId">
		[Dim Company].&125
	</parameter>
	<parameter name="Location">
		[Dim Location].[Country].[Canada]
	</parameter>
</query>
{% endhighlight %}

## Parameters defined at the test-suite level

Sometimes, a few parameters are used in more than one query and their values are constant through the test-suite. In this case, you can save time and define them at the test-suite level. This can be achieved by the usage of the element *parameter* in the element *default* of the element *settings*.

When a parameter is defined at the test-suite level, this parameter is applied to each test to the query within the corresponding scope (*system-under-test* or *assert*).

Within the code snippet here under, we're defining twice a *parameter* named *location*. The first one is defined for all queries (in a *system-under-test*) and is a MDX parameter with the value *[Dim Location].[Country].[Canada]*. The second parameter is an SQL parameter and its value is simply *"Canada"*. All the queries defined within an *assert* will try to use an attribute with the same name but with a sql type and the corresponding value.

{% highlight xml %}
<settings>
	<default apply-to="system-under-test">
		<parameter name="Location">
			[Dim Location].[Country].[Canada]
		</parameter>
	</default>
	<default apply-to="assert">
		<parameter name="Location" sql-type="varchar(50)">
			Canada
		</parameter>
	</default>
</settings>
{% endhighlight %}

### Overriding Parameters defined at the test-suite level

If a parameter is defined at the test-suite level and at the query level, the definition at the query level will be used. The value defined at the test-suite level will be overridden by the value provided at the query level.

If a parameter is not defined in a query but is provided to this query, this parameter is simply not used by SQL Server (so it's usually not a problem).

If you want to specify that a parameter defined at the test-suite level must not be used by the query, you need to specify the attribute *remove* and set it to *true*. This is especially useful when your default parameters are covering SQL queries and your effective query is an MDX statement.

{% highlight xml %}
<settings>
	<default apply-to="system-under-test">
		<parameter name="Age" sql-type="int">10</parameter>
	</default>
</settings>
<test>
  <system-under-test>
    <query>
	    SELECT ... FROM myCube WHERE (StrToMember(@Canada))
  		<parameter name="Age" remove="true"/>
			<parameter name="Name">[Dim Location].[Country].[Canada]</parameter>
    </query>
  </system-under-test>
</test>    
{% endhighlight %}

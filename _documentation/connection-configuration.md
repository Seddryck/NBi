---
layout: documentation
title: Connection strings in config files
prev_section: compare-intervals
next_section: connection-providers
permalink: /docs/connection-configuration/
---
## Default connection string
When you’ve more than a few tests, it's boring to repeat the same connection string for each test. For this kind of situation, the more efficient way to manage the connection strings is to specify a default value in the *settings* of your test suite. To achieve this, create a xml element named *settings* at the top of your test suite definition and define once the connection string for all your system-under-tests.

{% highlight xml %}
<testSuite name="The Query TestSuite" xmlns="http://NBi/TestSuite">
	<settings>
		<default apply-to="system-under-test">
			<connectionString>Provider=MSOLAP.4;Data Source=(local);Initial Catalog='Adventure Works DW 2008';localeidentifier=1049</connectionString>
		</default>
	</settings>
	<test name="...">
		...
	</test>
</testSuite>
{% endhighlight %}

When, this setting has been setup, you don't need anymore to repeat the connection string for each test. If the *connectionString* attribute is empty (or not specified), the framework will automatically apply the connection string defined in the element *default* of your *settings*. You can define one default value for the system-under-tests and another for the asserts (a third for the setup/cleanup is also available).

Note that if you’ve a *settings* element including a default value for a connection string and that you’ve define a value in the attribute *connectionString* for one of your tests, the specific value provided in the test will be used (and not the *default*) . So, even with a default set at the test-suite level, you can override this setting in a specific test.

In this sample, the first test use the default connection string for system-under-tests and the second will use the specific connectionString provided within the test.

{% highlight xml %}
<testSuite name="The Query TestSuite" xmlns="http://NBi/TestSuite">
	<settings>
		<default apply-to="system-under-test">
			<connectionString>Provider=MSOLAP.4;Data Source=(local);Initial Catalog='Adventure Works DW 2008';localeidentifier=1049</connectionString>
		</default>
	</settings>
	<test name="'Reseller Order Count' by year before 2006 (csv) on 2008" uid="0001">
		<system-under-test>
			<execution>
				<query>
					...
				</query>
			</execution>
		</system-under-test>
		<assert>
			<equalTo>
				<resultSet file="ResellerOrderCountByYearBefore2006.csv"/>
			</equalTo>
		</assert>
	</test>
	<test name="'Reseller Order Count' by year before 2006 (csv) on 2012" uid="0001">
		<system-under-test>
			<execution>
				<query connectionString="Provider=MSOLAP.4;Data Source=(local);Initial Catalog='Adventure Works DW 2012';">
					...
				</query>
			</execution>
		</system-under-test>
		<assert>
			<equalTo>
				<resultSet file="ResellerOrderCountByYearBefore2006.csv"/>
			</equalTo>
		</assert>
	</test>
</testSuite>
{% endhighlight %}

## Reference connection string

It's more tricky when your test suite is querying several databases and you want to avoid to repeat your connection strings for each tests. A typical case is when you compare the results of a few queries on your cube to the results on the corresponding operational databases. You probably have more than one source (operational database), and so you cannot apply the default value for the connection string to each test. That’s where *references* are useful. In your xml element *settings*, at the test-suite level, you can define *references* where you will specify a connection string. Each reference has a name uniquely identifying this reference in the whole test-suite.

{% highlight xml %}
<settings>
	<reference name="first-ref">
		<connectionString>My First Connection String</connectionString>
	</reference>
	<reference name="second-ref">
		<connectionString>My Second Connection String</connectionString>
	</reference>
</settings>
{% endhighlight %}

When you’ve defined some references in your test-suite, you can use them in your tests by specifiying the name of the reference prefixed by an arrobas (@) to the attribute *connectionString* of your test. The framework will automatically apply the connection string's value of the reference to your test. In this example, the first test uses the first referenced connection string (named *first-ref*) and the second test the second referenced connection string (named *second-ref*).

{% highlight xml %}
<test name="'Reseller Order Count' by year before 2006 (csv) on 2008" uid="0001">
	<system-under-test>
		<execution>
			<query connectionString="@first-ref">
				...
			</query>
		</execution>
	</system-under-test>
	<assert>
		<equalTo>
			<resultSet file="ResellerOrderCountByYearBefore2006.csv"/>
		</equalTo>
	</assert>
</test>
<test name="'Reseller Order Count' by year before 2006 (csv) on 2012" uid="0001">
	<system-under-test>
		<execution>
			<query connectionString="@second-ref">
				...
			</query>
		</execution>
	</system-under-test>
	<assert>
		<equalTo>
			<resultSet file="ResellerOrderCountByYearBefore2006.csv"/>
		</equalTo>
	</assert>
</test>
{% endhighlight %}

At the opposite of default connection strings which are specific for system-under-tests and asserts, references can be used between both elements.

## External management (config file)

If you want to use your test suite in different stages (development, test, user-acceptance, …), you want to configure different connection strings depending on the targeted stage.  To avoid to edit your test suite for each environment (and so have different nbits file), you can externalize the management of connection strings to the config file. In definitive you’ll have one unique nbits file with your test-suite definition and one config file (containing the connection strings) by stage.

To achieve this, in the settings of your test-suite, at the place where you usually write your connection string, you’ll need to specify an alias (prefixed by an arrobas (@)).

{% highlight xml %}
<settings>
	<default apply-to="system-under-test">
		<connectionString>@default-sut</connectionString>
	</default>
	<reference name="first-ref">
		<connectionString>@ref-one</connectionString>
	</reference>
	<reference name="second-ref">
		<connectionString>@ref-two</connectionString>
	</reference>
</settings>
{% endhighlight %}

This alias (without the arrobas) must be used in your config file to specify the connection string that will be used. The definition of connection strings must be done in the standard *connectionStrings* element provided by the .Net framework. You can define several alias and this can be done for *defaults* and *references*.

{% highlight xml %}
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="nbi" type="NBi.NUnit.Runtime.NBiSection, NBi.NUnit.Runtime"/>
  </configSections>
  <nbi testSuite="SubDirectory\myTestSuite.nbits"/>
  <connectionStrings>
    <clear />
    <add name="def-sut"
      connectionString="..." />
    <add name="ref-one"
      connectionString="..." />
    <add name="ref-two"
      connectionString="..." />
  </connectionStrings>
</configuration>
{% endhighlight %}

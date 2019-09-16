---
layout: documentation
title: Defaults and references
prev_section: extension-flatfile
next_section: config-settings-external-file
permalink: /docs/config-defaults-references/
---
Defaults and references are a generic case of the feature exposed in previous chapter about [connection strings](../config-connection-strings). The goal of this feature is to avoid repetition of common values for all (or a sub-part) of your tests. You write your values at one place (in the *settings* element) and these values will be applied in each of your tests. The big advantage is the centralization: in case you need to adapt these values, you'll have to change them at a unique place and not everywhere in your test-suite.

The difference between the two concepts is that a *default* will inject an information into your test if no value is provided for this information within your test. If a value is provided for this information, the *default* will be ignored.

A *reference* must be explicitly called from your test. If you don't call the reference from your test, the value of this information will not be used in your test.

## Default

### Scope

A *default* must be associated to a *scope*. This *scope* delimits where this default value will be applicable. The valid choices are:

* everywhere
* system-under-test
* assert
* setup-cleanup

If a *default* is specified for the scope *everywhere* and for any other scope, the value of everywhere will be overridden by the value provided for the specific scope. This let you define an applicable value for *assert* and *system-under-test* but another applicable value specific to the scope *setup-cleanup*. To achieve this define a default for *everywhere* and another for *setup-cleanup*.

### Values configurable in a default

Only a few kind of values are configurable in a *default* section.

The first one is the *connection-string* applicable to the tests, more information is available in [connection strings](/docs/config-connection-strings/).

You can also configure values for [query's parameters](/docs/query-parameters/) and [query's template-variables](/docs/query-template/) used in Sql or Mdx queries. Note that the sql-type of a parameter can be modified by overriding the parameter's definition into the test itself. To override the default value of a *parameter* or *template-variable* into a test, you can redefine the parameter into the test and set its new value.

The last kind of values that you can define in a *default* is information about reports and more precisely the attributes *source* and the *path*. These information will be used in each of your tests, except if overridden by a reference or by a value provided within the test.

{% highlight xml %}
<settings>
  <default apply-to="...">
    <report
      source="http://reporting.com/reports"
      path="Dashboards"
    />
  </default>
</settings>
{% endhighlight %}

The example, here under, configures a value for a connection-string that will be used everywhere, except in the system-under-test where another value is provided. A parameter named *paramEverywhere* will also be used in both *system-under-test* and *assertions*, the second parameter named *paramToOverride* will have a different values in the *system-under-test* than in the *assertion*.

{% highlight xml %}
<settings>
    <default apply-to="everywhere">
        <connection-string>My Connection String from Everywhere</connection-string>
        <parameter name="paramEverywhere">120</parameter>
        <parameter name="paramToOverride" sql-type="Int">60</parameter>
    </default>
    <default apply-to="system-under-test">
        <connection-string>My Connection String</connection-string>
        <parameter name="paramToOverride" sql-type="varchar(10)">Alpha</parameter>
    </default>
</settings>
{% endhighlight %}

In the following test, the values *Alpha*, *120* and *My Connection String* (defined in the settings above) will be used to execute the query defined in the system-under-test.

{% highlight xml %}
<test name="My first test case" uid="0001">
  <system-under-test>
    <result-set>
      <query name="Select first product">
        SELECT * FROM Product Where FieldOne=@paramToOverride and FieldTwo<>@paramEverywhere;
      </query>
    </result-set>
  </system-under-test>
  <assert>
    ...
  </assert>
</test>
{% endhighlight %}

## Reference

A reference is different than a default; in the way it must be explicitly called in the test. In the examples above, we've never explicitly said to NBi that we want to use the values provided into the default elements: it was automatic. On the other hand, by using only defaults, it's impossibleto define two (or more) values for the connection-strings: one used for the first fifty tests and the other one used for the last twenty. A *reference* is the concept to manage these cases.

When creating a *reference*, you're defining a *name* that will be used in your test to specify which *reference* needs to be used in this test. In the test, you must use the symbol *@* to specify that the value must come from a reference.

The sample here under creates two references named *first-ref* and *second-ref* and the test defined under them calls the value from the reference *second-ref*.

{% highlight xml %}
<settings>
  <reference name="first-ref">
    <connection-string>My First Connection String</connection-string>
  </reference>
  <reference name="second-ref">
    <connection-string>My Second Connection String</connection-string>
  </reference>
</settings>
<test name="My first test case" uid="0001">
  <system-under-test>
    <result-set>
      <query name="Select first product" connection-string="@second-ref">
        SELECT TOP 2 * FROM Product;
      </query>
    </result-set>
  </system-under-test>
  <assert>
    ...
  </assert>
</test>
{% endhighlight %}

### Values configurable in a reference

In a reference you can configure values for

* connection-string (more info in [Manage connection strings])
* Report (Source and Path)
* regex
* numeric-format
* currency-format

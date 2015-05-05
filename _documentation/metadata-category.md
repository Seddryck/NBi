---
layout: documentation
title: Categories
prev_section: metadata-edition
next_section: metadata-trait
permalink: /docs/metadata-category/
---
NBi supports to assign categories to individual tests. The *category* attribute provides an alternative to suites for dealing with groups of tests.

Both Gallio and NUnit test runners allow to specify a list of categories to be included (or excluded) from the run. For NUnit, This feature is accessible by use of the /include and /exclude arguments to the console runner and through a separate "Categories" tab in the user interface. This UI provides a visual indication of which categories are selected and which are not.

## Manually-defined categories

To manually add a category to your test you must use the xml element named *category* in your xml definition.

{% highlight xml %}
<test name="test's name">
    <category>Critical</category>
    <system-under-test>
        ...
    </system-under-test>
    <assert>
        ...
    </assert>
</test>
{% endhighlight %}

You can add several categories to your test by providing more than one xml element. Following example add two categories named "Critical" and "New feature release 5.2"

{% highlight xml %}
<test name="test's name">
    <category>Critical</category>
    <category>New feature release 5.2</category>
    <system-under-test>
        ...
    </system-under-test>
    <assert>
        ...
    </assert>
</test>
{% endhighlight %}

## Inherited categories

It's possible to specify categories at the level of a [group](../metadata-group). All the tests, contained in this group, inherit from the categories defined at the group level. Note that if the group contains sub-groups, all the tests in the sub-group will inherit the categories of the group and sub-group.

In the following snippet, the test is associated, at runtime, to the following three categories: Top, Sub, Test_Scope.

{% highlight xml %}
<group name="my top group">
  <category>Top</category>
  <group name="sub-group">
    <category>Sub</category>
    <test name="...">
      <category>Test_scope</category>
    </test>
  </group>
</group>
{% endhighlight %}

## Automatic categories

It's usually helpful to add categories based on the type of test (structure, members, query) or based on the kind of item in the system-under-test (dimensions, hierarchies, ...) or all the tests related to a specific item (a dimension or a measure-group).

NBi automatically adds a few of these categories to your test at runtime. It means that you don't need to add these categories to your tests but they are available in NUnit and are displayed in your reports. By default this feature is enabled but your disabled it in the config file of your test-suite.

To specify that you don't want to automatically generate additional categories for your tests, add the following attribute to your config file: enableAutoCategories="false".

{% highlight xml %}
<configuration>
    <configSections>
        <section name="nbi" type="NBi.NUnit.Runtime.NBiSection, NBi.NUnit.Runtime"/>
    </configSections>
    <nbi testSuite="SubDirectory\myTestSuite.nbits" enableAutoCategories="false"/>
</configuration>
{% endhighlight %}

Note that the framework NUnit doesn't support categories with an hyphen in the name. NBi will automatically replace each hyphen by an underscore to avoid any issue with your categories.

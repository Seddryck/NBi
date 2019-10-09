---
layout: documentation
title: Groups
prev_section: metadata-trait
next_section: data-quality-setup
permalink: /docs/metadata-group/
---
For edition purpose, especially when you've a test-suite with more than 100 tests, it's sometimes useful to group tests. This can be achieved with the xml element named *group*. You can include a few tests in a *group* element. If you're using an advanced IDE, such as Visual Studio, it will be possible to toggle off/on this group of tests when editing them. You can also create *group* elements under *group* elements and so on. The only attribute of this xml element *group* is named *name* and will let the editor specify a display name for the group.

This feature has absolutely no influence at runtime. Meaning, that this will not influence the way the tests are named or presented by NUnit.

The code snippet, here under, makes usage of several groups and groups under groups.

{% highlight xml %}
<?xml version="1.0" encoding="utf-8"?>
<testSuite name="The TestSuite" xmlns="http://NBi/TestSuite">
  <test name="My first test case" uid="0127">
    <system-under-test>
      <result-set>
        <query name="Select first product" connection-string="Data Source=.;Initial Cataloging;Integrated Security=True">
          SELECT TOP 1 * FROM Product;
        </query>
      </result-set>
    </system-under-test>
    <assert>
      <syntacticallyCorrect />
    </assert>
  </test>
  <test name="My second test case">
    <category>Category 1</category>
    <category>Category 2</category>
    <system-under-test>
      <result-set>
        <query name="Select all products" connection-string="Data Source=.;Initial Cataloging;Integrated Security=True">
          SELECT * FROM Product;
        </query>
      </result-set>
    </system-under-test>
    <assert>
      <syntacticallyCorrect />
    </assert>
    <assert>
      <fasterThan max-time-milliSeconds="5000" />
    </assert>
  </test>
  <group name="My first tests' group">
    <test name="My Mdx test case" uid="0001">
      <system-under-test>
        <result-set>
          <query file="SimpleMdx.Mdx" connection-string="Provider=MSOLAP.4;Data Source=localhost;Catalog=&quot;Finances Analysis&quot;;"/>
        </result-set>
      </system-under-test>
      <assert>
        <equal-to>
          <result-set file="SimpleMdx.csv"></result-set>
        </equal-to>
      </assert>
    </test>
    <test name="My Mdx against another Mdx" uid="0002">
      <system-under-test>
        <result-set>
          <query file="SimpleMdx.Mdx" connection-string="Provider=MSOLAP.4;Data Source=localhost;Catalog=&quot;Finances Analysis&quot;;"/>
        </result-set>
      </system-under-test>
      <assert>
        <equal-to>
          <query
            file="SimpleMdxTwo.mdx"
            connection-string="Provider=MSOLAP.4;Data Source=RemoteServer;Catalog=Finances;"
          />
        </equal-to>
      </assert>
    </test>
  </group>
  <group name="My second tests' group">
    <test name="the modifier 'Not' is available in assert Contain (Members)">
      <system-under-test>
        <members>
          <level dimension="dimension" hierarchy="hierarchy" caption="level" perspective="Perspective" connection-string="ConnectionString"/>
        </members>
      </system-under-test>
      <assert>
        <contain not="true" caption="member"/>
      </assert>
    </test>
    <group name="group in group">
      <test name="the modifier 'Not' is available in assert Contain (Members)">
        <system-under-test>
          <members>
            <level dimension="dimension" hierarchy="hierarchy" caption="level" perspective="Perspective" connection-string="ConnectionString"/>
          </members>
        </system-under-test>
        <assert>
          <contain not="true" caption="member"/>
        </assert>
      </test>
    </group>
  </group>
</testSuite>
{% endhighlight %}

Note that if, at the same level, you've tests and groups then you must ensure that all the tests are located above the first group of this level. It means that if you've, at a given level, three tests and two groups, the three tests must appear above the two groups.

This configuration is valid

{% highlight xml %}
<test/>
<test/>
<test/>
<group/>
<group/>
{% endhighlight %}

This one is not valid

{% highlight xml %}
<test/>
<group/>
<test/>
<test/>
<group/>
{% endhighlight %}

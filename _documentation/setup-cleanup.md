---
layout: documentation
title: Setup and cleanup
prev_section: members-patterns
next_section: setup-data-engineering
permalink: /docs/setup-cleanup/
---
With the help of the two xml elements named *setup* and *cleanup*, you're able to define commands that will respectively be executed before or after the test's execution. This is really useful to load predefined data in some tables or clean some tables before you effectively run your test. But setup and cleanup can also be used to start some processes, apply T-SQL scripts ...

The xml element *setup* must be located before the xml element *system-under-test*. Following the same idea, the xml element *cleanup* must be set after the xml element *assert*.

{% highlight xml %}
<test>
  <setup>
    ...
  </setup>
  <system-under-test>
    ...
  </system-under-test>
  <assert>
    ...
  </assert>
  <cleanup>
    ...
  </cleanup>
</test>
{% endhighlight %}

## Commands (parallelism vs sequential)

Some commands could be executed in parallel. It's usually the case when you're loading independent tables or starting independent services. At the opposite, some commands should be executed sequentially: clean a table then after load the table with the new data (and not the opposite).

Parallelism is usually a good way to improve performances when loading or truncating tables.

To group a set of commands, you must surround them by the xml element *tasks*. By default all commands grouped in an element *tasks* are executed in parallel. If you want to use them sequentially you must specify the xml attribute *parallel* with the value *false*.

In the sample bellow the two tables will be loaded in parallel.

{% highlight xml %}
<tasks parallel="true">
  <table-reset  name="Users"/>
  <table-reset  name="KeyDates"/>
</tasks>
{% endhighlight %}

In the sample bellow the two tables will be loaded sequentially.

{% highlight xml %}
<tasks parallel="false">
  <table-reset  name="Users"/>
  <table-reset  name="KeyDates"/>
</tasks>
{% endhighlight %}

## Inheritance

It could be really boring to write (or copy/paste) the same setup commands for a bunch of tests. To avoid this, NBi has a feature named *inheritance of setup/cleanup*. If some tests share the same setup/cleanup commands, you can move the commands at the *group* level. The commands defined at the *group* level will be executed for each test defined in the *group*.

In the sample bellow, the two tests will be preceded by a reset (truncate) of the tables *Users* and *KeyDates*.

{% highlight xml %}
<group name="Share some steps">
  <setup>
    <tasks>
      <table-reset  name="Users"/>
      <table-reset  name="KeyDates"/>
    </tasks>
  </setup>
  <test name="first test">
    ...
  </test>
  <test name="second test">
    ...
  </test>
</group>
{% endhighlight %}

You can create several layers of inheritance by defining groups in groups and you can also add additional commands valid for some individual tests. To achieve this you must specify a new setup (or cleanup) element inside the parent *group* or the *test*. The *setup* commands defined at the test level, will be executed after the setup commands defined at the group level.

In this sample, the first test will be preceded by the truncation of tables  *Users*, *KeyDates* and *FirstTable*. The second test will be preceded by the truncation of tables  *Users*, *KeyDates* and *SecondTable*.

{% highlight xml %}
<group name="Share some steps">
  <setup>
    <tasks>
      <table-reset  name="Users"/>
      <table-reset  name="KeyDates"/>
    </tasks>
  </setup>
  <test name="first test">
    <setup>
      <table-reset  name="FirstTable"/>
    </setup>
    ...
  </test>
  <test name="second test">
    <setup>
      <table-reset  name="SecondTable"/>
    </setup>
    ...
  </test>
</group>
{% endhighlight %}

## Run Once

It's a common requirement to have the same content of a database for a few tests. If these tests don't modify (update/delete/insert) the content of this database then you will be able to take advantage of feature *run-once*. The idea is that it's pointless to re-execute the *setup* after each test. The setup could be executed before the execution of the first test, it's enough. Keep in mind that you could execute individually the tests: if you only execute the test defined in second position without executing the test in first position, you want that the setup is executed.

To acieve this, group the tests in a *group* and use the inheritance of setups described above to write once your setup's commands. To specify that you want to execute some of them only once, you need to specify at the *task* level the attribute *run-once* with the value *true*.

In the sample bellow, the first set of tasks will be executed once (before the execution of the first test) and the second will be executed before each tests.
{% highlight xml %}
<setup>
  <tasks run-once="true">
    <table-reset  name="Users"/>
    <table-reset  name="KeyDates"/>
  </tasks>
  <tasks>
    <table-load   name="Users"
        file ="Users.csv"
     />
    <table-load   name="KeyDates"
        file ="KeyDates.csv"
     />
  </tasks>
</setup>
{% endhighlight %}

**Important note:** for the moment the attribute *run-once* is not designed to be compatible with cleanup decorations. If you use it, the cleanup function will be executed (once) after the first test (and not the last) ... probably not what you're looking for.

## Failures

If one of the commands requested during the setup is failing then the test will be considered as failed and will report the exception thrown during the setup. On the other hand, if one of the commands executed during the cleanup is failing this will not influence the result of your test.

In both case, commands ordered after the failing command will not be executed.

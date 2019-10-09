---
layout: documentation
title: Processes and services
prev_section: setup-io
next_section: setup-condition
permalink: /docs/setup-process/
---
This subset of commands let you start or kill a process (executable or a batch file) on the server running the test-suite (not on a remote server). The test will continue when the exe has returned.

All the attributes, enlisted here under, accept [literal values](../primitive-scalar/#literal), and [variables](../primitive-scalar/#reference-to-a-variable) including [formatting](../primitive-scalar/#formatting) and [inline transformations](../primitive-scalar/#inline-transformations).

## Wait

This command offers the opportunity to ask your test to make a pause and wait for a delay. This delay is specified in milliseconds.

{% highlight xml %}
<setup>
  <wait
    milliseconds="1000"
  />
</setup>
{% endhighlight %}

An alternative, introduced in the version 1.13, is to wait until a connection is available. You can also define (in milliseconds) a timeout to establish this connection. If the connection can't be established before this timeout, the test will not be executed and will report a failure.

{% highlight xml %}
<setup>
   <wait-connection
      connection-string="@PowerBI"
      max-timeout="100000"
   />
</setup>
{% endhighlight %}

## Run batch or executable

This command runs an executable (.exe) or a batch file (.bat).

The attribute *name* stands for the name of the exe or batch file to execute (you must specify the extension). The xml attribute named *path* is the path of this exe or batch to execute. The attribute *arguments* lets you define the arguments to pass to your executable. Finally, the xml attribute named *timeout-milliseconds* will let you define the maximum time to effectively execute the software, if this executable or batch file hasn't finished after the given time, the test will not be executed and will return a failure.

{% highlight xml %}
<setup>
  <exe-run
    name="MySoft.exe"
    path="C:\Program Files\Tools\"
    arguments="-f -t -e"
    timeout-milliseconds="1000"
  />
</setup>
{% endhighlight %}

## Kill process

This command kills *all* processes with a given name.

The attribute *name* stands for the name of the processes to kill. If no process with this name is currently running then the test will not fail.

{% highlight xml %}
<setup>
  <exe-kill
    name="PBiDesktop"
  />
</setup>
{% endhighlight %}

## Change service's state

These commands will be executed on the server running the test-suite (not on a remote server). They're able to start or stop a Windows Service. For both of the commands, if the service is already started/stopped, this command will have no influence (and will not failed).

* *service-start*: this command starts a windows service.
* *service-stop*: this command stops a windows service.

For both commands, the xml attribute *name* specifies the name of the windows service to start or stop.

{% highlight xml %}
<setup>
	<service-start name="MyService"/>
</setup>
{% endhighlight %}

The attribute *timeout-milliseconds* is setting a maximum time to elapse before the command has successfuly completed. If the timeout is reached, the command will return a failure and the test will not be executed. This attribute is optional but a **default value of 5 seconds** is applied in case this attribute is not specified.

{% highlight xml %}
<cleanup>
	<service-stop name="MyService" timeout-milliseconds="15000"/>
</cleanup>
{% endhighlight %}
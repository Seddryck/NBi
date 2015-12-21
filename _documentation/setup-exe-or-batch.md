---
layout: documentation
title: Run or kill an executable or a batch
prev_section: setup-sql-batch
next_section: setup-etl
permalink: /docs/setup-exe-or-batch/
---
This subset of commands let you start or kill a process (executable or a batch file) on the server running the test-suite (not on a remote server). The test will continue when the exe has returned.

## exe-run

This command runs an executable (.exe) or a batch file (.bat).

The attribute *name* stands for the name of the exe or batch file to execute (you must specify the extension). The xml attribute named *path* is the path of this exe or batch to execute. The attribute *arguments* lets you define the arguments to pass to your executable. Finally, the xml attribute named *timeout-milliseconds* will let you define the maximum time to effectively execute the software, if this executable or batch file hasn't finished after the given time, the test will not be executed and will return a failure.

{% highlight xml %}
<setup>
  <exe-run
    name="MySoft.exe"
    path="C:\Program Files\Tools"
    arguments="-f -t -e"
    timeout-milliseconds="1000"
  />
</setup>
{% endhighlight %}

## exe-kill

This command kills *all* processes with a given name.

The attribute *name* stands for the name of the processes to kill. If no process with this name is currently running then the test will not fail.

{% highlight xml %}
<setup>
  <exe-kill
    name="PBiDesktop"
  />
</setup>
{% endhighlight %}

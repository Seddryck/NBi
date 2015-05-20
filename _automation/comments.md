---
layout: automation
title: Comments in macro file
prev_section: genbil
next_section: cases-load
permalink: /automation/comments/
---
It's possible to annotate a macro written in genbiL with some comments. Two styles of comments are supported: single-line comments and delimited comments.

## Single-line comments

The start with the characters // and extend to the end of the source line.

{% highlight xml %}
// This is a comment on 1 line
{% endhighlight %}

## Delimited comments

They start with the characters /\* and end with the characters \*/. Delimited comments may span multiple lines.

{% highlight xml %}
/* This is a comment
on multiple
lines*/
{% endhighlight %}

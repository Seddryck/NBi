---
layout: documentation
title: Failures' report
prev_section: config-profile-csv
next_section: config-dtd-processing
permalink: /docs/config-profile-failure-report/
---
NBi execution and failures' analysis display by default the first 10 rows of a result-set or the first 10 items of a list. It's possible to override this behavior in the config file of a test-suite.

To achieve this, you must edit the *nbi* element in your config file and add a child element named *failure-report-profile*.

The first two attributes specify the maximum count of rows (items). The first one, *threshold-sample-items* specify when the full list will not be rendered and only a subset of the items of this list will be rendered. The second named *max-sample-items* specifies how many items will contain this subset.

Then, you've three attributes to specify when the previous values will be effectively in use. For each set rendered in a report you can define three values:

* *none*: this set will never be rendered
* *sample*: the two previously defined attributes will be applied. Meaning that if the set to render has more attributes than *threshold-sample-items*, only *max-sample-items* will be rendered. If the set contains less than *threshold-sample-items* items then the whole set will be rendered.
* *full*: the whole set will be rendered even if it contains more items than the value defined for the attribute *threshold-sample-items*.

{% highlight xml %}
<nbi>
    <failure-report-profile
        threshold-sample-items="50"
        max-sample-items="25"
        expected-set="none"
        actual-set="sample"
        analysis-set="full"
    />
</nbi>
{% endhighlight %}

For your information, the equivalent notation of the default profile is

{% highlight xml %}
<nbi>
    <failure-report-profile
        max-sample-items="10"
        threshold-sample-items="15"
        expected-set="sample"
        actual-set="sample"
        analysis-set="sample"
    />
</nbi>
{% endhighlight %}

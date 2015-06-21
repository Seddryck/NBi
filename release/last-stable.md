<!DOCTYPE HTML>
<html lang="en-US">
<head>
  <meta charset="utf-8">
  <title>NBi - Testing framework for Microsoft Business Intelligence platform</title>
  <meta name="viewport" content="width=device-width,initial-scale=1">
  <meta name="description" content="NBi is a testing framework (add-on to NUnit) for Microsoft Business Intelligence platform and Data Access. The main goal of this framework is to let users create tests with a declarative approach based on an Xml syntax. By the means of NBi, you don't need to develop C# code to specify your tests! Either, you don't need Visual Studio to compile your test suite. Just create an Xml file and let the framework interpret it and play your tests. The framework is designed as an add-on of NUnit but with the possibility to port it easily to other testing frameworks.
">
  <meta name="generator" content="Jekyll v2.4.0">
  <link rel="alternate" type="application/rss+xml" title="NBi - Testing framework for BI solutions" href="http://www.nbi.io/feed.xml">
  <link rel="alternate" type="application/atom+xml" title="Recent commits to NBi master branch" href="https://github.com/Seddryck/NBi/commits/master.atom">
  <link rel="stylesheet" href="//fonts.googleapis.com/css?family=Lato:300,300italic,400,400italic,700,700italic,900">
  <link rel="stylesheet" href="/css/screen.css">
  <link href="//maxcdn.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css" rel="stylesheet">
  <link rel="canonical" href="http://www.nbi.io/">
  <link rel="icon" type="image/x-icon" href="/favicon.ico">
  <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
  <!--[if lt IE 9]>
  <script src="/js/html5shiv.min.js"></script>
  <script src="/js/respond.min.js"></script>
  <![endif]-->
</head>
<body>
  <div>
    <a id="downloadURL" href="http://www.github.com/Seddryck/NBi/releases/latest">
      <section class="btn-wrapper-download">
        <div class="download-btns">
          <span class="download-text"><i class="fa fa-cloud-download">&nbsp;&nbsp;</i>Download</span>
        </div>
        <span id="download-info">
          NBi has been downloaded <span id="downloadCount">?</span>&nbsp;times. Current stable release is<span id="releaseName">?</span>.
        </span>
      </section>
    </a>
  </div>
  <div>

  </div>
  <script type="text/javascript">
    console.log("youpi");
    function GetLatestReleaseInfo() {
        $.getJSON("https://api.github.com/repos/Seddryck/NBi/releases/latest").done(function (release) {
            var asset = release.assets[0];
            var downloadURL = "https://github.com/Seddryck/NBi/releases/download/" + release.tag_name + "/" + asset.name;

            var downloadCount = 0;
            for (var i = 0; i < release.assets.length; i++) {
                downloadCount += release.assets[i].download_count;
            }
            downloadCount += 1956;
            var releaseInfo = release.name + " downloaded " + downloadCount + " times.";
            $("#downloadURL").attr("href", downloadURL);
            $("#downloadCount").text(downloadCount);
			      $("#releaseName").text(release.name);
            $("#releaseName").fadeIn("slow");
			      console.log(downloadURL);
			      console.log(release.name);
        });
    }
	GetLatestReleaseInfo();
	</script>
</body>
---
layout: page
title: About
permalink: /release/last-stable
---
<script type="text/javascript">
    $(document).ready(function () {
        GetLatestReleaseInfo();
    });

    function GetLatestReleaseInfo() {
        $.getJSON("https://api.github.com/repos/Seddryck/NBi/releases/latest").done(function (release) {
            var asset = release.assets[0];
            var downloadURL = "https://github.com/Seddryck/NBi/releases/download/" + release.tag_name + "/" + asset.name;
            var downloadCount = 0;
            for (var i = 0; i < release.assets.length; i++) {
                downloadCount += release.assets[i].download_count;
            }
            downloadCount += 1956;
            var releaseInfo = release.name + downloaded " + downloadCount + " times.";
            $(".sharex-download").attr("href", downloadURL);
            $(".release-info").text(releaseInfo);
            $(".release-info").fadeIn("slow");
        });
    }
</script>
<body>
  <div>
    <a class=".sharex-download" href="http://www.github.com">Download</a>
  </div>
  <div>
    NBi has been downloaded
    <span class=".sharex-download">?</span>
    &nbsp;times. Current release:
    <span class=".release-info">?</span>
  </div>
</body>

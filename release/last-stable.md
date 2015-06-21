---
layout: page
title: About
permalink: /release/last-stable/
---
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

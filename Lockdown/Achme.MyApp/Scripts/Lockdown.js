$(document).ready(function () {
    $("form").each(function (index, e) {
        var ok = false;
        for (i = 0; i < formUrls.length; i++) {
            var u = formUrls[i].toLowerCase();
            var a = $(e).attr("action").toLowerCase();
            if (urlMatch(u, a)) {
                ok = true;
                continue;
            }
        }

        if (!ok) {
            $(e).find("input[type='submit']").attr("disabled", "true");
        }
    });

    $("a").each(function (index, e) {
        var ok = false;
        for (i = 0; i < links.length; i++) {
            var u = links[i].toLowerCase();
            var a = $(e).attr("href").toLowerCase();
            if (urlMatch(u, a)) {
                ok = true;
                continue;
            }
        }

        if (!ok) {
            $(e).addClass("disabled");
            $(e).attr("href", "javascript:void(0);");
        }
    });
});

function urlMatch(pattern, url) {
    return (pattern == url || url.indexOf(pattern +"/") == 0 || url.indexOf(pattern + "?") == 0);
}
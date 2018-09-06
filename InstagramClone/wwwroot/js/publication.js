$(document).ready(function () {
    let post = document.getElementById("add-post");
    let postInput = document.getElementById("post-input");

    if (postInput !== null) {
        postInput.onchange = function (e) {
            document.getElementById("post").submit();
        }
    }

    if (post !== null) {
        post.onclick = function () {
            document.getElementById("label-post").click();
        }
    }

    $(window).on('resize', resize);
    $(window).on('load', resize);

    let posts = $("div.post");

    posts.on('mouseover', function () {
        let obj = $(this).find("div.view-for-post").first();
        obj.css("background-color", "rgba(0,0,0,0.5)");
        obj.css("visibility", "visible");
    });

    posts.on('mouseleave', function () {
        let obj = $(this).find("div.view-for-post").first();
        obj.css("background-color", "");
        obj.css("visibility", "hidden");
    });
});

function resize() {
    let windowWidth = $(this).width();
    $("div.post").each(function (index) {
        $(this).width(windowWidth * 0.2);
        let w = $(this).width();

        $(this).height(w);
        let h = $(this).height();

        let img = $(this).find("img.pic");
        img.width(w);
        img.height(h);
    });
}

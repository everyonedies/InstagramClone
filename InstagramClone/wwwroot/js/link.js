$(document).ready(function () {
    $("div a.nav-admin").each(function () {
        if (this.href == window.location.href) {
            $(this).addClass("currentLink");
        }
    });

    let showPosts = $("#show-posts");
    let showComments = $("#show-comments");

    let posts = $("#posts");
    let comments = $("#comments");

    showPosts.on('click', function () {
        posts.css("visibility", "visible");
        comments.css("visibility", "hidden");
    });

    showComments.on('click', function () {
        comments.css("visibility", "visible");
        posts.css("visibility", "hidden");
    });
});
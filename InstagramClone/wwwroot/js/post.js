$(document).ready(function () {
    let frm = $("#post-comment");

    frm.on('submit', function (e) {
        e.preventDefault();

        let text = $("#comment-input").val();
        let postId = $("#postId").val();

        var xmrl = new XMLHttpRequest();
        xmrl.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {

                let result = JSON.parse(this.responseText);
                if (result.error) {
                    alert(result.error);
                }
                else {
                    let comments = $("#comments");
                    let newComment = $("<p>");

                    newComment.addClass("comment-style");
                    newComment.html("<a href='/" + result.alias + "' style='text-decoration: none; color: black;'><b>" + result.alias + ":</b></a> " + text);

                    comments.append(newComment);
                    $("#comment-input").val("");
                }
            }
        };
        let body = `postId=${encodeURIComponent(postId)}&text=${encodeURIComponent(text)}`;

        xmrl.open("POST", "/Post/AddNewComment", true);
        xmrl.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
        xmrl.send(body);
    });

    let frmOwnText = $("#post-owner-text");

    frmOwnText.on('submit', function (e) {
        e.preventDefault();

        let text = $("#post-input-text").val();
        let postId = $("#post-text-id").val();

        var xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                $("#caption-text").text(text);
                $("#post-input-text").val("");
            }
        };
        let body = `postId=${encodeURIComponent(postId)}&caption=${encodeURIComponent(text)}`;

        xhr.open("POST", "/Post/AddPostCaption", true);
        xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
        xhr.send(body);
    });
});
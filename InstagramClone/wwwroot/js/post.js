$(document).ready(function () {
    let frm = $("#post-comment");

    frm.on('submit', function (e) {
        e.preventDefault();

        let text = $("#comment-input").val();
        let postId = $("#postId").val();

        let xmrl = new XMLHttpRequest();
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

        let xhr = new XMLHttpRequest();
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

    let frmTags = $("#tag-form");

    frmTags.on('submit', function (e) {
        e.preventDefault();

        let text = $("#post-input-tags").val();
        let postId = $("#post-id-tags").val();

        let xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                let tags = text.split("#");
                let strView = "";
                if (text != "") {
                    for (let i = 0; i < tags.length; i++) {
                        let t = tags[i].trim();
                        if (t != "") {
                            strView += "<span><a href='/TagPost/ShowPostsByTag?text=" + t + "' style='text-decoration: none; color: blue;'>#" + t + "</a>&nbsp</span>";
                        }
                    }
                    let htl = $("#tags").html();
                    $("#tags").html(htl + strView);
                }
                else {
                    $("#tags").html("");
                }

                $("#post-input-tags").val("");
            }
        };
        let body = `postId=${encodeURIComponent(postId)}&tags=${encodeURIComponent(text)}`;

        xhr.open("POST", "/Post/AddPostTags", true);
        xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
        xhr.send(body);
    });

    let frmLike = $("#post-like");

    frmLike.on('click', function (e) {
        $(this).submit();
    });

    frmLike.on('submit', function (e) {
        e.preventDefault();

        let postId = $("#post-like-id").val();

        let xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                let result = JSON.parse(this.response);
                $("#num-of-likes").text(result.likes + " likes");

                let img = $("#like-img");
                if (result.state == "Like") {
                    img.attr('src', "/images/favorite-heart-icon-png-23.png");
                }
                else if (result.state == "Unlike") {
                    img.attr('src', "/images/heart-outline.png");
                }
            }
        };
        let body = `postId=${encodeURIComponent(postId)}`;

        xhr.open("POST", "/Post/Like", true);
        xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
        xhr.send(body);
    });

    let frmDeletePost = $("#delete-post");
    let check = false;
    frmDeletePost.on('submit', function (e) {
        if (!check && confirm("Are you sure want to delete this post?")) {
            check = true;
            frmDeletePost.submit();
        }
        else {
            e.preventDefault();
        }
    });
});
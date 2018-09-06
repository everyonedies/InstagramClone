let followers = document.getElementById("followers");
let following = document.getElementById("following");

followers.addEventListener('click', function (e) {
    e.preventDefault();
    let url = this.getAttribute("href");
    getListOfUser(url, "Followers");
});

following.addEventListener('click', function (e) {
    e.preventDefault();
    let url = this.getAttribute("href");
    getListOfUser(url, "Following");
});

function getListOfUser(url, text) {
    var xmlhttp = new XMLHttpRequest();
    xmlhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200)
        {
            let objFollowers = JSON.parse(this.responseText);
            if (objFollowers.error)
            {
                alert(objFollowers.error);
            }
            else
            {
                createView(objFollowers, text);
            }
        }
    };
    xmlhttp.open("GET", url, true);
    xmlhttp.send();
}

function createView(objFollowers, text)
{
    lockScroll();

    let x = document.body.offsetWidth / 2;
    let y = document.body.offsetHeight / 2;

    let blackDiv = document.createElement("div");
    blackDiv.style = "width: 100%; height: 100%; position: fixed; left: 0; top: 0; background-color: black; opacity: 0.2;";

    document.body.appendChild(blackDiv);

    let followersView = document.createElement("div");
    followersView.id = "followersView";
    followersView.classList.add("absoluteBox");
    followersView.classList.add("followersBlock");
    followersView.style.visibility = "visible";

    document.body.appendChild(followersView);

    window.onresize = function (event) {
        let x = document.body.offsetWidth / 2;
        let y = document.body.offsetHeight / 2;

        let w = followersView.offsetWidth / 2;
        let h = followersView.offsetHeight / 2;
        followersView.style = "left: " + (x - w) + "px;" + "top: " + (y - h * 1.5) + "px; max-height: 245px;";
    };

    blackDiv.addEventListener("click", function (e) {
        followersView.outerHTML = "";
        blackDiv.outerHTML = "";

        unlockScroll();
    });

    let view = "<p style='margin: 0; padding-top: 20px; padding-bottom: 20px; text-align: center; border-bottom: 1px solid #ccc;'>" + text + "</p>";
    objFollowers.forEach(function (o) {
        view += "<a href='/" + o + "' class='searchLink'>" + "<p style='text-align: center; margin: 0; padding: 15px; border-bottom: 1px solid #ccc;'>" + o + "</p></a>";
    });
    followersView.innerHTML = view;

    let w = followersView.offsetWidth / 2;
    let h = followersView.offsetHeight / 2;
    followersView.style = "left: " + (x - w) + "px;" + "top: " + (y - h * 1.5) + "px;";
}

function lockScroll() {
    var scrollPosition = [
        self.pageXOffset || document.documentElement.scrollLeft || document.body.scrollLeft,
        self.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop
    ];
    var html = jQuery('html'); // it would make more sense to apply this to body, but IE7 won't have that
    html.data('scroll-position', scrollPosition);
    html.data('previous-overflow', html.css('overflow'));
    html.css('overflow', 'hidden');
    window.scrollTo(scrollPosition[0], scrollPosition[1]);
}

function unlockScroll() {
    var html = jQuery('html');
    var scrollPosition = html.data('scroll-position');
    html.css('overflow', html.data('previous-overflow'));
    window.scrollTo(scrollPosition[0], scrollPosition[1])
}

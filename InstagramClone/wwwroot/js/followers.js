$(document).ready(function () {
    let followers = document.getElementById("followers");
    let following = document.getElementById("following");

    addClickHandler(followers, "Followers");
    addClickHandler(following, "Following");
});

function addClickHandler(element, text) {
    element.addEventListener('click', async function (e) {
        e.preventDefault();
        let url = this.getAttribute("href");
        let users = await getListOfUsers(url);
        view(users, text);
    });
}

function getListOfUsers(url) {
    return fetch(url, { method: "GET" })
        .then(async res => {
            if (res.status === 200) {
                let obj = await res.json();
                return obj;
            } else {
                alert(res.status + ' - ' + res.statusText);
            }
        })
        .catch(alert);
}

function view(users, text)
{
    lockScroll();

    let blackDiv = createBlackDiv();
    $(document.body).append(blackDiv);

    let usersList = createUsersListDiv();
    $(document.body).append(usersList);

    let viewForUsersList = createUsersView(users, text);
    usersList.html(viewForUsersList);

    setUsersListSize(usersList);

    $(window).on("resize", () => setUsersListSize(usersList));
    blackDiv.on("click", () => deleteView(blackDiv, usersList));
}

function createBlackDiv() {
    let blackDiv = $("<div>");
    blackDiv.css("width", "100%"); 
    blackDiv.css("height", "100%"); 
    blackDiv.css("position", "fixed"); 
    blackDiv.css("left", "0"); 
    blackDiv.css("top", "0"); 
    blackDiv.css("background-color", "black"); 
    blackDiv.css("opacity", "0.2");

    return blackDiv;
}

function createUsersListDiv() {
    let usersView = $("<div>");
    usersView.id = "followersView";
    usersView.addClass("absoluteBox followersBlock");
    usersView.css("visibility", "visible");

    return usersView;
}

function deleteView(usersList, blackDiv) {
    usersList.remove();
    blackDiv.remove();

    unlockScroll();
}

function createUsersView(users, text) {
    let view = `<p style='
                    margin: 0; 
                    padding-top: 20px;
                    padding-bottom: 20px;
                    text-align: center;
                    border-bottom: 1px solid #ccc;'> ${text}
                </p>`;

    $.each(users, function (key, value) {
        view += `<a href='/${value}' class='searchLink'>
                    <p style='
                        text-align: center;
                        margin: 0;
                        padding: 15px;
                        border-bottom: 1px solid #ccc;'>${value}
                    </p>
                 </a>`;
    });

    return view;
}

function setUsersListSize(usersList) {
    let w = usersList.outerWidth() / 2;
    let h = usersList.outerHeight() / 2;

    let x = $(window).width() / 2;
    let y = $(window).height() / 2;

    usersList.css("left", x - w);
    usersList.css("top", y - h);
}

function lockScroll() {
    var scrollPosition = [
        self.pageXOffset || document.documentElement.scrollLeft || document.body.scrollLeft,
        self.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop
    ];
    var html = jQuery('html');
    html.data('scroll-position', scrollPosition);
    html.data('previous-overflow', html.css('overflow'));
    html.css('overflow', 'hidden');
    window.scrollTo(scrollPosition[0], scrollPosition[1]);
}

function unlockScroll() {
    var html = jQuery('html');
    var scrollPosition = html.data('scroll-position');
    html.css('overflow', html.data('previous-overflow'));
    window.scrollTo(scrollPosition[0], scrollPosition[1]);
}

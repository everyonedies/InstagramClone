$(document).ready(function () {
    let searchField = document.getElementById("input-search");
    let searchResult = document.getElementById("searchResult");
    let state = "hidden";

    function forBlur(e) {
        state = "hidden";
        searchResult.style.visibility = state;
    }

    searchResult.addEventListener("mouseover", function (e) {
        searchField.removeEventListener("blur", forBlur, false);
    });

    searchResult.addEventListener("mouseleave", function (e) {
        if (searchField !== document.activeElement) {
            state = "hidden";
            searchResult.style.visibility = state;
        }
        searchField.addEventListener("blur", forBlur, false);
    });

    searchField.addEventListener('blur', forBlur, false);

    searchField.addEventListener('focus', function (e) {
        if (searchField.value !== "")
            state = "visible";
        else
            state = "hidden";
        searchResult.style.visibility = state;
    });

    function searchFunc(evt) {
        if (this.value == 0) {
            state = "hidden";
            searchResult.style.visibility = state;
        }
        else {
            var xmlhttp = new XMLHttpRequest();
            xmlhttp.onreadystatechange = function () {
                if (this.readyState == 4 && this.status == 200) {
                    let obj = JSON.parse(this.responseText);
                    state = "visible";
                    if (obj.error) {
                        searchResult.style.visibility = state;
                        searchResult.innerHTML = '<p align="center" style="width: 100%; margin: 0; padding-top: 10px; padding-bottom: 10px;">No results</p>';
                    }
                    else {
                        searchResult.style.visibility = state;
                        let text = "";
                        obj.forEach(function (o) {
                            let hrf = "";
                            if (o.type == "user") {
                                hrf = "/" + o.text;
                            }
                            else if (o.type == "tag") {
                                hrf = "/tagpost/showpostsbytag?text=" + o.text;
                                o.text = "#" + o.text;
                            }
                            text += "<a href='" + hrf + "' class='searchLink'>" + "<p style='margin: 0; padding: 15px; border-bottom: 1px solid #ccc;'>" + o.text + "</p></a>";
                        });
                        searchResult.innerHTML = text;
                    }
                    searchResult.style.visibility = state;
                }
            };
            let text = `${encodeURIComponent(this.value)}`;
            xmlhttp.open("GET", "/Home/SearchAjax?text=" + text, true);
            xmlhttp.send();
        }
    }

    searchField.addEventListener('input', searchFunc);
    searchField.addEventListener('click', function (e) {
        if (this.value != 0) {
            state = "visible";
            searchResult.style.visibility = state;
        }
    });
});

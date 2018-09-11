let searchField = document.getElementById("search");
let rect = searchField.getBoundingClientRect();
let state = "hidden";

let resultViewDiv = document.createElement("div");
resultViewDiv.id = "resultView";
resultViewDiv.classList.add("searchBox");
resultViewDiv.style = "left: " + (rect.left - 1) + "px;" + "top: " + (rect.bottom + 10) + "px; max-height: 245px;";
resultViewDiv.style.visibility = state;

window.onresize = function (event) {
    let rect = searchField.getBoundingClientRect();
    resultViewDiv.style = "left: " + (rect.left - 1) + "px;" + "top: " + (rect.bottom + 10) + "px; max-height: 245px;";
    resultViewDiv.style.visibility = state;
};

document.body.appendChild(resultViewDiv);

function forBlur(e) {
    state = "hidden";
    resultViewDiv.style.visibility = state;
}

resultViewDiv.addEventListener("mouseover", function (e) {
    searchField.removeEventListener("blur", forBlur, false)
});

resultViewDiv.addEventListener("mouseleave", function (e) {
    if (searchField !== document.activeElement) {
        state = "hidden";
        resultViewDiv.style.visibility = state;
    }
    searchField.addEventListener("blur", forBlur, false)
});

searchField.addEventListener('blur', forBlur, false);

searchField.addEventListener('focus', function (e) {
    if (searchField.value !== "")
        state = "visible";
    else
        state = "hidden";
    resultViewDiv.style.visibility = state;
});

function searchFunc(evt) {
    if (this.value == 0) {
        state = "hidden";
        resultViewDiv.style.visibility = state;
    }
    else {
        var xmlhttp = new XMLHttpRequest();
        xmlhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                let obj = JSON.parse(this.responseText);
                state = "visible";
                if (obj.error) {
                    resultViewDiv.style.visibility = state;
                    resultViewDiv.innerHTML = '<p align="center" style="width: 100%; margin: 0; padding-top: 10px; padding-bottom: 10px;">No results</p>';
                }
                else {
                    resultViewDiv.style.visibility = state;
                    let text = "";
                    obj.forEach(function (o) {
                        text += "<a href='/" + o + "' class='searchLink'>" + "<p style='margin: 0; padding: 15px; border-bottom: 1px solid #ccc;'>" + o + "</p></a>";
                    });
                    resultViewDiv.innerHTML = text;
                }
                resultViewDiv.style.visibility = state;
            }
        };
        xmlhttp.open("GET", "/Home/SearchAjax?alias=" + this.value, true);
        xmlhttp.send();
    }
}

searchField.addEventListener('input', searchFunc);
searchField.addEventListener('click', function (e) {
    if (this.value != 0) {
        state = "visible";
        resultViewDiv.style.visibility = state;
    }
});


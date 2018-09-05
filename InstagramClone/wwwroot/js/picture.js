let input = document.getElementById("file-input");

if (input !== null) {
    input.onchange = function (e) {
        document.getElementById("photo").submit();
    }
}

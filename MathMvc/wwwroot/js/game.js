function limit(element, id) {
    var max_chars = 1;
    if (element.value.length > max_chars) {
        element.value = element.value.substr(1, 2);
    }
}

function setResult() {
    var value = "";
    var inputs = document.getElementsByClassName("number");
    for (var i = 0; i < inputs.length; i++) {
        value += inputs[i].value;
    }
    document.getElementById("result").value = value;
}

function setAutoSubmitInInput() {
    var inputs = document.getElementsByClassName("number");
    inputs[inputs.length - 1].addEventListener("input", () => {
        setResult();
        document.getElementById("form-high-level").submit();
    })
}

function intervalSeconds() {
    const second_label = document.getElementById("seconds");
    if (seconds > 1) {
        seconds = seconds - 1;
        second_label.innerHTML = seconds + "s";
    }
    else {
        second_label.innerHTML = "";
        clearInterval(intervalSeconds);
    }
}

function intervalFunction() {
    const div = document.getElementById("loading");

    if (width > 1) {
        width = width - 1;
        div.style.width = width + "%";
        console.log(width);
    }
    else {
        div.style.width = "0%";
        div.style.backgroundColor = "white";
        div.style.border = "none";
        setResult();
        document.getElementById("form-high-level").submit();
        clearInterval(intervalFunction);
    }
}
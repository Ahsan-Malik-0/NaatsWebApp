const alertBtn = document.getElementById("alertBtn");
if (alertBtn) {
    alertBtn.addEventListener("click", function () {
        alert("This is an alert from external JS file!");
    });
}

$(document).ready(function () {
    console.log("Hello Pluralsight");

    var theForm = $("#theForm");
    theForm.hide();

    var button = $("#buyButton");
    button.on("click",
        function () {
            console.log("Buying an item");
        });

    var productInfo = $(".product-props li");
    productInfo.on("click", function () {
        console.log(`You clikced on ${$(this).text()}`);
    });

    var $loginToggle = $("#loginToggle");
    var $popupForm = $(".popup-form");

    $loginToggle.on("click", function() {
        $popupForm.fadeToggle(1000);
    });
});


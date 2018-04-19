// Write your JavaScript code.

$(document).ready(function () {
    // $(function()){
    console.log("JavaScript running...");
    // $('#myNav').fadeOut(0);
    $("#background-img").animate({ opacity: 1 }, 2 * 1000);

    if(window.location.pathname === '/') {
        $(".navbar-fixed-bottom").remove();
    }

    $(this).scroll(function () {
        console.log($(this).scrollTop());
        if ($(this).scrollTop() === 0) {
            $('#myNav').fadeOut();
            // $('.body').fadeOut();
        } else {
            $('#myNav').fadeIn();
            // $('.body').fadeIn();
        }
    });

    
    // $("body").parallax()
    console.log("JavaScript ended...");
});
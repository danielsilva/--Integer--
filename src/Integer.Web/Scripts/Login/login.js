$("#btnlogin").click(function () {
    var loginform = $("#loginform");
    loginform.toggle();

    $('body').append("<div id='mask'></div>");
    $('#mask').css({
        'position': 'absolute',
        'left': 0,
        'right': 0,
        'top': 0,
        'bottom': 0,
        'z-index': 3
    });

    $('#mask').click(function () {
        $(this).remove();
        $("#loginform").hide();
    });
});


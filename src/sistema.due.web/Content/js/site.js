$(document).ready(function () {
    var url = window.location;
    $('ul.nav li a').each(function () {
        if (this.href == url) {
            $("ul.nav li").each(function () {
                if ($(this).hasClass("active")) {
                    $(this).removeClass("active");
                }
            });
            $(this).parent().parent().parent().addClass('active');
            $(this).parent().addClass('active');
        }
    });

    $(".inteiro").on("keypress keyup blur", function (event) {
        $(this).val($(this).val().replace(/[^\d].+/, ""));
        if ((event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });  
});

$('.dropdown-menu-sobrepor').on('show.bs.dropdown', function () {
    $('.table-responsive').css("overflow", "inherit");
   
});

$('.dropdown-menu-sobrepor').on('hide.bs.dropdown', function () {
    $('.table-responsive').css("overflow", "auto");
})

$('.dropdown-menu-scroll').on('show.bs.dropdown', function () {
    setTimeout(function () {
        $('.dropdown-menu-scroll')[0].scrollTop = $('.dropdown-menu-scroll-referencia').height() + 20;
        $(window).scrollTop($('.dropdown-menu-scroll-referencia').height());
    }, 100);
});

function Inicializar() {
    
    $(".moeda").mask('#.##0,00', { reverse: true });

    $(".inteiro").on("keypress keyup blur", function (event) {
        $(this).val($(this).val().replace(/[^\d].+/, ""));
        if ((event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });   
}
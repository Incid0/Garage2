$(function () {
    var filterhandler;
    $('#vehiclefilter').on('input', function () {
        var filter = $(this);
        if (filterhandler) clearTimeout(filterhandler);
        filterhandler = setTimeout(function () {
            filter.parents('form').submit();
            filterhandler = null;
        }, 300);
    });

    $('input#RegNr').change(function () {
        this.value = this.value.toUpperCase();
    });

    var alertbox = $('#alertbox');
    if (alertbox.length && alertbox.text()) {
        alertbox.parent().show();
        setTimeout(function () {
            alertbox.parent().fadeOut(500);
        }, 3000)
    }
});

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
        console.log(this);
        this.value = this.value.toUpperCase();
    });
});

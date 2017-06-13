var garage = (function ($) {
    var filterhandler, alertbox;

    function localAlert(message) {
        if (message) alertbox.text(message);
        alertbox.parent().show();
        setTimeout(function () {
            alertbox.parent().fadeOut(500);
        }, 3000);
    }

    return {
        init: function () {
            console.log('init started');
            $('#vehiclefilter').on('input', function () {
                var filter = $(this);
                if (filterhandler) clearTimeout(filterhandler);
                filterhandler = setTimeout(function () {
                    filter.closest('form').submit();
                    filterhandler = null;
                }, 300);
            });

            alertbox = $('#alertbox');
            if (alertbox.length && alertbox.text()) {
                localAlert();
            }

            var aClick = false;
            $('#vehicleList').on('click', '.modal-link', function () {
                if (!aClick) $(this).attr('data-toggle', 'modal').attr('data-target', '#modalContainer').parents('tr').removeAttr('data-toggle');
                aClick = (this.nodeName === 'A');
            });
            // Attach listener to .modal-close-btn's so that when the button is pressed the modal dialog disappears
            $('body').on('click', '.modal-close-btn', function () {
                $('#modalContainer').modal('hide');
            });
            //clear modal cache, so that new content can be loaded and clear old content so it won't show before new
            $('#modalContainer').on('hidden.bs.modal', function () {
                $(this).removeData('bs.modal').children('.modal-content').html('');
            });
        },
        showAlert: function (message) {
            localAlert(message)
        },
        popupStart: function () {
            $.validator.unobtrusive.parse('#popupForm');
            $("#save-btn").click(function () {
                if (!$("#popupForm").valid()) {
                    return false;
                }
            });
            $('input#RegNr').change(function () {
                this.value = this.value.toUpperCase();
            });
            var input = $('#popupResult'), result = input.val(), data = result.split('|');
            if (result) {
                localAlert(data[1]);
                if (data[0] === 'success') {
                    $('#modalContainer').modal('hide');
                    $('#vehiclefilter').closest('form').submit();
                }
            }
        }
    };
})(jQuery);

$(function(){
    garage.init();
});
$(function () {

    var PlaceHolderElement = $('#PlaceHolderHere');
    $('button[data-toggle="ajax-modal"]').off().click(function (event) {

        console.log("It's run");
        var url = $(this).data('url');
        var decodeUrl = decodeURIComponent(url);
        $.get(decodeUrl).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show');
        })
    })

    PlaceHolderElement.on('click', '[data-bs-save="modal"]', function (event) {

        var formData = new FormData($('#modalForm').get(0));
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        console.log(actionUrl);
        $.ajax({
            url: actionUrl,
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (data) {
                PlaceHolderElement.find('.modal').modal('hide');
            }

        })
        //PlaceHolderElement.find('.modal').modal('hide');
    })
})
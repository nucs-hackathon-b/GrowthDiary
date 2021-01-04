$(() => {
    $('.btn-remove-image').click(function (e) {
        e.preventDefault();
        let input = $('<input>');
        input.attr('type', 'text');
        input.attr('name', 'ImagesToRemove[]');
        input.prop('hidden', true);
        $('#hidden-fields').append(input);
        input.val($(this).attr('data-image-url'));
        $('img[src="' + $(this).attr('data-image-url') + '"]').remove();
        $(this).remove();
        return false;
    })

    $('.link-remove-tag').click(function (e) {
        e.preventDefault();
        $(this).parent().remove();
        return false;
    });

    $('#btn-upload-image').click(function (e) {
        e.preventDefault();
        let input = $('<input>');
        input.attr('type', 'file').prop('multiple', true).prop('hidden', true)
            .attr('accept', 'image/*');
        input.on('change', handleUploadImage);
        input.click();
    });

   

    $('#btn-add-tag').click(function (e) {
        e.preventDefault();
        let input = $('<input>');
        input.attr('type', 'text').attr('name', 'Tags[]').val($('#input-tag').val());
        //input.prop('disabled', true);
        input.prop('hidden', true);
        $('#input-tag').val('');
        let span = $('<span></span>');
        span.addClass('mr-1 mb-3').addClass('badge').addClass(randomColor());
        span.text(input.val());
        let a = $('<a></a>');
        a.addClass('text-decoration-none').addClass('link-remove-tag').text('×').attr('href', '#');
        //a.click(function (e) {
        //    e.preventDefault();
        //    $(this).parent().remove();
        //    return false;
        //})
        span.append(a);
        $('#div-tag').append(span);
        span.append(input);
    })

    $('#form-edit-post').on('submit', function () {
        let dt = new DataTransfer();
        $('input:file.input-upload-image').each(function () {
            let file = this.files[0];
            dt.items.add(file);
            $(this).remove();
        })
        $('input:file#files').prop('files', dt.files);
    });
})


function handleUploadImage() {
    if (this.files.length > 0) {
        for (const file of this.files) {
            let div = $('<div></div>');
            let img = $('<img>');
            img.attr('src', URL.createObjectURL(file))
                .addClass('img-selected')
                .on('load', function () {
                    URL.revokeObjectURL(this.src);
                });
            let input = $('<input>');
            input.attr('type', 'file').prop('hidden', true)
                .addClass('input-upload-image');
            let dt = new DataTransfer();
            dt.items.add(file);
            input.prop('files', dt.files);
            let btn = $('<button></button>');
            btn.addClass('btn').addClass('btn-danger')
                .text('Remove')
                .on('click', function () { div.remove(); });
            div.append(img).append(input).append(btn);
            $('#div-images').append(div);
        }
    }
}

function randomColor() {
    let colors = [
        "bg-primary text-light", "bg-secondary text-light", "bg-success text-light", "bg-danger text-light",
        "bg-warning text-dark", "bg-info text-dark", "bg-light text-dark", "bg-dark text-light"
    ];
    return colors[Math.floor(Math.random() * colors.length)];
}
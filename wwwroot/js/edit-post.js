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
    })
})
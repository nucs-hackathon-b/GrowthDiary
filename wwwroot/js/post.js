


$(() => {
    $('#form-create-post').on('submit', function () {
        let dt = new DataTransfer();
        $('input:file.input-upload-image').each(function () {
            let file = this.files[0];
            dt.items.add(file);
            $(this).remove();
        })
        $('input:file#files').prop('files', dt.files);
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
        span.addClass('m-1').addClass('badge').addClass(randomColor());
        span.text(input.val());
        let a = $('<a></a>');
        a.addClass('text-decoration-none').text('×').attr('href','#');
        a.click(function (e) {
            e.preventDefault();
            $(this).parent().remove();
            return false;
        })
        span.append(a);
        $('#div-tag').append(span);
        $('#div-tag').append(input);
    })

    $('body').on('click', 'a.like-link', function () {
        console.log(this);
        var url = $(this).attr('href').toLowerCase();
        $.ajax({
            url: url,
            type: 'POST',
            async: false,
            cache: false,
            processData: false,
            contentType: false,
            success: (data) => {
                if (data == 'like') {
                    $(this).addClass('btn-outline-danger').removeClass('btn-outline-info');
                    $(this).attr('href', url.replace('like', 'unlike'));
                    $(this).text('Unlike');
                } else {
                    $(this).addClass('btn-outline-info').removeClass('btn-outline-danger');
                    $(this).attr('href', url.replace('unlike', 'like'));
                    $(this).text('Like');
                }
            }
        });
        return false;
    });

    var currentId = 0;
    var time = Date.now();
    var inReplyToId = parseInt(window.location.pathname.split(/(\\|\/)/g).pop());

    if (document.body.scrollHeight <= document.body.clientHeight) {
        load();
    }
    $(window).scroll(function () {
        if (Math.ceil($(window).scrollTop()) >= $(document).height() - $(window).height()) {
            load();
        }
    })

    function load() {
        var data = new FormData();
        data.append('timestamp', time);
        data.append('currentId', currentId);
        if (inReplyToId != NaN) {
            data.append('inReplyToId', inReplyToId);
        }

        $.ajax({
            url: "/posts/load",
            type: 'POST',
            data: data,
            async: false,
            cache: false,
            processData: false,
            contentType: false,
            success: (data) => {
                $("#post-list").html(function (index, oldhtml) {
                    var div = $('<div></div>');
                    div.html(data);
                    currentId += div.find('li').length;
                    $(this).html(oldhtml + data);
                });
            },
            error: function () {
                $(window).off('scroll');
            }
        });
    }


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
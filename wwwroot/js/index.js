$(document).ready(function () {
    var currentId = "btn0";

    var $loading = $('#loadingDiv').hide();
    $(document)
        .ajaxStart(function () {
            $loading.show();
        })
        .ajaxStop(function () {
            $loading.hide();
        });

    $("#formPostsSearch").submit(function (event) {
        event.preventDefault();

        $.ajax({
            url: "/posts/getpostsdata",
            data: { search: $("#searchTxt").val(), tags: $("#tagsTxt").val() },
            type: "GET",
            success: function (data) {
                $("#postTable").html(data);
                addLikeButtons();
                //alert(data);
            },
            error: failedSearch
        });

        function failedSearch() {
            $("#postTable").html = "<p>Oops</p>";
            ///alert();
        }

    });

    $("#formPostsSearch").submit();

    var btns = document.getElementsByClassName("orderBtn");

    Array.from(btns).forEach(function (btn) {
        btn.addEventListener("click", function () {
            //document.cookie = "order=" + btn.getAttribute("order");
            //document.cookie = "descending=" + btn.getAttribute("desc");
            resetButtons();
            btn.style.backgroundColor = "#230046";
            setCookie("order", btn.getAttribute("order"), 1);
            setCookie("descending", btn.getAttribute("desc"), 1);
            $("#formPostsSearch").submit();
        });
    });

    function addLikeButtons() {
        var likeBtns = document.getElementsByClassName("likeBtn");
        //alert(likeBtns.length);

        Array.from(likeBtns).forEach(function (btn) {


            btn.addEventListener("click", function () {
                $.ajax({
                    url: "/posts/likeinc/" + btn.getAttribute("btnId") + "?inc=1",
                    type: "POST",
                    async: true,
                    success: function (data) {
                        $("#formPostsSearch").submit();
                    },
                    error: function (xhr) {
                        alert('Error: ' + xhr.statusText + " " + data);
                    }
                });
            });
        });
    }

    function resetButtons() {
        var orderBtns = document.getElementsByClassName("orderBtn");

        Array.from(orderBtns).forEach(function (btn) {
            btn.style.backgroundColor = "#0026ff";
        });
    }
});

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    function checkCurrencyValid() {
        var needUpdateCurrency = $.cookie('NeedUpdateCurrency');

        if (needUpdateCurrency.toLowerCase() == "false") {
            return;
        }
        $("#needUpdateCurrencyModal").modal()
    }

    $("#updateBtn").on("click", () => {
        $.ajax({
            type: "POST",
            url: "Currency/Update",
            success: function (msg) {
                console.log(msg);
            },
            error: function (req, status, error) {
                console.log(msg);
            }
        }); 
    });

    checkCurrencyValid();

    $("#storageLink").on("click", function (event) {
        event.preventDefault();

        $("#ModalStorage").modal();
        var MainContainer = $("#contentWrapp");
        var url = $(this).attr("href");
        $.ajax({
            type: "POST",
            url: url,
            success: function (msg) {
                MainContainer.html(msg)                
            },
            error: function (req, status, error) {
                console.log(msg);
            }
        }); 
    });

    $('#ModalStorage').on('click', 'a', function () {
        event.preventDefault();
        var MainContainer = $("#contentWrapp");
        var method = "GET";
        console.log($(this).data("method"));
        console.log($(this).data("method") == "post");
        if ($(this).data("method") == "post") {

            method = "POST";
        }

        var url = $(this).attr("href");
        $.ajax({
            type: method,
            url: url,
            success: function (msg) {
                MainContainer.html(msg)                
            },
            error: function (req, status, error) {
                console.log(req);
                console.log(status);
                console.log(error);
            }
        }); 
    });
});
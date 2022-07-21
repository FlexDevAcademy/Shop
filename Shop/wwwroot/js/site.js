$(document).ready(function () {
    $('#Addtobag').click(function () {
        let id = $(this).attr("data-id");
        console.log(id)

        $(this).addClass("disabled")

        $.ajax({
            type: "POST",
            url: "/ShoppingBag/AddToShoppingBag/" + id,
            success: function (msg) {
                console.log(msg);
            },
            error: function (req, status, error) {
                alert(error);
            }
        });

    })
});

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

    $('.removeFromShoppingBag').click(function () {
        let button = $(this)
        let id = $(this).attr("data-id");
        console.log(id)

        $.ajax({
            type: "POST",
            url: "/ShoppingBag/DeleteFromShoppingBag/" + id,
            success: function (msg) {
                $(button).closest("tr").remove()
            },
            error: function (req, status, error) {
                alert(error);
            }
        });
    })
});

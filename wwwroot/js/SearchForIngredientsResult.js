
$('.btn-addIngredient').each(function (event) {
    $(this).click(function (e) {
        var ingredient = {
            Id: $(this).data("id"),
            Name: $(this).data("name"),
            Quantity: getQuantity($(this).data("id")),
            Unit: getUnit($(this).data("id"))
        };

        AddIngredient(ingredient);
    })
})

function getQuantity(id) {
    return $('input[data-quantity="' + id + '"]').val();
}

function getUnit(id) {
    var idAndName = $('select[data-unit="' + id + '"]').val();
    var Unit = {
        Name: idAndName.slice(37),
        Id: idAndName.slice(0, 37)}
    return Unit;
}

function AddIngredient(ingredient) {
    $.ajax({
        url: '/Ingredient/AddIngredient',
        type: 'POST',
        cache: false,
        async: true,
        dataType: "html",
        data: ingredient

    })
        .done(function (result) {
            $('#addedIngredients').html(result);
        }).fail(function (xhr) {
            console.log('error : ' + xhr.status + ' - '
                + xhr.statusText + ' - ' + xhr.responseText);
        });
}

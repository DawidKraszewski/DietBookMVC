
$('.btn-Del').each(function (a) {
    $(this).click(function (e) {
        var ingredient = {
            Id: $(this).data("delete")
        };

        DeleteIngredient(ingredient);
    })
})

function DeleteIngredient(ingredient) {
    $.ajax({
        url: '/Ingredient/DeleteIngredient',
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
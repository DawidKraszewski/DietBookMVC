
$('#addIngredientFromInput').on('click', function (e) {
    
    var ingredient = { Name: $('#ingredientToAdd').val() };
    AddIngredientFromInput(ingredient);
    $('#ingredientToAdd').val('');
    
});

function AddIngredientFromInput(ingredient) {
    $.ajax({
        url: '/Ingredient/AddIngredientFromInput',
        type: 'POST',
        cache: false,
        async: true,
        dataType: "html",
        data: ingredient
    })
        .done(function (result) {
            $('#searchResult').html(result);
        }).fail(function (xhr) {
            console.log('error : ' + xhr.status + ' - '
                + xhr.statusText + ' - ' + xhr.responseText);
        });
}

$('#ingredientToAdd').bind('keyup', function (event) {
    if (event.keyCode == 13) {
        event.preventDefault();
        $("#addIngredientFromInput").click();
        return false;
    }
});
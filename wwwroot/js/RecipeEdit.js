
$(function () {
    GetIngredients();
});

function GetIngredients() {
    $.ajax({
        url: '/Ingredient/GetIngredientsForRecipe',
        type: 'GET',
        cache: false,
        async: true,
        dataType: "html"

    })
        .done(function (result) {
            $('#ingredients').html(result);
        }).fail(function (xhr) {
            console.log('error : ' + xhr.status + ' - '
                + xhr.statusText + ' - ' + xhr.responseText);
        });
}

function stopRKey(evt) {
    var evt = (evt) ? evt : ((event) ? event : null);
    var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
    if ((evt.keyCode == 13) && (node.type == "text")) { return false; }
}
document.onkeypress = stopRKey;
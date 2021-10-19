
$(function () {
    GetIngredients();
});

$('#btnSearch').on('click', function (e) {
    var searchString = { searchString: $('#searching').val() };
    SearchForIngredients(searchString);
});

function GetIngredients() {
    $.ajax({
        url: '/Ingredient/SearchForIngredientsIndex',
        type: 'POST',
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

function SearchForIngredients(searchString) {
    $.ajax({
        url: '/Ingredient/SearchForIngredients',
        type: 'POST',
        cache: false,
        async: true,
        dataType: "html",
        data: searchString
    })
        .done(function (result) {
            $('#ingredients').html(result);
        }).fail(function (xhr) {
            console.log('error : ' + xhr.status + ' - '
                + xhr.statusText + ' - ' + xhr.responseText);
        });
}

$('#searching').bind('keyup', function (event) {
    if (event.keyCode == 13) {
        event.preventDefault();
        $("#btnSearch").click();
        return false;
    }
});
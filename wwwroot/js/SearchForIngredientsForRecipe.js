
$(function () {
    GetNewIngredientsInput();
    GetIngredientsIndex();
    GetAddedIngredients();
});

function GetAddedIngredients() {
    $.ajax({
        url: '/Ingredient/GetAddedIngredients',
        type: 'POST',
        cache: false,
        async: true,
        dataType: "html"

    })
        .done(function (result) {
            $('#addedIngredients').html(result);
        }).fail(function (xhr) {
            console.log('error : ' + xhr.status + ' - '
                + xhr.statusText + ' - ' + xhr.responseText);
        });
}

$('#btnSearch').on('click', function (e) {
    var searchString = { searchString: $('#searching').val() };
    SearchForIngredients(searchString);
});

$('#searching').bind('keyup', function (event) {
    if (event.keyCode == 13) {
        event.preventDefault();
        $("#btnSearch").click();
        return false;
    }
});

function SearchForIngredients(searchString) {
    $.ajax({
        url: '/Ingredient/SearchForIngredientsForRecipe',
        type: 'POST',
        cache: false,
        async: true,
        dataType: "html",
        data: searchString
    })
        .done(function (result) {
            $('#searchResult').html(result);
        }).fail(function (xhr) {
            console.log('error : ' + xhr.status + ' - '
                + xhr.statusText + ' - ' + xhr.responseText);
        });
}

function GetIngredientsIndex() {
    $.ajax({
        url: '/Ingredient/SearchForIngredientsForRecipeIndex',
        type: 'POST',
        cache: false,
        async: true,
        dataType: "html"

    })
        .done(function (result) {
            $('#searchResult').html(result);
        }).fail(function (xhr) {
            console.log('error : ' + xhr.status + ' - '
                + xhr.statusText + ' - ' + xhr.responseText);
        });
}

function GetNewIngredientsInput() {
    $.ajax({
        url: '/Ingredient/GetIngredientsInput',
        type: 'POST',
        cache: false,
        async: true,
        dataType: "html"

    })
        .done(function (result) {
            $('#addIngredient').html(result);
        }).fail(function (xhr) {
            console.log('error : ' + xhr.status + ' - '
                + xhr.statusText + ' - ' + xhr.responseText);
        });
}

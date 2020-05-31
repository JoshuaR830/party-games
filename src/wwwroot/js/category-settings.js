var lettersConnection = new signalR.HubConnectionBuilder().withUrl("/lettersHub").build();
var connectionName = "GroupOfJoshua";

lettersConnection.start().then(function () {
    lettersConnection.invoke("AddToGroup", connectionName);
});

$submitButton = document.querySelector('.js-submit-button');
$word = document.querySelector('.js-word-input');

$submitButton.addEventListener('click', function() {
    console.log($word.value);
    let category = document.querySelector('input[name="word-category"]:checked').value;
    console.log(category);
    lettersConnection.invoke("UpdateCategory", connectionName, $word.value, category);
    lettersConnection.invoke("SaveUpdatesToFile");
});
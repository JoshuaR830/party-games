var lettersConnection = new signalR.HubConnectionBuilder().withUrl("/lettersHub").build();

var connectionName = "GroupOfJoshua";
var wordContainer = document.querySelector('.js-word-container');
var $word = document.querySelector('.js-word');
var $definition = document.querySelector('.js-definition');
var $definitionButton = document.querySelector('.js-definition-update-button');
var $tickButton = document.querySelector('.js-tick');
var $crossButton = document.querySelector('.js-cross');

lettersConnection.start().then(function () {
    lettersConnection.invoke("AddToGroup", connectionName);
    lettersConnection.invoke("GetGuessedWords", connectionName);
});

lettersConnection.on("ReceiveGuessedWord", function(data){
    let parsedData = JSON.parse(data);
    console.log(parsedData.Words);
    
    console.log(parsedData.Words[0]);
    
    console.log(parsedData.Words.length)
    
    let html = "";
    wordContainer.innerHTML = html;

    parsedData.Words.forEach(function(val) {
        console.log(val);
        console.log(val.Word, val.Status);
        
        let wordItem = document.createElement('div');
        
        wordItem.textContent = val.Word;
        wordItem.className = (val.Status === 4 ? 'word-error' : 'word-ticked');
        
        wordItem.addEventListener('click', function($el) {
            lettersConnection.invoke("GetDefinition", connectionName, val.Word);
        })
        
        wordContainer.appendChild(wordItem);
        
    });
});


lettersConnection.on("ReceiveDefinition", function(definition, word) {
    
    $word.textContent = word;
    $definition.value = definition;
    
    console.log(definition, word)
});

$definitionButton.addEventListener('click', function() {
    console.log($word.textContent);
    console.log($definition.value);
    lettersConnection.invoke("UpdateDictionary", connectionName, $word.textContent, $definition.value);
})

$tickButton.addEventListener('click', function() {
    lettersConnection.invoke("WordTicked", $word.textContent, connectionName, true);
})

$crossButton.addEventListener('click', function() {
    lettersConnection.invoke("WordTicked", $word.textContent, connectionName, false);
})
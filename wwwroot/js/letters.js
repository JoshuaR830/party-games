var connection = new signalR.HubConnectionBuilder().withUrl("/lettersHub").build();

var letterSpaces = [];
var letterOrigins = [];
var wordsCreated = [];

var confirmWord = document.querySelector('.js-confirm-word');
var wordList = document.querySelector('.js-word-list');


var groupName = "GroupOfJoshua";

connection.start().then(function () {
    connection.invoke("AddToGroup", groupName);
});

document.querySelector('.js-letters-start').addEventListener('click', function(event) {
    console.log("Hello");
    connection.invoke("ChooseLetters", groupName);
});

connection.on("LettersForGame", function(jsonLetters) {
    confirmWord.classList.remove('hidden');
    document.querySelector('.js-letters-start').classList.add('hidden');
    var chosenLetters = JSON.parse(jsonLetters);
    console.log(chosenLetters);
    let container = document.querySelector('.js-random-letters-container');
    let letterInputRow = document.querySelector('.js-letter-input-row');
    container.innerHTML = "";
    letterInputRow.innerHTML = "";
    letterSpaces = [];
    letterOrigins = [];
    wordsCreated = [];
    wordList.innerHTML = "";
    for (let i = 0; i < chosenLetters.length; i ++) {
        chosenLetters[i];
        // <div class="letter-container letter-choice populated js-letter random-letter-0" data-origin="0">A</div>
        
        let letterContainer = document.createElement('div');
        letterContainer.className = `letter-container letter-choice populated js-letter random-letter-${i}`;
        letterContainer.setAttribute('data-origin', `${i}`);
        letterContainer.textContent = chosenLetters[i];
        container.appendChild(letterContainer);

        let letterSelectionContainer = document.createElement('div');
        letterSelectionContainer.className = `letter-container letter-${i} js-selected-letter`;
        letterSelectionContainer.setAttribute('data-letter', `${i}`);
        letterInputRow.appendChild(letterSelectionContainer);

        letterSpaces.push('');
        letterOrigins.push('');
        // <div class="letter-container letter-0 js-selected-letter" data-letter="0"></div>
        
    }

    var letters = document.querySelectorAll('.js-letter');
    var selectedLetters = document.querySelectorAll('.js-selected-letter');
    
    letters.forEach(function($el) {
        $el.addEventListener('click', function(event) {
            if ($el.classList.contains('populated')) {
                $el.classList.remove("populated");
                let origin = $el.dataset.origin;
                let letter = $el.textContent;
                console.log(letter);
                placeNextLetter(letter, origin)
                $el.textContent = '';
            }
        })
    })
    
    selectedLetters.forEach(function($el) {
        $el.addEventListener('click', function(event) {
            if ($el.classList.contains('populated')) {
                let letter = $el.dataset.letter; // This is the wrong thing - this could be up to 5
                console.log($el.dataset.letter)
                console.log(letterSpaces[$el.dataset.letter]);
                console.log(letterOrigins[$el.dataset.letter]);
                $el.classList.remove('populated');
                let origin = letterOrigins[letter];
                letters[origin].textContent = letterSpaces[letter];
                letterOrigins[letter] = '';
                letterSpaces[letter] = '';
                letters[origin].classList.add('populated');
                $el.textContent = '';
            }
    
        })
    })
})

function placeNextLetter(letter, origin) {
    for (let i = 0; i < letterSpaces.length; i++) {
        if (letterSpaces[i] === '') {
            console.log(`.letter-${i}`);
            letterOrigins[i] = origin;
            let currentLetter = document.querySelector(`.letter-${i}`);
            currentLetter.classList.add('populated');
            currentLetter.textContent = letter;
            letterSpaces[i] = letter;
            break;
        }
    }
    console.log(letterSpaces)
    console.log(letterOrigins)
}


confirmWord.addEventListener('click', function() {
    var currentWord = letterSpaces.join('');
    wordsCreated.push(currentWord);
    var word = document.createElement('div');
    word.textContent = currentWord;
    word.className = "word-list-item";
    wordList.appendChild(word);

    connection.invoke("IsValidWord", currentWord, groupName);
});

connection.on("WordStatusResponse", function(status) {
    console.log(status);
})


// var xhttp = new XMLHttpRequest()
// xhttp.onreadystatechange = function() {
//     if(this.readyState === 4 && this.status === 200)
//     {
//         console.log("success");
//     }

//     if(this.readyState === 4 && this.status === 404)
//     {
//         console.log("failed")
//     }
// }

// xhttp.open("GET", "https://www.dictionary.com/browse/hello");
// // xhttp.setRequestHeader('Content-type', 'application/json; charset=utf-8');
// xhttp.setRequestHeader('Access-Control-Allow-Origin', '*');
// xhttp.send();

// fetch("https://www.dictionary.com/browse/hello");
// fetch("https://stackoverflow.com/");
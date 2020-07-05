var lettersConnection = new signalR.HubConnectionBuilder().withUrl("/lettersHub").build();
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

var letterSpaces = [];
var letterOrigins = [];
var wordsCreated = [];

var score = 0;

var confirmWord = document.querySelector('.js-confirm-word');
var wordList = document.querySelector('.js-word-list');
var definitionContainer = document.querySelector('.js-definition-container');
var wordToDefine = document.querySelector('.js-word-to-define');
var letterInputRow = document.querySelector('.js-letter-input-row');
var definitionSubmitButton = document.querySelector('.js-submit-new-definition');

var $loggedInUserContainer = document.querySelector('.js-logged-in-container');
var $loggedInUserItemsContainer = document.querySelector('.js-logged-in-items-container');

var connectionName = "GroupOfJoshua";

window.addEventListener('load', function(){
    console.log("Loaded");
})

lettersConnection.start().then(function () {
    lettersConnection.invoke("AddToGroup", connectionName);
});

connection.start().then(function () {
    connection.invoke("AddToGroup", connectionName);
})

window.addEventListener('load', function() {
    document.querySelector('#my-name').focus();
})

document.querySelector('#my-name').addEventListener('keydown', function(event) {
    console.log("Key code");
    if (event.keyCode === 13){
        console.log("Trigger events");
        let name = document.querySelector('#my-name').value;
        lettersConnection.invoke("Startup", connectionName, name, 1);
        lettersConnection.invoke("SetupNewUser", connectionName, name);
    }
});

document.querySelector('.js-login-button').addEventListener('click', function() {
    console.log("Trigger events")
    let name = document.querySelector('#my-name').value;
    lettersConnection.invoke("Startup", connectionName, name, 1);
    lettersConnection.invoke("SetupNewUser", connectionName, name);
});

connection.on("ReceiveCompleteRound", function() {
    console.log("ReceiveCompletedRound");
    var round = document.createElement('div');
    round.textContent = `Round ${gameRoundNumber}`;
    round.className = "round-title";
    document.getElementById("messageList").appendChild(round);
    document.getElementById('playAgain').classList.remove('hidden');
    stopTimer();
    letterInputRow.classList.add('hidden');
    confirmWord.classList.add('hidden');

    let selectedLetters = document.querySelectorAll('.js-selected-letter');
    var letters = document.querySelectorAll('.js-letter');

    selectedLetters.forEach(function($el) {
        
        if ($el.classList.contains('populated')) {
            let letter = $el.dataset.letter; // This is the wrong thing - this could be up to 5
            console.log($el.dataset.letter);
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

document.querySelector('.js-letters-start').addEventListener('click', resetWordGame);

document.querySelector('#startGame').addEventListener('click', resetWordGame);
document.querySelector('#play-again-option').addEventListener('click', resetWordGame);
document.querySelector('.js-letters-continue').addEventListener('click', launchWordGame);

document.getElementById('playAgain').addEventListener('click', function() {
    connection.invoke("CollectScores", connectionName);
    lettersConnection.invoke("RoundComplete", connectionName);
});

function launchWordGame() {
    gameRoundNumber ++;
    lettersConnection.invoke("GetUserData", connectionName, document.getElementById('my-name').value)
    let timerMins = parseInt(document.getElementById('set-minutes').value);
    let timerSecs = parseInt(document.getElementById('set-seconds').value);

    let time = [timerMins, timerSecs];
    connection.invoke("SendTime", connectionName, time);
    // lettersConnection.invoke("ChooseLetters", connectionName);
}

document.getElementById('playAgainFab').addEventListener('click', function() {
    resetWordGame();
})

function resetWordGame() {
    lettersConnection.invoke("ResetGame", connectionName, document.querySelector('#my-name').value, 1);
    launchWordGame();
}


lettersConnection.on("ReceiveUserData", function(letters, words, letterCount, wordCount) {
    let chosenLetters = JSON.parse(letters);
    console.log(chosenLetters);
    console.log("Hi");
    console.log(chosenLetters);
    console.log(">>>", words);
    console.log("Bye");

    document.getElementById('playAgainFab').classList.add('hidden');
    document.querySelector('.js-letters-continue').classList.add('hidden');

    document.querySelector('.js-word-list').classList.remove('hidden');
    document.querySelector('.js-home-screen-container').classList.add('hidden');
    wordToDefine.classList.add("hidden");
    definitionContainer.classList.add("hidden");
    document.querySelector('#options').classList.add('hidden');
    document.querySelector('.js-letter-game-container').classList.remove('hidden');
    document.querySelector('#startGame').classList.add('hidden');
    confirmWord.classList.remove('hidden');
    document.querySelector('.js-letters-start').classList.add('hidden');
    // var chosenLetters = JSON.parse(jsonLetters);
    console.log(chosenLetters);
    let container = document.querySelector('.js-random-letters-container');
    letterInputRow.classList.remove('hidden');
    container.innerHTML = "";
    letterInputRow.innerHTML = "";
    letterSpaces = [];
    letterOrigins = [];
    wordsCreated = [];
    wordList.innerHTML = "";
    for (let i = 0; i < letterCount; i ++) {
        chosenLetters[i].Letter;

        let letterContainer = document.createElement('div');
        let myName = document.querySelector('#my-name').value;
        letterContainer.className = `letter-container letter-choice populated js-letter ${myName === "Andrew" ? `random-letter-${i}` : ""}`;
        if (myName.toLowerCase() === "andrew") {
            document.querySelector(`.js-random-letters-container`).style = "height: 260px; width: 260px; border-radius: 50%; border: 2px solid #55edba5b; box-sizing: content-box; box-shadow: 0 0 10px #55edba5b;"
            let angle = (2 * Math.PI) / chosenLetters.length;
            let xPos = (260 / 2) - 30;
            let yPos = (260 / 2) - 30;
            let radius = 100;
            letterContainer.style = ""
            let x = Math.round(radius * (Math.sin(i * angle))) + xPos;
            let y = Math.round(radius * (Math.cos(i * angle))) + yPos;
            letterContainer.style = `position: absolute; top: ${y}px; left: ${x}px; margin-top: 0; width: 60px; border-radius: 50%;`;
        }
        letterContainer.setAttribute('data-origin', `${i}`);
        letterContainer.setAttribute('data-score', `${chosenLetters[i].Score}`);
        letterContainer.textContent = chosenLetters[i].Letter;
        container.appendChild(letterContainer);

        let letterSelectionContainer = document.createElement('div');
        letterSelectionContainer.className = `letter-container letter-${i} js-selected-letter`;
        letterSelectionContainer.setAttribute('data-letter', `${i}`);
        letterInputRow.appendChild(letterSelectionContainer);

        letterSpaces.push('');
        letterOrigins.push('');
    }

    var letters = document.querySelectorAll('.js-letter');
    var selectedLetters = document.querySelectorAll('.js-selected-letter');

    letters.forEach(function($el) {
        $el.addEventListener('click', function(event) {
            if ($el.classList.contains('populated')) {
                $el.classList.remove("populated");
                let origin = $el.dataset.origin;
                let letter = $el.textContent;
                let score = $el.dataset.origin;
                console.log(letter);
                placeNextLetter(letter, origin, score)
                $el.textContent = '';
            }
        })
    })

    selectedLetters.forEach(function($el) {
        $el.addEventListener('click', function(event) {
            if ($el.classList.contains('populated')) {
                let letter = $el.dataset.letter; // This is the wrong thing - this could be up to 5
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

    let serializedWords = JSON.parse(words);
    for(let i = 0; i < wordCount; i++ ) {
        console.log();
        let currentWord = serializedWords[i].Word;
        
        wordsCreated.push(currentWord);
        let num = wordsCreated.length - 1;
        var word = document.createElement('div');
        word.textContent = currentWord;
        console.log(serializedWords[i].IsValid);
        word.className = `word-list-item word-${num} ${serializedWords[i].IsValid?"": "word-error"}`;
        word.setAttribute('data-score', serializedWords[i].Score);

        word.addEventListener('click', function () {
            var statusToSet = true;
            if (word.classList.contains('word-ticked')) {
                statusToSet = false;
            }
            lettersConnection.invoke("WordTicked", currentWord, connectionName, statusToSet);
        })

        wordList.appendChild(word);
    }
    
});

connection.on("CompletedScores", function() {
    submitScores();
    
    console.log("Hello");
    document.getElementById('playAgain').classList.add('hidden');
    document.getElementById('startGame').classList.add('hidden');
    document.getElementById('playAgainFab').classList.remove('hidden');
    document.querySelector('.js-letter-game-container').classList.add('hidden');
    document.getElementById("options").classList.remove('hidden');
    document.querySelector('.js-home-screen-container').classList.remove('hidden');
})

function submitScores() {
    let name = document.getElementById('my-name').value; 

    console.log("Score sent");

    let userScore = calculateScore();

    var user = connectionName;

    connection.invoke("SendScore", user, name, userScore).catch(function (err) {
        return console.error(err.toString());
    });
}

function calculateScore() {
    var myScore = 0;
    document.querySelectorAll('.word-list-item').forEach(function($el){
        if(!$el.classList.contains("word-error")) {
            console.log($el.dataset.score);
            myScore += parseInt($el.dataset.score);
        } else {
            console.log("Word error");
        }
    })
    return myScore;
}

function placeNextLetter(letter, origin, score) {
    for (let i = 0; i < letterSpaces.length; i++) {
        if (letterSpaces[i] === '') {
            console.log(`.letter-${i}`);
            letterOrigins[i] = origin;
            let currentLetter = document.querySelector(`.letter-${i}`);
            currentLetter.classList.add('populated');
            currentLetter.setAttribute('data-score', score);
            currentLetter.textContent = letter;
            letterSpaces[i] = letter;
            break;
        }
    }
    console.log(letterSpaces)
    console.log(letterOrigins)
}

function removeConfirmed()
{
    document.querySelectorAll('.js-selected-letter.populated').forEach(function($el) {
        $el.classList.remove("confirmed");
        $el.classList.remove("alert-user");
    })
}

confirmWord.addEventListener('click', function() {
    let currentWord = letterSpaces.join('');

    if (wordsCreated.includes(currentWord)){
        document.querySelectorAll('.js-selected-letter.populated').forEach(function($el) {
            $el.classList.add("alert-user");
        })
    
        setTimeout(removeConfirmed, 150);
        return;
    }

    let wordScore = 0;
    document.querySelectorAll('.js-selected-letter.populated').forEach(function($el) {
        $el.classList.add("confirmed");
        console.log($el.dataset.score);
        wordScore += parseInt($el.dataset.score);
    })

    setTimeout(removeConfirmed, 150);

    wordsCreated.push(currentWord);
    let num = wordsCreated.length - 1;
    var word = document.createElement('div');
    word.textContent = currentWord;
    word.className = `word-list-item word-${num} word-pending`;
    word.setAttribute('data-score', wordScore);

    word.addEventListener('click', function() {
        var statusToSet = true;
        if(word.classList.contains('word-ticked')) {
            statusToSet = false;
        }
        lettersConnection.invoke("WordTicked", currentWord, connectionName, statusToSet);
    })

    wordList.appendChild(word);

    lettersConnection.invoke("ServerIsValidWord", currentWord, connectionName, document.getElementById('my-name').value);

    var wordListContainer = document.querySelector('.js-word-list');
    var scrollWidth = wordListContainer.scrollWidth;
    console.log(scrollWidth);
    wordListContainer.scrollTo()

    if(wordListContainer.scrollLeft !== scrollWidth) {
        wordListContainer.scrollTo((scrollWidth), 0);
    }
});

lettersConnection.on("WordStatusResponse", function(status, word) {
    console.log(status, word);
    if (status === false) {
        let wordPos = getWordPosition(word);
        if(wordPos < 0) {
            return;
        }
        document.querySelector(`.word-${wordPos}`).classList.remove("word-pending");
        document.querySelector(`.word-${wordPos}`).classList.add("word-error");
    } else {
        let wordPos = getWordPosition(word);
        if(wordPos < 0) {
            return;
        }

        document.querySelector(`.word-${wordPos}`).classList.remove("word-pending");
    }
})

lettersConnection.on("TickWord", function(word) {

    wordToDefine.classList.remove("hidden");
    wordToDefine.textContent = word;

    let wordPos = getWordPosition(word)
    if(wordPos < 0) {
        return;
    }
    console.log("Word ticked");

    let wordListItem = document.querySelector(`.word-${wordPos}`);
    
    if(wordListItem.classList.contains('word-ticked')) {
        console.log("Add word error")
        wordListItem.classList.add("word-error");
        wordListItem.classList.remove("word-ticked");
    }else {
        console.log("Remove error");
        wordListItem.classList.add("word-ticked");
        wordListItem.classList.remove("word-error");
    }
})

wordToDefine.addEventListener('click', function(event) {
    console.log(event.currentTarget.textContent)
    let word = event.currentTarget.textContent;
    
    lettersConnection.invoke("GetDefinition", connectionName, word);
})

definitionSubmitButton.addEventListener('click', function() {
    let word = definitionSubmitButton.dataset.word;
    let definition = document.querySelector('.js-definition-update').value;
    document.querySelector('.js-no-definition-container').classList.add('hidden');
    lettersConnection.invoke("AddWordToDictionary", connectionName, word, definition);
})

lettersConnection.on("ReceiveDefinition", function(definition, word) {

    if (definition === "" || definition === null)
    {
        document.querySelector('.js-definition-container').classList.add('hidden');
        document.querySelector('.js-no-definition-container').classList.remove('hidden');
        definitionSubmitButton.setAttribute("data-word", word);
        definitionSubmitButton.textContent = `Submit ${word}`;
    } else {
        
        var definitionContainer = document.querySelector('.js-definition-container');
        definitionContainer.classList.remove('hidden');
        definitionContainer.textContent = `${word}: ${definition}`;
    }
})


function getWordPosition(word) {
    let wordPos = wordsCreated.indexOf(word)
    if (wordPos < 0){
        console.log("does not contain word");
        return -1;
    }
    return wordPos;
}

document.querySelector('#completeRoundButton').addEventListener('click', function () {
    lettersConnection.invoke("RoundComplete", connectionName);
    console.log('Completed round button pressed invoke round complete');
});

document.querySelectorAll('.word-list-item').forEach(function($el) {
    $el.addEventListener()
});

lettersConnection.on("LoggedInUsers", function(users) {
    $loggedInUserItemsContainer.innerHTML = "";

    users.forEach(function(user) {
        let loggedInUserItem  = document.createElement('div');
        loggedInUserItem.className = 'logged-in-item';
        loggedInUserItem.textContent = user;
        $loggedInUserItemsContainer.appendChild(loggedInUserItem);
    });

    console.log(users);
    console.log('logged-in');
});

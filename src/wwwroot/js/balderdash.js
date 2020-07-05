var balderdashConnection = new signalR.HubConnectionBuilder().withUrl("/balderdashHub").build();
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

var connectionName = "GroupOfJoshua";

var $loggedInUserContainer = document.querySelector('.js-logged-in-container');
var $loggedInUserItemsContainer = document.querySelector('.js-logged-in-items-container');

var $nextArrow = document.querySelector('.js-next-card');
var $previousArrow = document.querySelector('.js-previous-card');
var $carousel = document.querySelector('.js-carousel');
var $contextTitle = document.querySelector('.js-word-choice');

var $resetButton = document.querySelector('.js-reset-button');
var $submitAnswer = document.querySelector('.js-submit-answer');
var $answerEntry = document.querySelector('.js-answer-entry-box');

var $answerContainer = document.querySelector('.js-answer-container');
var $cardContainer = document.querySelector('.js-card-container');
var $waitingForPlayersContainer = document.querySelector('.js-waiting-for-players');
var $currentDasher = document.querySelector('.js-current-dasher');
var $answerTitle = document.querySelector('.js-answer-title');
var totalScoresToChoose = 0;

balderdashConnection.start().then(function () {

});

document.querySelector('.js-login-button').addEventListener('click', function() {
    let name = document.querySelector('#my-name').value;
    // Todo: change game type
    console.log(connectionName)
    balderdashConnection.invoke("StartUp", connectionName, name, 4);
    // balderdashConnection.invoke("PassToDasher", connectionName, name);
    // pixenaryConnection.invoke("JoinPixenaryGame", connectionName, name, 2);
});


connection.start().then(function () {
});

// document.querySelectorAll('')

$nextArrow.addEventListener('click', function(event) {
    if(event.currentTarget.classList.contains('--disabled')){
        return;
    }
    
    let cardNumber = parseInt($carousel.querySelector('.--active').dataset.cardNumber);
    let currentCard = $carousel.querySelector(`[data-card-number="${cardNumber}"]`);
    let nextCard = $carousel.querySelector(`[data-card-number="${cardNumber+1}"]`);

    $contextTitle.textContent = `${nextCard.dataset.user}'s guess`;
    
    currentCard.classList.add('hidden');
    currentCard.classList.remove('--active');
    nextCard.classList.add('--active');
    nextCard.classList.remove('hidden');

    $previousArrow.classList.remove('--disabled');

    nextCard = $carousel.querySelector(`[data-card-number="${cardNumber+2}"]`);
    if(nextCard === null)
    {
        $nextArrow.classList.add('--disabled');
    }
});

$previousArrow.addEventListener('click', function(event) {
    if(event.currentTarget.classList.contains('--disabled')){
        return;
    }
    
    let cardNumber = parseInt($carousel.querySelector('.--active').dataset.cardNumber);
    let currentCard = $carousel.querySelector(`[data-card-number="${cardNumber}"]`);
    let previousCard = $carousel.querySelector(`[data-card-number="${cardNumber-1}"]`);

    $contextTitle.textContent = `${previousCard.dataset.user}'s guess`;

    currentCard.classList.add('hidden');
    currentCard.classList.remove('--active');
    previousCard.classList.add('--active');
    previousCard.classList.remove('hidden');

    $nextArrow.classList.remove('--disabled');

    previousCard = $carousel.querySelector(`[data-card-number="${cardNumber-2}"]`);

    if(previousCard === null)
    {
        $previousArrow.classList.add('--disabled');
    }
});

$resetButton.addEventListener('click', function () {
    console.log("Clicked");
    balderdashConnection.invoke("BeginBalderdash", connectionName);
    let name = document.querySelector('#my-name').value;

    balderdashConnection.invoke('DisplayBalderdashScores', connectionName, name);
    console.log("Hello there")
});

$submitAnswer.addEventListener('click', function() {
    
    $answerContainer.classList.add('hidden');
    console.log($answerEntry.value);
    
    let name = document.getElementById('my-name').value;

    balderdashConnection.invoke("PassToDasher", connectionName, name, $answerEntry.value)
    // $specialContainer.classList.
});

balderdashConnection.on("ReceivedGuess", function(dasher) {
    console.log(dasher);
    $currentDasher.textContent = dasher;
    $answerContainer.classList.add('hidden');
    $waitingForPlayersContainer.classList.remove('hidden');
});

balderdashConnection.on("RevealCardsToDasher", function(guesses) {
    console.log("Here is the stuff")
    $cardContainer.classList.remove('hidden');
    console.log(guesses.length);
    
    if (guesses.length === 1) {
        $nextArrow.classList.add('--disabled');
    }

    console.log(guesses);
    $carousel.innerHTML = "";
    guesses.forEach(function(guess, index) {
        let $card = document.createElement('div');
        $card.className = "balderdash-card";
        $card.setAttribute('data-card-number', index);
        $card.setAttribute('data-user', guess.name);
        $card.setAttribute('data-player-who-proposed', guess.name);
        $card.textContent = guess.guess;

        if(index === 0) {
            $card.classList.add("--active")
            $contextTitle.textContent = `${guess.name}'s guess`;
        } else {
            $card.classList.add('hidden');
        }

        $carousel.appendChild($card);
    })
    // ToDo: only the dasher should see this
});

balderdashConnection.on("DasherSelected", function(dasher) {
    let name = document.querySelector('#my-name').value;
    if (name === dasher)
    {
        $answerTitle.innerText = "You are the dasher";
    } else {
        $answerTitle.innerText = `${dasher} is the dasher`;
    }
})

balderdashConnection.on("Reset", function() {
    $loggedInUserContainer.classList.add('hidden');
    $waitingForPlayersContainer.classList.add('hidden');
    $answerEntry.value = "";

    $answerContainer.classList.remove('hidden');

    $cardContainer.classList.add('hidden')
    $resetButton.classList.add('hidden');
    $answerContainer.classList.remove('hidden');

})


balderdashConnection.on("DisplayBalderdashScores", function(names)
{
    console.log("Hi")
    let usersToScore = document.querySelector('.js-score-user-selector');
    usersToScore.innerHTML = "";
    
    names.forEach(function(playerWhoGuessed) {
        console.log("Name");
        var $el = document.createElement('div');
        $el.className = "button";
        $el.addEventListener('click', function() {
            if ($el.classList.contains('--disabled')) {
                return;
            }
            
            let playerWhoProposed = $carousel.querySelector('.--active').dataset.playerWhoProposed;
            
            balderdashConnection.invoke("BalderdashScores", connectionName, playerWhoGuessed, playerWhoProposed);
            balderdashConnection.invoke("GetScoresForAllUsers", connectionName);
            $el.classList.add('--disabled');
            countActive(names);
        });

        $el.textContent = playerWhoGuessed;

        usersToScore.appendChild($el);
    })
})

function countActive(names) {
    let usersToScore = document.querySelector('.js-score-user-selector');
    console.log(usersToScore.querySelectorAll('.--disabled').length);
    console.log(names.length);
    console.log(names.length === usersToScore.querySelectorAll('.--disabled').length);
    
    if (names.length === usersToScore.querySelectorAll('.--disabled').length) {
        balderdashConnection.invoke("ResetGame", connectionName);
    }
}

balderdashConnection.on("UpdateUserScore", function (score) {
    console.log(score);
    document.getElementById('score').innerText = score;
})

balderdashConnection.on("LoggedInUsers", function(users) {
    $loggedInUserItemsContainer.innerHTML = "";
    
    users.forEach(function(user) {
        let loggedInUserItem  = document.createElement('div');
        loggedInUserItem.className = 'logged-in-item';
        loggedInUserItem.textContent = user;
        $loggedInUserItemsContainer.appendChild(loggedInUserItem);
    });
    
    console.log(users);
    console.log('logged-in');
})
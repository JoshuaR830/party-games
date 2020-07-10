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

$roundScoreContainer = document.querySelector('.js-round-score-container');

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
    
    if($answerEntry.value === "") {
        $answerEntry.classList.add('error');
        return;
    }

    $answerEntry.classList.remove('error');

    $answerContainer.classList.add('hidden');
    console.log($answerEntry.value);

    let name = document.getElementById('my-name').value;

    balderdashConnection.invoke("PassToDasher", connectionName, name, $answerEntry.value);

    
    
   
    // $specialContainer.classList.
});

balderdashConnection.on("ReceivedGuess", function(dasher) {
    console.log(dasher);
    let name = document.getElementById('my-name').value;

    if (name === dasher) {
        $currentDasher.textContent = `Waiting for other players to submit their answers`;
    } else {
        $currentDasher.textContent = `${dasher} has got your answer`;
    }
    
    $waitingForPlayersContainer.classList.remove('hidden');
    $answerContainer.classList.add('hidden');
});

balderdashConnection.on("RevealCardsToDasher", function(guesses) {
    console.log("Here is the stuff");
    $waitingForPlayersContainer.classList.add('hidden');
    $cardContainer.classList.remove('hidden');
    console.log(guesses.length);

    $previousArrow.classList.add('--disabled');

    if (guesses.length === 1) {
        $nextArrow.classList.add('--disabled');
    } else {
        $nextArrow.classList.remove('--disabled');
    }

    console.log(guesses);
    $carousel.innerHTML = "";
    guesses.forEach(function(guess, index) {
        let $card = document.createElement('div');
        let $cardBorder = document.createElement('div');
       
        $cardBorder.className = "content-border info-border --centered";
       
        $card.className = "balderdash-card inner-content";
        $cardBorder.setAttribute('data-card-number', index);
        $cardBorder.setAttribute('data-user', guess.name);
        $cardBorder.setAttribute('data-player-who-proposed', guess.name);
        $card.textContent = guess.guess;

        if(index === 0) {
            $cardBorder.classList.add("--active")
            $contextTitle.textContent = `${guess.name}'s guess`;
        } else {
            $cardBorder.classList.add('hidden');
        }

        $cardBorder.appendChild($card);
        $carousel.appendChild($cardBorder);
    })
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
    let name = document.querySelector('#my-name').value;
    $contextTitle.textContent = "";
    
    $roundScoreContainer.classList.add('hidden');
    
    balderdashConnection.invoke('DisplayBalderdashScores', connectionName, name);
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
        let $el = document.createElement('div');
        let $elBorder = document.createElement('div');

        $elBorder.className = "content-border button-border --centered js-submit-answer";
        
        $el.className = "inner-content button-border";
        $elBorder.addEventListener('click', function() {
            if ($el.classList.contains('--disabled')) {
                return;
            }
            
            let playerWhoProposed = $carousel.querySelector('.--active').dataset.playerWhoProposed;
            
            balderdashConnection.invoke("BalderdashScores", connectionName, playerWhoGuessed, playerWhoProposed);
            balderdashConnection.invoke("GetScoresForAllUsers", connectionName);
            $el.classList.add('--disabled');
            $elBorder.classList.add('--disabled');
            countActive(names);
        });

        $el.textContent = playerWhoGuessed;

        $elBorder.appendChild($el)

        usersToScore.appendChild($elBorder);
    })
})

function countActive(names) {
    let usersToScore = document.querySelector('.js-score-user-selector');
    console.log(usersToScore.querySelectorAll('.inner-content.--disabled').length);
    console.log(names.length);
    console.log(names.length === usersToScore.querySelectorAll('.inner-content.--disabled').length);
    
    if (names.length === usersToScore.querySelectorAll('.inner-content.--disabled').length) {
        balderdashConnection.invoke("DisplayRoundInformation", connectionName);
    }
}

balderdashConnection.on("RoundReviewPage", function(userRoundScores) {
    $roundScoreContainer.innerHTML = "";
    
    for (let user in userRoundScores) {
        if(userRoundScores.hasOwnProperty(user))
        {
            console.log(user);
            console.log(userRoundScores[user])


            let scoreWrapper = document.createElement('div');
            let scoreNameBorder = document.createElement('div');
            let scoreNameText = document.createElement('div');
            let scoreValueBorder = document.createElement('div');
            let scoreValueText = document.createElement('div');
            
            scoreWrapper.className = 'individual-score-wrapper';
            scoreNameBorder.className = 'individual-score-name-border';
            scoreNameText.className = 'individual-score-name-text';
            scoreValueBorder.className = 'individual-score-value-border';
            scoreValueText.className = 'individual-score-value-text';

            scoreNameText.innerText = user[0].toUpperCase() + user.slice(1);
            scoreValueText.innerText = userRoundScores[user];

            scoreNameBorder.appendChild(scoreNameText);
            scoreValueBorder.appendChild(scoreValueText);
            
            scoreWrapper.appendChild(scoreNameBorder);
            scoreWrapper.appendChild(scoreValueBorder);
            
            $roundScoreContainer.appendChild(scoreWrapper);
        }
        
        $roundScoreContainer.classList.remove('hidden');
    }

    $contextTitle.textContent = "Scores for this round";

    $waitingForPlayersContainer.classList.add('hidden');

    $cardContainer.classList.add('hidden')
    $resetButton.classList.remove('hidden');
})

balderdashConnection.on("UpdateUserScore", function (score) {
    console.log(score);
    document.getElementById('score').innerText = score;
})

balderdashConnection.on("LoggedInUsers", function(users) {
    $loggedInUserItemsContainer.innerHTML = "";
    
    users.forEach(function(user) {
        let loggedInUserBorder  = document.createElement('div');
        loggedInUserBorder.className = "content-border info-border --centered";
        
        let loggedInUserItem  = document.createElement('div');
        loggedInUserItem.className = 'inner-content info-border --extra-width';
        loggedInUserItem.textContent = user;
        
        loggedInUserBorder.appendChild(loggedInUserItem)
        $loggedInUserItemsContainer.appendChild(loggedInUserBorder);
    });
    
    console.log(users);
    console.log('logged-in');
})
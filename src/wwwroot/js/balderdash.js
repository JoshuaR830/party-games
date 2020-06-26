var balderdashConnection = new signalR.HubConnectionBuilder().withUrl("/balderdashHub").build();
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

var connectionName = "GroupOfJoshua";

var $nextArrow = document.querySelector('.js-next-card');
var $previousArrow = document.querySelector('.js-previous-card');
var $carousel = document.querySelector('.js-carousel');
var $contextTitle = document.querySelector('.js-word-choice');

balderdashConnection.start().then(function () {

});

document.querySelector('.js-login-button').addEventListener('click', function() {
    let name = document.querySelector('#my-name').value;
    // Todo: change game type
    console.log(connectionName)
    balderdashConnection.invoke("StartUp", connectionName, name, 2);
    balderdashConnection.invoke("PassToDasher", connectionName, name);
    connection.invoke("JoinForScores", connectionName, name);
    // pixenaryConnection.invoke("JoinPixenaryGame", connectionName, name, 2);
    connection.invoke('DisplayScores', connectionName, name);
});


connection.start().then(function () {
});

balderdashConnection.on("ReceivedGuesses", function(guesses) {
    console.log(guesses);
    $carousel.innerHTML = "";
    guesses.forEach(function(guess, index) {
        let $card = document.createElement('div');
        $card.className = "balderdash-card";
        $card.setAttribute('data-card-number', index);
        $card.setAttribute('data-user', guess.name);
        $card.textContent = guess.guess;
        
        if(index === 0) {
            $card.classList.add("--active")
            $contextTitle.textContent = `${guess.name}'s guess`;
        } else {
            $card.classList.add('hidden');
        }

        $carousel.appendChild($card);
    })
})

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
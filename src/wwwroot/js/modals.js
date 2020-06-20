document.querySelector('.timer-fab').addEventListener('click', function(event) {
    document.querySelector('.js-timer-modal').classList.remove('popup-hidden');
})

document.querySelector('.js-score-fab').addEventListener('click', function(event) {
    console.log("Scores");
    document.querySelector('.js-scores-modal').classList.remove('popup-hidden');
    document.querySelector('.js-scores-breakdown-modal').classList.add('popup-hidden');
})

document.querySelector('#view-score-option').addEventListener('click', function(event) {
    document.querySelector('.js-scores-modal').classList.remove('popup-hidden');
    document.querySelector('.js-scores-breakdown-modal').classList.add('popup-hidden');
})

document.querySelector('.js-close-scores-modal').addEventListener('click', function(event) {
    document.querySelector('.js-scores-modal').classList.add('popup-hidden');
})

document.querySelector('.js-open-scores').addEventListener('click', function(event) {
    document.querySelector('.js-scores-modal').classList.remove('popup-hidden');
    document.querySelector('.js-scores-breakdown-modal').classList.add('popup-hidden');
})

document.querySelector('.js-open-scores-breakdown').addEventListener('click', function(event) {
    document.querySelector('.js-scores-breakdown-modal').classList.remove('popup-hidden');
    document.querySelector('.js-scores-modal').classList.add('popup-hidden');
})

document.querySelectorAll('.js-close-timer-modal').forEach(function(el){
    el.addEventListener('click', function(event) {
        document.querySelector('.js-timer-modal').classList.add('popup-hidden');
    })
})

document.querySelector('.js-close-scores-breakdown-modal').addEventListener('click', function() {
        document.querySelector('.js-scores-breakdown-modal').classList.add('popup-hidden');
})
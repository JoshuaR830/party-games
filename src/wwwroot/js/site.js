var gameRoundNumber = 0;
var scores = {};

var $scoreNamesContainer = document.querySelector('.js-score-user-selector');

document.querySelector('.login-button').addEventListener('click', userLogin);

function userLogin() {
    console.log("Logged in");
    let nameInput = document.getElementById('my-name');
    var name = nameInput.value;
    if (name.length > 0) {
        document.querySelector('.js-login-modal').classList.add('popup-hidden');
    } else {
        nameInput.style.borderColor = "#ff0000";
    }
    console.log();
}

document.getElementById("completeRoundButton").addEventListener('click', function(event) {
    connection.invoke("CompleteRound", connectionName);
})

connection.on("ReceiveMessage", function (user, friendlyName, message) {
    console.log(message);
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt").replace(/>/g, "&gt;");
    var li = document.createElement("div");
    li.className = "score-list-item";
    li.textContent = msg;

    var scoreData = message.split(": ");
    console.log(scoreData[0]);
    console.log(scoreData[1]);

    if (scores[scoreData[0]]) {
        scores[scoreData[0]] += parseInt(scoreData[1]);
    } else {
        scores[scoreData[0]] = parseInt(scoreData[1]);
    }

    console.log(scores);

    console.log(msg);

    document.getElementById("messageList").appendChild(li);

    document.getElementById("scoresList").innerHTML = "";

    for(var key in scores) {
        console.log(key);
        var scoreItem = document.createElement("div");
        scoreItem.className = "score-list-item";
        scoreItem.textContent = `${key}: ${scores[key]}`;
        console.log(scoreItem);
        document.getElementById("scoresList").appendChild(scoreItem);
    }
});

connection.on("DisplayScores", function(names, gameId)
{
    var usersToScore = document.querySelector('.js-score-user-selector');
    usersToScore.innerHTML = "";
    names.forEach(function(name) {
        var $el = document.createElement('div');
        $el.className = "button";
        $el.addEventListener('click', function() {
            let myName = document.getElementById('my-name').value;
            connection.invoke("UpdateManualScore", connectionName, myName, name, gameId) ;
            $scoreNamesContainer.classList.add('hidden');
        });
        
        $el.textContent = name;
        
        usersToScore.appendChild($el);
    })
})
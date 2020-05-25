"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var connectionName = "GroupOfJoshua";
var list = Array.from(Array(3), () => new Array(3));

document.getElementById("sendButton").disabled = true;

window.addEventListener('load', function() {
    document.getElementById("my-name").focus();
});

connection.on("CompletedScores", function() {
    submitScores();
    clearScore();
    document.getElementById('playAgain').classList.add('hidden');
    document.getElementById('playAgainFab').classList.remove('hidden');
    document.getElementById("table").classList.add('hidden');
    document.getElementById("options").classList.remove('hidden');
})

connection.on('StartNewRound', function() {
    gameRoundNumber ++;
    document.getElementById('startGame').classList.add('hidden');
    document.getElementById('playAgainFab').classList.add('hidden');
    document.getElementById("table").classList.remove('hidden');
    document.getElementById("options").classList.add('hidden');
})

connection.on("ReceiveLetter", function(letter) {
    document.getElementById('letter').textContent = letter;
});

document.querySelector('.js-login-button').addEventListener('click', function() {
    connection.invoke("Startup", connectionName, document.querySelector('#my-name').value, 0);
    connection.invoke("SetupNewUser", connectionName, document.querySelector('#my-name').value);
});

var row;

var counter = 0;
console.log(list);

document.getElementById("container").classList.remove("hidden");
connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    connection.invoke("AddToGroup", connectionName);
}).catch(function (err) {
    return console.error((err.toString()));
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    calculateScore();
    connection.invoke("AddToGroup", connectionName);
    connection.invoke("AddUserToDashboard", connectionName);
});

document.getElementById("submitScoresButton").addEventListener("click", submitScores());

function submitScores() {
    let name = document.getElementById('my-name').value; 

    connection.invoke("UpdateScoreBoard", connectionName, name).catch(function (err) {
        return console.error(err.toString());
    });
}

function clearScore(event) {
    document.querySelectorAll('.cell').forEach(function(cell) {
        cell.classList.remove('selected');
        for (var x = 0; x < 3; x++) {
            for (var y = 0; y < 3; y++) {
                list[x][y] = 0;
            }
        }
        calculateScore();
    })
}

function calculateScore() {
    connection.invoke("CalculateScore", connectionName, document.querySelector('#my-name').value);
}

connection.on("ScoreCalculated", function(score){
    document.getElementById('score').textContent  = score;
})

document.getElementById('startGame').addEventListener('click', launchThoughtsAndCrosses);
document.getElementById('playAgainFab').addEventListener('click', resetThoughtsAndCrosses);

function resetThoughtsAndCrosses() {
    connection.invoke("ResetGame", connectionName, document.querySelector('#my-name').value, 0);
    launchThoughtsAndCrosses();
    clearScore();
}

document.querySelector('#play-again-option').addEventListener('click', resetThoughtsAndCrosses);

document.getElementById('playAgain').addEventListener('click', function() {
    connection.invoke("CollectScores", connectionName);
});

function launchThoughtsAndCrosses() {
    connection.invoke("JoinRoom", connectionName, document.querySelector('#my-name').value, 0);

    let timerMins = parseInt(document.getElementById('set-minutes').value);
    let timerSecs = parseInt(document.getElementById('set-seconds').value);

    let time = [timerMins, timerSecs];

    connection.invoke("StartServerGame", connectionName, time);
}

connection.on("ReceiveCompleteRound", function() {
    console.log("ReceiveCompletedRound");
    var round = document.createElement('div');
    round.textContent = `Round ${gameRoundNumber}`;
    round.className = "round-title";
    document.getElementById("messageList").appendChild(round);
    document.getElementById('playAgain').classList.remove('hidden');
    stopTimer();
})

connection.on("ReceiveWordGrid", function(wordGrid) {
    document.getElementById('table').innerHTML = "";
    counter = 0;
    for (let x = 0; x < 9; x += 3) {
        row = document.createElement("div");
        row.className = "row";
        row.id = "row-" + x;
        document.getElementById('table').appendChild(row);
        for (let y = 0; y < 3; y++) {
            var cell = document.createElement('div');
            cell.className = 'cell cell-' + counter;
            let category = wordGrid[x + y].item1;
            cell.setAttribute("data-category", category);
            cell.addEventListener('click', function (event) {

                if (event.target.classList.contains('answer')) {
                    return;
                }

                if (event.currentTarget.querySelector('.answer') == null) {
                } else if (event.currentTarget.querySelector('.answer').value.length <= 0) {
                    event.currentTarget.querySelector('.answer').focus();
                    return;
                }
                
                if (wordGrid[x + y].item3) {
                    event.currentTarget.classList.remove('selected');
                    wordGrid[x + y].item3 = false;
                    connection.invoke("SetIsValidForCategory", connectionName, document.querySelector('#my-name').value, wordGrid[x + y].item1, wordGrid[x + y].item3)

                } else {
                    event.currentTarget.classList.add('selected');
                    wordGrid[x + y].item3 = true;
                    connection.invoke("SetIsValidForCategory", connectionName, document.querySelector('#my-name').value, wordGrid[x + y].item1, wordGrid[x + y].item3)
                }

                calculateScore();
            });

            cell.innerHTML = `<div class="category-title">${wordGrid[x + y].item1}</div><input type="text" class="answer" value="${wordGrid[x + y].item2}"/><div class="status"></div>`
            
            let myCell = cell;
            cell.addEventListener('keyup', function(event){
                connection.invoke("SetGuessForCategory", connectionName, document.querySelector('#my-name').value, wordGrid[x + y].item1, myCell.querySelector('.answer').value)
            })
            
            if (wordGrid[x + y].item3) {
                myCell.classList.add('selected');
            } else {
                cell.classList.remove('selected');
            }

            row.appendChild(cell);
            counter ++;
        }
    }
});

for (var x = 0; x < 3; x++) {
    row = document.createElement("div");
    row.className = "row";
    row.id = "row-" + x;
    console.log("New row");
    document.getElementById('table').appendChild(row);
    for (var y = 0; y < 3; y++) {
        list[x][y] = 0;
        console.log(counter);
        console.log(list);

        var cell = document.createElement('div');
        cell.className = 'cell cell-' + counter;
        let a = x;
        let b = y;
        cell.addEventListener('click', function(event) {
            if(!event.currentTarget.classList.contains('selected')) {
                event.currentTarget.classList.add('selected');
                list[a][b] = 1;
            } else {
                event.currentTarget.classList.remove('selected');
                list[a][b] = 0;
            }
            calculateScore();
        });

        row.appendChild(cell);

        console.log("Cell")
        counter ++;
    }
}
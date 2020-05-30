"use strict";
var list = Array.from(Array(3), () => new Array(3));
var randomNumbers = [];
var lettersNumbers = [];
// window.addEventListener("beforeunload", function(event) {
//     event.returnValue = "Hello"
// })

window.addEventListener('load', function() {
    document.getElementById("my-name").focus();
});

var connectionName = "GroupOfJoshua";

var categories = ["Boys name", "Girls name", "Hobby", "Fictional character", "Something outside", "Book", "Electrical item", "Kitchen item", "Body part", "Song", "Something savoury", "Something sweet", "Colour", "Toy", "Movie", "Job / Occupation", "Sport / Game", "Place", "Food", "TV programme", "Transport", "Pet", "Actor / Actress", "Family member", "Holiday destination", "Weather", "Animal / Bird", "Something you make", "Drink", "Ice cream", "Artist", "Company / Brand", "Musical instrument", "Fundraising Activity"]
var letters = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "XYZ"];

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

document.getElementById("sendButton").disabled = true;

connection.on("ReceiveTopics", function (user, friendlyName, topics) {

    topics = topics.split(',');

    topicSetter(topics);

    console.log(topics);
});

connection.on("CompletedScores", function() {
    if(calculateScore() > 0) {
        submitScores();
    }
    
    clearScore();
    document.getElementById('playAgain').classList.add('hidden');
    document.getElementById('startGame').classList.remove('hidden');
    document.getElementById("table").classList.add('hidden');
    document.getElementById("options").classList.remove('hidden');
})

connection.on('StartNewRound', function() {
    gameRoundNumber ++;
    document.getElementById('startGame').classList.add('hidden');
    document.getElementById("table").classList.remove('hidden');
    document.getElementById("options").classList.add('hidden');
    clearScore();
})

connection.on("ReceiveDirectMessage", function (recipient, myName, message) {
    console.log("Received direct message");
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt").replace(/>/g, "&gt;");
    var li = document.createElement("div");
    li.className = "message --left";
    li.textContent = msg;
    document.getElementById("messageList").appendChild(li);
});

connection.on("ReceiveLetter", function(letter) {
    console.log("Received letter");
    document.getElementById('letter').textContent = letter;
});

// document.getElementById('startButton').addEventListener('click', function () {


console.log(list[0][0]);
var row;

var counter = 0;
console.log(list);
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
            console.log(event.target.classList);
            if (event.target.classList.contains('answer')) {
                return;
            }

            if (event.currentTarget.querySelector('.answer') == null) {
                console.log("Not found");
            } else if (event.currentTarget.querySelector('.answer').value.length <= 0) {
                event.currentTarget.querySelector('.answer').focus();
                return;
            }

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

document.getElementById('categoryButton').addEventListener('click', function(event) {
    var topic = topicPicker();
    document.getElementById('category').textContent = topic;
})

document.getElementById('topicsButton').addEventListener('click', function(event) {
    let topics = selectNineTopics();
    var message = topics.toString();
    
    connection.invoke("SendTopics", connectionName, message).catch(function (err) {
        return console.error(err.toString());
    });
})

document.getElementById('letterButton').addEventListener('click', function(event) {
    var letter = letterPicker();
    connection.invoke("SendLetter", letter, connectionName);
});

document.getElementById("container").classList.remove("hidden");
connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    // connection.invoke("AddToGroup", "my group");
    // var friendlyName = document.getElementById("userInput").value;
    var friendlyName = connectionName;
    // connection.invoke("AddUserToDashboard", friendlyName);
    connection.invoke("AddToGroup", friendlyName);
}).catch(function (err) {
    return console.error((err.toString()));
});
// })

document.getElementById("sendButton").addEventListener("click", function (event) {

    console.log(list);

    calculateScore();

    var user = connectionName;
    connection.invoke("AddToGroup", user);
    connection.invoke("AddUserToDashboard", user);

    var message = document.getElementById("messageInput").value;
    // connection.invoke("SendMessage", user, message).catch(function (err) {
    //     return console.error(err.toString());
    // });
    event.preventDefault();
});

document.getElementById("submitScoresButton").addEventListener("click", submitScores());

function submitScores() {
    let name = document.getElementById('my-name').value; 

    console.log("Score sent");

    let score = calculateScore();

    var user = connectionName;

    connection.invoke("SendScore", user, name, score).catch(function (err) {
        return console.error(err.toString());
    });
}

// document.getElementById('score').addEventListener('click', clearScore);


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
    let score = 0;
    if (list[0][0] == 1 && list[1][0] == 1 && list[2][0] == 1) {
        score += 3;
    }

    if (list[0][1] == 1 && list[1][1] == 1 && list[2][1] == 1) {
        score += 3;
    }

    if (list[0][2] == 1 && list[1][2] == 1 && list[2][2] == 1) {
        score += 3;
    }

    if (list[0][0] == 1 && list[0][1] == 1 && list[0][2] == 1) {
        score += 3;
    }

    if (list[1][0] == 1 && list[1][1] == 1 && list[1][2] == 1) {
        score += 3;
    }

    if (list[2][0] == 1 && list[2][1] == 1 && list[2][2] == 1) {
        score += 3;
    }

    if (list[0][0] == 1 && list[1][1] == 1 && list[2][2] == 1) {
        score += 3;
    }

    if (list[0][2] == 1 && list[1][1] == 1 && list[2][0] == 1) {
        score += 3;
    }

    list.forEach(row => {
        row.forEach(cell => {
            if (cell === 1) {
                score++;
                console.log("Yay");
            }
        })
    });

    document.getElementById('score').textContent  = score;

    return score;
}

function topicPicker() {

    if(categories.length - randomNumbers.length < 9) {
        randomNumbers = [];
    }

    do {
        var number = Math.floor(Math.random() * categories.length);
    } while(randomNumbers.indexOf(number) >= 0);

    randomNumbers.push(number);

    console.log(categories[number]);
    return categories[number];
}

function selectNineTopics()
{
    let topics = []
    for (let i = 0; i < 9; i++) {
        topics.push(topicPicker());
    }

    return topics;
}

function topicSetter(topics) {

    let chosenCells = [];  

    for (var cellNo = 0; cellNo < 9; cellNo++) {
        do {
            var cellNumber = Math.floor(Math.random() * 9);
        } while(chosenCells.indexOf(cellNumber) >= 0);

        chosenCells.push(cellNumber);
        console.log(cellNumber);
        console.log(chosenCells);

        cell = document.querySelector(`.cell-${cellNumber}`);
        cell.innerHTML = `<div class="category-title">${topics[cellNo]}</div><input type="text" class="answer"/><div class="status"></div>`
    }
}

function letterPicker() {

    do {
        var number = Math.floor(Math.random() * letters.length);
    } while(lettersNumbers.indexOf(number) >= 0);

    lettersNumbers.push(number);

    console.log(letters[number]);
    return letters[number];
}

document.getElementById('startGame').addEventListener('click', launchThoughtsAndCrosses);

document.querySelector('#play-again-option').addEventListener('click', launchThoughtsAndCrosses);

document.getElementById('playAgain').addEventListener('click', function() {
    connection.invoke("CollectScores", connectionName);
});

function launchThoughtsAndCrosses() {
    connection.invoke("JoinRoom", connectionName, document.querySelector('#my-name').value, 0);

    let timerMins = parseInt(document.getElementById('set-minutes').value);
    let timerSecs = parseInt(document.getElementById('set-seconds').value);

    let user = connectionName;
    let letter = letterPicker();
    let time = [timerMins, timerSecs];
    let topics = selectNineTopics().toString();

    console.log(">>>" + letter)

    connection.invoke("StartGame", user, letter, time, topics);
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
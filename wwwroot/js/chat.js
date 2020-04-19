"use strict";
var list = Array.from(Array(3), () => new Array(3));
var randomNumbers = [];
var lettersNumbers = [];
var timer;

var categories = ["Boys name", "Girls name", "Hobby", "Company", "Fictional character", "Something outside", "Book", "Bird", "Electrical item", "Kitchen item", "Body part", "Song", "Something savoury", "Something sweet", "Colour", "Toy", "Movie", "Job/Occupation", "Sport/Game", "Place", "Food", "TV programme", "Transport", "Type of Pet", "Actor/ Actress", "Family member", "Holiday destination", "Cooking implement", "Super hero/villain", "Weather", "Animal", "Something you make", "Drink", "Ice cream flavour", "Artist", "Brand", "Musical instrument"]
var letters = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "XYZ"];

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, friendlyName, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt").replace(/>/g, "&gt;");
    var li = document.createElement("div");
    li.className = "message --right";
    li.textContent = msg;

    console.log(msg);

    document.getElementById("messageList").appendChild(li);
});

connection.on("ReceiveTopics", function (user, friendlyName, topics) {

    topics = topics.split(',');

    if(calculateScore() > 0) {
        submitScores();
    }

    clearScore();
    topicSetter(topics);

    console.log(topics);
});

connection.on("ReceiveDirectMessage", function (recipient, myName, message) {
    console.log("Received direct message");
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt").replace(/>/g, "&gt;");
    var li = document.createElement("div");
    li.className = "message --left";
    li.textContent = msg;
    document.getElementById("messageList").appendChild(li);
});

connection.on("ReceiveTimeStart", function (time) {
    console.log("Received time message");
    document.getElementById('set-minutes').value = time[0];
    document.getElementById('set-seconds').value = time[1];

    startTimer(time[0], time[1]);
});

connection.on("ReceiveStopTimer", function () {
    stopTimer();
});

// document.getElementById('startButton').addEventListener('click', function () {
document.getElementById('clock-minutes').textContent = document.getElementById('set-minutes').value;
document.getElementById('clock-seconds').textContent = document.getElementById('set-seconds').value;

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

            if (event.currentTarget.querySelector('.answer').value.length <= 0) {
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
    selectNineTopics();
})

document.getElementById('letterButton').addEventListener('click', function(event) {
    var topic = letterPicker();
    document.getElementById('letter').textContent = topic;
})

document.getElementById("container").classList.remove("hidden");
connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    // connection.invoke("AddToGroup", "my group");
    // var friendlyName = document.getElementById("userInput").value;
    var friendlyName = "GroupOfJoshua";
    // connection.invoke("AddUserToDashboard", friendlyName);
    connection.invoke("AddToGroup", friendlyName);
}).catch(function (err) {
    return console.error((err.toString()));
});
// })

document.getElementById("sendButton").addEventListener("click", function (event) {

    console.log(list);

    calculateScore();

    var user = "GroupOfJoshua";
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

    var user = "GroupOfJoshua";

    connection.invoke("SendScore", user, name, score).catch(function (err) {
        return console.error(err.toString());
    });
}

document.getElementById('score').addEventListener('click', clearScore);


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

    var message = topics.toString();
    
    connection.invoke("SendTopics", "GroupOfJoshua", message).catch(function (err) {
        return console.error(err.toString());
    });
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

document.getElementById('startTimerButton').addEventListener('click', function() {

    let timerMins = parseInt(document.getElementById('set-minutes').value);
    let timerSecs = parseInt(document.getElementById('set-seconds').value);
    
    connection.invoke("SendTime", "GroupOfJoshua", [timerMins, timerSecs]);
});

function startTimer(timerMins, timerSecs) {
    timerSecs = (timerMins * 60) + timerSecs;
    console.log(timerMins);
    console.log(timerSecs);

    if(timer != undefined) {
        clearInterval(timer);
    }

    timer = setInterval(function() {
        console.log(timerSecs)
        timerSecs --;
        updateClock(timerSecs);
        
        if(timerSecs === 0)
        {
            triggerAlarm();
            clearInterval(timer);
            return;
        }

    }, 1000);
}

function updateClock(seconds) {
    let minutes = Math.floor(seconds/60);
    seconds = seconds % 60;

    document.getElementById('clock-minutes').textContent = minutes;
    document.getElementById('clock-seconds').textContent = seconds;
}

function triggerAlarm()
{
    var sound = document.getElementById('alarm-sound');
    sound.play();
}

document.getElementById("stopTimerButton").addEventListener('click', function (event) {
    connection.invoke("StopTimer", "GroupOfJoshua");
});

function stopTimer() {
    var sound = document.getElementById('alarm-sound');
    if(timer != undefined) {
        clearInterval(timer);
        console.log(document.getElementById('set-minutes').value)
        document.getElementById('clock-minutes').textContent = document.getElementById('set-minutes').value;
        document.getElementById('clock-seconds').textContent = document.getElementById('set-seconds').value;
    }
    sound.pause();
}

document.getElementById("clock-minutes").addEventListener('click', incrementMinutes);
document.getElementById("clock-seconds").addEventListener('click', incrementSeconds);
document.getElementById("delete-button").addEventListener('click', function() {
    if(timer != undefined) {
        clearInterval(timer);
    }
    document.getElementById('set-minutes').value = 0;
    document.getElementById('set-seconds').value = 0;
    document.getElementById('clock-minutes').textContent = 0;
    document.getElementById('clock-seconds').textContent = 0;

});

function incrementMinutes() {
    let minutes = parseInt(document.getElementById('set-minutes').value);
    if (minutes < 59) {
        document.getElementById('set-minutes').value = minutes += 1;
    } else {
        document.getElementById('set-minutes').value = 0;
    }
    document.getElementById('clock-minutes').textContent = document.getElementById('set-minutes').value;
}

function incrementSeconds() {
    let seconds = parseInt(document.getElementById('set-seconds').value);

    if (seconds < 59) {
        document.getElementById('set-seconds').value = seconds += 1;
    } else {
        document.getElementById('set-seconds').value = 0;
        let minutes = parseInt(document.getElementById('set-minutes').value);
        document.getElementById('set-minutes').value = minutes += 1;
    }

    document.getElementById('clock-seconds').textContent = document.getElementById('set-seconds').value;
    document.getElementById('clock-minutes').textContent = document.getElementById('set-minutes').value;
}
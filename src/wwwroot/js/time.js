"use strict";

var timer;

connection.on("ReceiveTimeStart", function (time) {
    console.log("Received time message");
    document.getElementById('set-minutes').value = time[0];
    document.getElementById('set-seconds').value = time[1];

    startTimer(time[0], time[1]);
});

connection.on("ReceiveStopTimer", function () {
    
    if(document.getElementById('playAgainFab') !== null) {
        document.getElementById('playAgainFab').classList.remove('hidden');
    } else {
        document.getElementById('startGame').classList.remove('hidden');
    }
    
    stopTimer();
});

document.getElementById('startTimerButton').addEventListener('click', function() {

    let timerMins = parseInt(document.getElementById('set-minutes').value);
    let timerSecs = parseInt(document.getElementById('set-seconds').value);
    
    connection.invoke("SendTime", connectionName, [timerMins, timerSecs]);
});



document.getElementById('clock-minutes').textContent = pad(document.getElementById('set-minutes').value);
document.getElementById('clock-seconds').textContent = pad(document.getElementById('set-seconds').value);

document.getElementById("stopTimerButton").addEventListener('click', function (event) {
    connection.invoke("StopTimer", connectionName);
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

    document.getElementById('clock-minutes').textContent = pad(minutes);
    document.getElementById('clock-seconds').textContent = pad(seconds);
}

function triggerAlarm()
{
    document.querySelector('.js-times-up').classList.remove("popup-hidden");
    document.querySelector(".js-end-round-title").innerHTML = `Round ${gameRoundNumber} complete`;
    var sound = document.getElementById('alarm-sound');
    sound.play();
}

function stopTimer() {
    var sound = document.getElementById('alarm-sound');
    if(timer != undefined) {
        clearInterval(timer);
        console.log(document.getElementById('set-minutes').value)
        document.getElementById('clock-minutes').textContent = pad(document.getElementById('set-minutes').value);
        document.getElementById('clock-seconds').textContent = pad(document.getElementById('set-seconds').value);
    }
    sound.pause();
    document.querySelector('.js-times-up').classList.add("popup-hidden");
}

document.getElementById("delete-button").addEventListener('click', function() {
    if(timer != undefined) {
        clearInterval(timer);
    }
    document.getElementById('set-minutes').value = 0;
    document.getElementById('set-seconds').value = 0;
    document.getElementById('clock-minutes').textContent = pad(0);
    document.getElementById('clock-seconds').textContent = pad(0);

});

function incrementMinutes() {
    let minutes = parseInt(document.getElementById('set-minutes').value);
    if (minutes < 59) {
        document.getElementById('set-minutes').value = minutes += 1;
    } else {
        document.getElementById('set-minutes').value = 0;
    }
    document.getElementById('clock-minutes').textContent = pad(document.getElementById('set-minutes').value);
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

    document.getElementById('clock-seconds').textContent = pad(document.getElementById('set-seconds').value);
    document.getElementById('clock-minutes').textContent = pad(document.getElementById('set-minutes').value);
}

document.getElementById('my-name').addEventListener('keydown', function(event) {
    if(event.keyCode === 13)
    {
        console.log("Hi");
        event.currentTarget.style.outlineColor = "#ff0000";
        userLogin();
    }
})

document.getElementById('decrement-minutes').addEventListener('click', function() {
    var setMinutes = document.querySelector('#set-minutes');
    var num = parseInt(setMinutes.value);

    if (num > 0) {
        setMinutes.value = num - 1;
    }

    document.querySelector('#clock-minutes').textContent = pad(setMinutes.value);
})
document.getElementById('increment-minutes').addEventListener('click', function() {
    var setMinutes = document.querySelector('#set-minutes');
    var num = parseInt(setMinutes.value);
    if (num < 59) {
        setMinutes.value = num + 1;
    }
    document.querySelector('#clock-minutes').textContent = pad(setMinutes.value);
})
document.getElementById('decrement-seconds').addEventListener('click', function() {
    var setSeconds = document.querySelector('#set-seconds');
    var setMinutes = document.querySelector('#set-minutes');
    var num = parseInt(setSeconds.value);

    if (num > 0) {
        setSeconds.value = num - 1;
    } else {
        setSeconds.value = 59;
        let minutes = parseInt(setMinutes.value);
        if(minutes > 0){
            minutes -= 1;
        }
        document.getElementById('clock-minutes').value = minutes;
        setMinutes.value = minutes;
    }


    document.querySelector('#clock-seconds').textContent = pad(setSeconds.value);
})
document.getElementById('increment-seconds').addEventListener('click', function() {
    var setSeconds = document.querySelector('#set-seconds');
    var setMinutes = document.querySelector('#set-minutes');
    var num = parseInt(setSeconds.value);
    if (num < 59) {
        setSeconds.value = num + 1;
    } else {
        setSeconds.value = 0;
        let minutes = parseInt(setMinutes.value) + 1;
        document.getElementById('clock-minutes').value = minutes;
        setMinutes.value = minutes;
    }
    document.querySelector('#clock-seconds').textContent = pad(setSeconds.value);
})

function pad(time)
{
    let paddedTime = time;
    if(time < 10) {
        paddedTime = `0${time}`
    }
    return paddedTime;
}
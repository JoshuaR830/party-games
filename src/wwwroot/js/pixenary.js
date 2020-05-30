var pixenaryConnection = new signalR.HubConnectionBuilder().withUrl("/pixenaryHub").build();
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

var connectionName = "GroupOfJoshua"

var $table = document.querySelector('.js-pixenary-table');
var $red = document.querySelector('.js-red');
var $yellow = document.querySelector('.js-yellow');
var $green = document.querySelector('.js-green');
var $blue = document.querySelector('.js-blue');
var $brown = document.querySelector('.js-brown');
var $grey = document.querySelector('.js-grey');

var $wordToDraw = document.querySelector('.js-word-choice');

var $resetButton = document.querySelector('.js-reset-button');

var color = "dodgerblue";

pixenaryConnection.start().then(function () {
});

document.querySelector('.js-login-button').addEventListener('click', function() {
    let name = document.querySelector('#my-name').value;
    pixenaryConnection.invoke("JoinPixenaryGame", connectionName, name, 2);

});

pixenaryConnection.on("PixelGridResponse", function(grid, isUsersTurn) {
    let data = JSON.parse(grid);
    
    let size = Math.sqrt(data.length)
    
    $table.innerHTML = "";
    
    for(let y = 0; y < data.length; y += size)
    {
        let row = document.createElement('div');
        row.className = "pixenary-row";
        
        for(let x = 0; x < size; x ++)
        {
            let cell = document.createElement('div');
            cell.className = "pixenary-cell --not-selected";
            cell.id = `cell-${x+y}`;
            if(data[x + y]) {
                cell.style.backgroundColor = data[x + y];
                cell.classList.remove('--not-selected');
            }
            if (isUsersTurn === true) {
                cell.addEventListener('click', function() {
                    cell.style.backgroundColor = color;
                    cell.classList.remove('--not-selected');
                    pixenaryConnection.invoke("UpdatePixelGrid", connectionName, x + y, color);
                });
            }
            row.appendChild(cell);
        }
        $table.appendChild(row);
    }
});

pixenaryConnection.on("PixelGridUpdate", function(pixelPosition, pixelColor) {
    let $selectedCell = document.querySelector(`#cell-${pixelPosition}`);
    $selectedCell.style.backgroundColor = pixelColor;
    $selectedCell.classList.remove('--not-selected');
});

pixenaryConnection.on("PixelWord", function(word) {
    console.log(word);
    
    $wordToDraw.textContent = word.word;
});

pixenaryConnection.on("ResetGame", function() {
    $wordToDraw.textContent = "";
    $resetButton.textContent = "Next round";
});

$resetButton.addEventListener('click', function() {
    // if(confirm("Do you want to move to the next round? Your current drawing will be deleted.")){
    pixenaryConnection.invoke("ResetPixenary", connectionName);
    // }
})

$red.addEventListener('click', function() {
    color = "red";
    selectColor($red);
});

$green.addEventListener('click', function() {
    color = "green";
    selectColor($green);
});

$blue.addEventListener('click', function() {
    color = "dodgerblue";
    selectColor($blue);
});

$yellow.addEventListener('click', function() {
    color = "yellow";
    selectColor($yellow);
});

$brown.addEventListener('click', function() {
    color = "saddlebrown";
    selectColor($brown);
});

$grey.addEventListener('click', function() {
    color = "lightslategrey";
    selectColor($grey);
});

function selectColor($color) {
    $red.classList.remove('--selected-color');
    $green.classList.remove('--selected-color');
    $blue.classList.remove('--selected-color');
    $yellow.classList.remove('--selected-color');
    $brown.classList.remove('--selected-color');
    $grey.classList.remove('--selected-color');
    
    $color.classList.add('--selected-color')
}
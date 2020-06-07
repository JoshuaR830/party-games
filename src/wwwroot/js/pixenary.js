var pixenaryConnection = new signalR.HubConnectionBuilder().withUrl("/pixenaryHub").build();
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

var connectionName = "GroupOfJoshua"

var $table = document.querySelector('.js-pixenary-table');

var $colorTable = document.querySelector('.js-color-container');
var $setColoursButton = document.querySelector('.js-set-colors-button');

var $colorModal = document.querySelector('.js-color-modal');
var $colourCanvas = document.getElementById('colourWheel');

var $colorSelectorContainer = document.querySelector('.js-color-selectors');

var colorList = ["#1E90FF", "#ff0000", "#ffff00", "#228B22", "#8B4513", "white"];

var $wordToDraw = document.querySelector('.js-word-choice');

var $resetButton = document.querySelector('.js-reset-button');

var color = "dodgerblue";

var coordinates = [[223, 134], [9, 153], [112, 121], [178, 218], [60, 243]];

var ctx = $colourCanvas.getContext("2d");
var img = new Image();
img.src = '/images/colourWheel.png';
img.height = 300;
img.width = 300;
img.onload = function() {
    ctx.drawImage(img, 0, 0, 300, 300);
    img.style.display = 'none';
}

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
            
            console.log(isUsersTurn);
            // if (isUsersTurn === true) {
                cell.addEventListener('click', function() {
                    cell.style.backgroundColor = color;
                    cell.classList.remove('--not-selected');
                    console.log(color);
                    pixenaryConnection.invoke("UpdatePixelGrid", connectionName, x + y, color);
                });
            // }
            row.appendChild(cell);
        }
        $table.appendChild(row);
    }
});

pixenaryConnection.on("PixelGridUpdate", function(pixelPosition, pixelColor) {
    let $selectedCell = document.querySelector(`#cell-${pixelPosition}`);
    $selectedCell.style.backgroundColor = pixelColor;

    $selectedCell.classList.remove("--eraser");

    if(pixelColor === "white") {
        $selectedCell.classList.add("--eraser");
    }
    
    $selectedCell.classList.remove('--not-selected');
});

pixenaryConnection.on("PixelWord", function(word) {
    console.log(word);
    $colorModal.querySelector('.modal-title').textContent = `Word: ${word.word}`;
    $colorModal.classList.remove('popup-hidden');
    $wordToDraw.textContent = word.word;

    ctx.clearRect(0, 0, $colourCanvas.width, $colourCanvas.height);
    ctx.drawImage(img, 0, 0, 300, 300);
   
    let $colorSelectorButtons = document.querySelector('.js-color-selectors').querySelectorAll('.color');
    $colorSelectorButtons.forEach(function ($el, index) {
        let imageData = ctx.getImageData(coordinates[index][0], coordinates[index][1], 1, 1);
        let color = `rgb(${imageData.data[0]}, ${imageData.data[1]}, ${imageData.data[2]})`;
        colorList[index] = color;
        $el.style.backgroundColor = color;
    });
    
    $colorSelectorButtons[0].click();
});

pixenaryConnection.on("ResetGame", function() {
    $wordToDraw.textContent = "";
    $resetButton.textContent = "Next round";
});

$resetButton.addEventListener('click', function() {
    pixenaryConnection.invoke("ResetPixenary", connectionName);
});

pixenaryConnection.on("ReceiveColors", function (colorsChosen) {
    colorList = colorsChosen;
    drawColours();
})

function drawColours() {
    $colorTable.innerHTML = "";
    let colorSelection = $colorSelectorContainer.querySelectorAll('.color');
    colorList.forEach(function (myColor, index) {
        
        if(index < colorSelection.length) {
            colorSelection[index].style.backgroundColor = myColor;
        }      
        console.log(myColor);

        let $el = document.createElement('div');
        $el.className = "color"
        $el.style.backgroundColor = myColor;

        $el.classList.remove("eraser");
        if (myColor === "white") {
            $el.classList.add("eraser");
        }

        $el.addEventListener('click', function () {
            color = myColor;
            selectColor($el);
        })

        $colorTable.appendChild($el);
    })
}

$colorSelectorContainer.innerHTML = "";
colorList.forEach(function (itemColor, index) {
    if (itemColor === "white") {
        return;
    }
    let $colorSelectorButton = document.createElement('div');
    $colorSelectorButton.className = "color";
    $colorSelectorButton.style.backgroundColor = itemColor;
    $colorSelectorButton.setAttribute('data-number', `${index}`);

    $colorSelectorButton.addEventListener('click', function(event) {
        $colorModal.querySelectorAll('.color').forEach(function($el) {
            $el.classList.remove('--selected-color');
        });
        
        event.currentTarget.classList.add('--selected-color')
        placeColorCrosshair(index);
    })

    $colorSelectorContainer.appendChild($colorSelectorButton);
})

$setColoursButton.addEventListener('click', function() {
    $colorModal.classList.add('popup-hidden');
    pixenaryConnection.invoke("SendColors", connectionName, colorList)
});

function selectColor($color) {
    let colorEls = $colorTable.querySelectorAll('.color');
    colorEls.forEach(function($el) {
        $el.classList.remove('--selected-color');
    });
    
    $color.classList.add('--selected-color')
}



$colourCanvas.addEventListener('mousedown', function(event){
    let selectedColor = document.querySelector('.js-color-selectors').querySelector('.--selected-color');
    let selectionIndex = selectedColor.getAttribute('data-number');
    
    coordinates[selectionIndex] = [event.layerX, event.layerY];
    
    colorList.forEach(function(currentColor, index) {
        if(currentColor === 'white') {
            return;
        }
        
        let colorData = ctx.getImageData(coordinates[index][0], coordinates[index][1], 1, 1);
        colorList[index] = `rgb(${colorData.data[0]}, ${colorData.data[1]}, ${colorData.data[2]})`;
    });
    
    selectedColor.style.backgroundColor = colorList[selectionIndex];
    placeColorCrosshair(selectionIndex);
})

function placeColorCrosshair(index)
{
    ctx.clearRect(0, 0, $colourCanvas.width, $colourCanvas.height);
    ctx.drawImage(img, 0, 0, 300, 300);
    ctx.beginPath();
    ctx.moveTo((coordinates[index][0] - 5), (coordinates[index][1] - 5));
    ctx.lineTo((coordinates[index][0] + 5), (coordinates[index][1] + 5));
    ctx.moveTo((coordinates[index][0] + 5), (coordinates[index][1] - 5));
    ctx.lineTo((coordinates[index][0] - 5), (coordinates[index][1] + 5));
    ctx.stroke();
}
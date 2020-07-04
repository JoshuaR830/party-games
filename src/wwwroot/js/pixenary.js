var pixenaryConnection = new signalR.HubConnectionBuilder().withUrl("/pixenaryHub").build();
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

var connectionName = "GroupOfJoshua"

// var $table = document.querySelector('.js-pixenary-table');

var $colorTable = document.querySelector('.js-color-container');
var $setColoursButton = document.querySelector('.js-set-colors-button');

var $colorModal = document.querySelector('.js-color-modal');
var $colourCanvas = document.getElementById('colourWheel');

var $pixelCanvas = document.getElementById('pixel-canvas');
var previousTouchX = -20;
var previousTouchY = -20;

var $colorSelectorContainer = document.querySelector('.js-color-selectors');

var colorList = ["#1E90FF", "#ff0000", "#ffff00", "#228B22", "#8B4513", "white"];

var $wordToDraw = document.querySelector('.js-word-choice');

var $resetButton = document.querySelector('.js-reset-button');
var $scoreNamesContainer = document.querySelector('.js-score-user-selector');
var $scoreButton = document.querySelector('.js-score-test');

var color = "dodgerblue";
var size = 0;

var coordinates = [[223, 134], [9, 153], [112, 121], [178, 218], [60, 243]];

var colorContext = $colourCanvas.getContext("2d");
var img = new Image();
img.src = '/images/colourWheel.png';
img.height = 300;
img.width = 300;
img.onload = function() {
    colorContext.drawImage(img, 0, 0, 300, 300);
    img.style.display = 'none';
}

pixenaryConnection.start().then(function () {
});

connection.start().then(function () {
});

document.querySelector('.js-login-button').addEventListener('click', function() {
    let name = document.querySelector('#my-name').value;
    connection.invoke("JoinForScores", connectionName, name);
    pixenaryConnection.invoke("JoinPixenaryGame", connectionName, name, 2);

});

pixenaryConnection.on("PixelGridResponse", function(grid, isUsersTurn, score, nameOfCurrentPlayer) {
    document.getElementById('score').textContent = score;
    let data = JSON.parse(grid);
    size = Math.sqrt(data.length)
    
    if (!isUsersTurn) {
        document.querySelector('.js-word-choice').innerText = `It's ${nameOfCurrentPlayer}'s turn`;
    }
    // $table.innerHTML = "";

    let pixelsPerSide = $pixelCanvas.width/size;
    let pixelCount = Math.pow(pixelsPerSide, 2);
    let pixelSize = Math.floor($pixelCanvas.width/size);
    let myPixelContext = $pixelCanvas.getContext("2d");

    // let myPixelContext = $pixelCanvas.getContext("2d");
    myPixelContext.clearRect(0, 0, $colourCanvas.width, $colourCanvas.height);
    
    $pixelCanvas.style.backgroundColor = "white";
    
    for(let j = 0; j < pixelsPerSide; j++ )
    {
        console.log(j);
        myPixelContext.beginPath();
        myPixelContext.moveTo(j*pixelSize, 0);
        myPixelContext.lineTo(j*pixelSize, pixelsPerSide*pixelSize);
        myPixelContext.strokeStyle = 'rgba(219, 201, 226, 0.36)';
        myPixelContext.stroke();
        myPixelContext.beginPath();
        myPixelContext.moveTo(0, j*pixelSize);
        myPixelContext.lineTo(pixelsPerSide*pixelSize, j*pixelSize);
        myPixelContext.strokeStyle = 'rgba(219, 201, 226, 0.36)';
        myPixelContext.stroke();
    }
});

$pixelCanvas.addEventListener('touchmove', function(event) {
    event.preventDefault();
    let touch = event.touches[0];
    let canvasPosition = $pixelCanvas.getBoundingClientRect();
    
    let touchX = touch.clientX - canvasPosition.left;
    let touchY = touch.clientY - canvasPosition.top;
    
    if (touchX > $pixelCanvas.width || touchX < 0) {
        return;
    }
    
    if (touchY > $pixelCanvas.height || touchY < 0) {
        return;
    }
    
    if(Math.abs(touchX - previousTouchX) > 10 ||  Math.abs(touchY - previousTouchY) > 10)
    {
        let pixelPosition = Math.floor((touchX)/($pixelCanvas.width/size)) + Math.floor((touchY/($pixelCanvas.width/size)))*size;
        console.log(pixelPosition);
        setCanvasColor(pixelPosition, color)
        pixenaryConnection.invoke("UpdatePixelGrid", connectionName, pixelPosition, color);
        previousTouchX = touch.clientX - canvasPosition.left;
        previousTouchY = touch.clientY - canvasPosition.top;
    }

})

$pixelCanvas.addEventListener('click', function(event) {
    let pixelPosition = Math.floor(event.layerX/($pixelCanvas.width/size)) + Math.floor((event.layerY/($pixelCanvas.width/size)))*size;
    pixenaryConnection.invoke("UpdatePixelGrid", connectionName, pixelPosition, color);

    previousTouchX = event.layerX;
    previousTouchY = event.layerY;
})

function setCanvasColor(pixelPosition, pixelColor)
{
    let gridY = Math.floor(pixelPosition/size);
    let gridX = pixelPosition%size;

    let pixelSize = Math.floor($pixelCanvas.width/size);

    console.log($pixelCanvas.width/size);

    var pixelContext = $pixelCanvas.getContext("2d");

    pixelContext.beginPath();
    colorContext.moveTo(gridX*pixelSize, gridY*pixelSize);
    pixelContext.rect(gridX*pixelSize, gridY*pixelSize, pixelSize, pixelSize);
    pixelContext.fillStyle = pixelColor;
    pixelContext.fill();

    if(pixelColor === 'white') {
        pixelContext.rect(gridX*pixelSize, gridY*pixelSize, pixelSize, pixelSize);
        pixelContext.fillStyle = '#ffffff';
        pixelContext.fill();
        pixelContext.rect(gridX*pixelSize, gridY*pixelSize, pixelSize, pixelSize);
        pixelContext.fillStyle = 'rgba(219, 201, 226, 0.36)';
        pixelContext.fill();

        pixelContext.beginPath();
        pixelContext.rect(gridX*pixelSize + 1, gridY*pixelSize + 1, pixelSize - 2, pixelSize - 2);
        pixelContext.fillStyle = '#ffffff';
        pixelContext.fill();

    }
}

pixenaryConnection.on("PixelGridUpdate", function(pixelPosition, pixelColor) {

    setCanvasColor(pixelPosition, pixelColor)
});

pixenaryConnection.on("PixelWord", function(word) {
    console.log(word);
    $scoreButton.classList.remove("hidden");
    document.querySelector('.js-tool-bar').classList.remove("hidden");
    document.querySelector('.js-pixel-canvas-container').classList.remove('hidden');
    document.querySelector('.js-color-container').classList.remove('hidden');
    $colorModal.querySelector('.modal-title').textContent = `Word: ${word.word}`;
    $colorModal.classList.remove('popup-hidden');
    $wordToDraw.textContent = `Draw this: ${word.word}`;

    colorContext.clearRect(0, 0, $colourCanvas.width, $colourCanvas.height);
    colorContext.drawImage(img, 0, 0, 300, 300);
   
    let $colorSelectorButtons = document.querySelector('.js-color-selectors').querySelectorAll('.color');
    $colorSelectorButtons.forEach(function ($el, index) {
        let imageData = colorContext.getImageData(coordinates[index][0], coordinates[index][1], 1, 1);
        let color = `rgb(${imageData.data[0]}, ${imageData.data[1]}, ${imageData.data[2]})`;
        colorList[index] = color;
        $el.style.backgroundColor = color;
    });
    
    $colorSelectorButtons[0].click();
});

pixenaryConnection.on("ResetGame", function() {
    let $eraser = document.querySelector('.js-tool-bar').querySelector('.eraser');
    if($eraser !== null) {
        $eraser.remove();
    }
    $wordToDraw.textContent = "";
    $resetButton.textContent = "Next round";
    $resetButton.classList.add('hidden');
    $scoreNamesContainer.classList.add('hidden');
    document.querySelector('.js-color-container').classList.remove('hidden');
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

        $el.addEventListener('click', function () {
            color = myColor;
            selectColor($el);
        })

        if (myColor === "white") {
            $el.classList.add("eraser");
            document.querySelector('.js-tool-bar').appendChild($el);
            return;
        }
        
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

    document.querySelector('.js-fill-button').style.background = `repeating-linear-gradient(45deg, black 0, black 2px, ${color} 2px, ${color} 4px)`;
    document.querySelector('.js-fill-button').style.color = `${color}`;
    
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
        
        let colorData = colorContext.getImageData(coordinates[index][0], coordinates[index][1], 1, 1);
        colorList[index] = `rgb(${colorData.data[0]}, ${colorData.data[1]}, ${colorData.data[2]})`;
    });
    
    selectedColor.style.backgroundColor = colorList[selectionIndex];
    placeColorCrosshair(selectionIndex);
})

function placeColorCrosshair(index)
{
    colorContext.clearRect(0, 0, $colourCanvas.width, $colourCanvas.height);
    colorContext.drawImage(img, 0, 0, 300, 300);
    colorContext.beginPath();
    colorContext.moveTo((coordinates[index][0] - 5), (coordinates[index][1] - 5));
    colorContext.lineTo((coordinates[index][0] + 5), (coordinates[index][1] + 5));
    colorContext.moveTo((coordinates[index][0] + 5), (coordinates[index][1] - 5));
    colorContext.lineTo((coordinates[index][0] - 5), (coordinates[index][1] + 5));
    colorContext.stroke();
}

$scoreButton.addEventListener('click', function() {
    console.log("Clicked");
    document.querySelector('.js-word-choice').innerText = "Select the player who guessed correctly"
    document.querySelector('.js-pixel-canvas-container').classList.add('hidden');
    document.querySelector('.js-color-container').classList.add('hidden');
    let name = document.querySelector('#my-name').value;
    $scoreNamesContainer.classList.remove('hidden');
    $scoreButton.classList.add('hidden');
    document.querySelector('.js-tool-bar').classList.add("hidden");
    connection.invoke('DisplayScores', connectionName, name, 2);
});

connection.on("ManuallyIncrementedScore", function(score) {
    console.log(score);
    document.querySelector('.js-word-choice').innerText = "";
    document.querySelector('.js-pixel-canvas-container').classList.remove('hidden');
    $scoreButton.classList.add("hidden");
    document.querySelector('.js-tool-bar').classList.add("hidden");
    $resetButton.classList.remove('hidden');
    document.getElementById('score').textContent = score;
    let name = document.querySelector('#my-name').value;
    console.log(name);
    console.log(score);
    // connection.invoke("SendMessage", name, score);
})

pixenaryConnection.on("BackgroundColor", function(backgroundColor){
    console.log("Hello");
    console.log(backgroundColor);

    $pixelCanvas.style.backgroundColor = backgroundColor;
});

document.querySelector('.js-fill-button').addEventListener('click', function() {
    console.log("Hello");
    pixenaryConnection.invoke("SetBackgroundColor", connectionName, color);
});
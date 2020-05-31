var pixenaryConnection = new signalR.HubConnectionBuilder().withUrl("/pixenaryHub").build();
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

var connectionName = "GroupOfJoshua"

var $table = document.querySelector('.js-pixenary-table');

var $colorTable = document.querySelector('.js-color-container');
var $setColoursButton = document.querySelector('.js-set-colors-button');

var $colorModal = document.querySelector('.js-color-modal');

var $colorSelectorContainer = document.querySelector('.js-color-selectors');

var colorList = ["#1E90FF", "#ff0000", "#ffff00", "#228B22", "#8B4513", "white"];

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
    let $colorInput = document.createElement('input');
    let $colorLabel = document.createElement('label');
    $colorInput.type = "color";
    $colorInput.className = "color-input";
    $colorLabel.className = "color";
    $colorLabel.style.backgroundColor = itemColor;
    $colorInput.value = itemColor;

    $colorInput.addEventListener('change', function() {
        $colorInput.parentNode.style.backgroundColor = $colorInput.value;
        colorList[index] = $colorInput.value;
    })
    $colorLabel.appendChild($colorInput);
    $colorSelectorContainer.appendChild($colorLabel);
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
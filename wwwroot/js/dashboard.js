"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

var users = [];
var myName = "Alana";

document.getElementById("sendDirectButton").disabled = true;

connection.start().then(function () {
    document.getElementById("sendDirectButton").disabled = false;
    connection.invoke("AddToAdminGroup", myName);
    console.log("Start AddToGroup");
}).catch(function (err) {
    return console.error((err.toString()));
});

connection.on("UserConnected", function(user) {
    console.log(user);
    if(!users.includes(user)) {
        console.log("Create");
        users.push(user);
        var container = document.createElement("div");
        container.id = "message-container-" + user;
        container.className = "message-container hidden";

        var messageList = document.getElementById("messageList");

        messageList.appendChild(container);
        console.log(">>>>" + user);
        connection.invoke("AddToGroup", user);

        console.log("> Hello <");

        var li = document.createElement("div");
        li.className = "message --left";
        li.textContent = user + " would like help";
        console.log("message-container-" + user);
        var messageContainer = document.getElementById("message-container-" + user);
        console.log("messageContainer");
        messageContainer.appendChild(li);

        var userBox = document.createElement("div");
        userBox.className = "user"
        userBox.id = "user-" + user;
        var nameBox = document.createElement("div");
        nameBox.textContent = user;
        nameBox.className = "name"
        userBox.appendChild(nameBox);

        var userList = document.getElementById("userList");
        userList.appendChild(userBox);

        // messageList.id = `message-container-${user}`;
        messageList.classList.remove("hidden");

        var subtitle = document.createElement('div');
        subtitle.textContent = "Just joined";
        subtitle.className = "user-subtitle";
        userBox.appendChild(subtitle);
        userBox.addEventListener('click', function(event) {
            console.log(event.currentTarget.textContent);
            var recipientName = event.currentTarget.querySelector(".name").textContent;
            document.getElementById('recipientInput').value = recipientName;
            var messageContainers = document.querySelectorAll('.message-container');
            messageContainers.forEach(container => {
                container.classList.add("hidden");
                console.log(container);
            });

            var userBoxes = document.querySelectorAll('.user');
            userBoxes.forEach(box => {
                box.classList.remove('user-focused');
            });

            event.currentTarget.classList.add('user-focused');
            document.getElementById("messageInput").value = "";

            document.getElementById(`message-container-${user}`).classList.remove("hidden");
        });
    }
});

connection.on("AdminConnected", function (name) {
    console.log(name);

    if (name == myName) {
        document.getElementById("messageList").classList.add("online");
    }

    console.log("Online")
});

connection.on("ReceiveMessage", function (user, friendlyName, message) {
    console.log(user);

    if (!users.includes(friendlyName)) {
        connection.invoke("AddToGroup", friendlyName);
        users.push(user);
    }

    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt").replace(/>/g, "&gt;");

    var li = document.createElement("div");
    li.className = "message --left";
    li.textContent = msg;
    var messageContainer = document.getElementById("message-container-" + friendlyName);
    messageContainer.appendChild(li);

    var currentUserBox = document.getElementById("user-" + friendlyName);
    console.log(currentUserBox);
    var subTitle = currentUserBox.querySelector(".user-subtitle");
    subTitle.textContent = message;
});

connection.on("ReceiveDirectMessage", function (recipient, myName, message) {
    console.log("Received direct message");
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt").replace(/>/g, "&gt;");
    var li = document.createElement("div");
    li.className = "message --right";
    li.textContent = msg;
    var messageContainer = document.getElementById("message-container-" + recipient);
    messageContainer.appendChild(li);
});

document.getElementById("sendDirectButton").addEventListener("click", function (event) {
    var recipient = document.getElementById('recipientInput').value;
    var myName = "Bob"; //document.getElementById("myName");
    var message = document.getElementById("messageInput").value;
    console.log("Direct");
    connection.invoke("SendDirectMessage", recipient, myName, message).catch(function (err) {
        return console.error(err.toString());
    });
});
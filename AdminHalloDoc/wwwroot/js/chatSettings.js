"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
//document.getElementById("sendButton").disabled = true;




var loginconnectionId = '';
connection.start().then(function () {
    connection.invoke("GetConnectionId").then(function (id) {
        console.log(id);
        loginconnectionId = id;
    });
   
}).catch(function (err) {
    return console.error(err.toString());
});


connection.on("ReceiveMessage", function (ConnectionId, user, message) {
    showNotification(user, message);
    console.log(user + ' ' + ConnectionId + ' ' + message);
    if (ConnectionId == loginconnectionId) {
        var li = document.createElement("div");
        var span = document.createElement("span");
        span.textContent = `you : ${message}`;
        li.appendChild(span);
        document.getElementById("messagesList").appendChild(li);
        li.classList.add("text-end");
        li.classList.add("first-replay");
        span.classList.add("reply"); 
    } else {
        var li = document.createElement("div");
        var span = document.createElement("span");
        span.textContent = `${user} says ${message}`;
        li.appendChild(span);
        document.getElementById("messagesList").appendChild(li);
        li.classList.add("text-start" );
        li.classList.add("first-replay" );
        span.classList.add("reply-sender"); 
    }
   
    console.log(li.textContent);
});

if (!("Notification" in window)) {
    console.error("This browser does not support desktop notification");
} else if (Notification.permission === "granted") {
    // If notification permission is already granted
    initConnection();
} else if (Notification.permission !== "denied") {
    // If notification permission is not denied, request permission
    Notification.requestPermission().then(function (permission) {
        if (permission === "granted") {
           
        }
    });
}
function showNotification(sender, message) {
    var notification = new Notification(sender, {
        body: message
    });
}
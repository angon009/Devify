"use strict";

//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var sender = document.getElementById("userInput").value;
//    var receiver = document.getElementById("userInput2").value;
//    var message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", sender, receiver, message).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});

function myFunction() {
    var sender = document.getElementById("userInput").value;
    var receiver = document.getElementById("userInput2").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", sender, receiver, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
}
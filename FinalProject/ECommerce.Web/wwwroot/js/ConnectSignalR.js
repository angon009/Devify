"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/messagehub")
    .build();

connection.start();

connection.on("ReceiveMessageHandler", function (message,sender,receiver) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    const username = document.getElementById("userEmail").value;  //from razor page
    generateHtml(msg, receiver, sender, username);
    document.getElementById("messageInput").value = "";
   
});


function generateHtml(messageText, receiver,sender, username) {
   
    let isOppositeUserMessage = receiver == sender;
    let isUser = receiver == username;
    const parent = document.getElementById("parentdiv"); //selecting parent
    const child = document.createElement("div");  //creating child

    child.className = isUser ?"chat-message-left mb-4":"chat-message-right mb-4";
    const childchild1 = document.createElement("div"); //creating childchild
    const imag = document.createElement("img");
    //image tag initialization
    imag.className = "rounded-circle mr-1";
    imag.src = "https://cdn-icons-png.flaticon.com/512/219/219983.png";
    imag.width = "40";
    imag.height = "40";
    const firstInnerDiv = document.createElement("div");
    firstInnerDiv.className = "text-muted small text-nowrap mt-2";
    firstInnerDiv.innerText = new Date().getHours() + ":" + new Date().getMinutes() + ":" + new Date().getSeconds();
    childchild1.appendChild(imag);
    childchild1.appendChild(firstInnerDiv);
    child.appendChild(childchild1);
   

    
    const childchild2 = document.createElement("div"); //creating childchild
    childchild2.className = "flex-shrink-1 bg-light rounded py-2 px-3 mr-3";

    const secondInnerDiv = document.createElement("div");
    secondInnerDiv.className = "font-weight-bold mb-1";
    secondInnerDiv.innerText = isUser ? sender : "You";
    childchild2.appendChild(secondInnerDiv);
    const messageDiv = document.createElement("div");
    messageDiv.innerText = messageText;
    childchild2.appendChild(messageDiv);
    child.appendChild(childchild2);
    parent.appendChild(child);
}
"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub?username=" + document.getElementById("userInput").value).build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message)
{
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});
connection.on("ReceiveMessageOne", function (user, message)
{
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = "*** "+user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function ()
{
    document.getElementById("sendButton").disabled = false;
    connection.invoke('GetConnectionId');
}).catch(function (err)
{
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event)
{
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err)
    {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("sendButtonCaller").addEventListener("click", function (event)
{
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessageOne", user, message, "aminRostami").catch(function (err)
    {
        return console.error(err.toString());
    });
    event.preventDefault();
});
//
window.ConnectionId = "";

connection.on("GetConnectionId", function (message)
{
    window.ConnectionId = message;
    console.log("GetConnectionId ===> ", message);
});


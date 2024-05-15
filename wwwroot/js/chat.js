"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `${user} says ${message}`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

function sendMessage() {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    document.getElementById("chatArea").innerHTML += "<p>You: " + message + "</p>";

    $.ajax({
        url: "/Chatbot/GetResponse",
        type: "GET",
        data: { message: message },
        success: function (response) {
            document.getElementById("chatArea").innerHTML += "<p>Chatbot: " + response.result + "</p>";
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}
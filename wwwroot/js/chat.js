"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (message) {
    var li = document.createElement("li");
    li.className = "bot-message";
    li.textContent = `${message}`;
    document.getElementById("messagesList").appendChild(li);

    var chatArea = document.getElementById("chatArea");
    chatArea.scrollTop = chatArea.scrollHeight;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();

    document.getElementById("messageInput").value = "";
});

function sendMessage() {
    var message = document.getElementById("messageInput").value;
    var userLi = document.createElement("li");
    userLi.className = "user-message";
    userLi.textContent = `You: ${message}`;
    document.getElementById("messagesList").appendChild(userLi);

    document.getElementById("sendButton").disabled = true;

    $.ajax({
        url: "/Chatbot/GetResponse",
        type: "GET",
        data: { message: message },
        success: function (response) {
            var botLi = document.createElement("li");
            botLi.className = "bot-message";
            botLi.textContent = `Chatbot: ${response.result}`;
            document.getElementById("messagesList").appendChild(botLi);

            document.getElementById("sendButton").disabled = false;

            var chatArea = document.getElementById("chatArea");
            chatArea.scrollTop = chatArea.scrollHeight;
        },
        error: function (xhr, status, error) {
            console.error(error);

            document.getElementById("sendButton").disabled = false;
        }
    });

    document.getElementById("messageInput").value = "";
}

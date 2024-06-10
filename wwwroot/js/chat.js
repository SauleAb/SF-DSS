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

document.querySelector(".new-chat").addEventListener("click", async function (event) {
    event.preventDefault();
    await saveConversation();
    startNewConversation();
});

async function saveConversation() {
    await fetch("/Chatbot/SaveConversation", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        }
    });
} //needs fixing

function startNewConversation() {
    document.getElementById("messagesList").innerHTML = "";
}

document.getElementById("sendButton").addEventListener("click", function (event) {
    event.preventDefault();
    sendMessage();
});

document.getElementById("messageInput").addEventListener("keypress", function (event) {
    if (event.key === "Enter") {
        event.preventDefault();
        sendMessage();
    }
});

function sendMessage() {
    var message = document.getElementById("messageInput").value;
    if (message.trim() === "") {
        return;
    }

    var userLi = document.createElement("li");
    userLi.className = "user-message";
    userLi.textContent = `You: ${message}`;
    document.getElementById("messagesList").appendChild(userLi);

    document.getElementById("sendButton").disabled = true;

    var typingLi = document.createElement("div");
    typingLi.id = "typingIndicator";
    typingLi.className = "loader";
    document.getElementById("messagesList").appendChild(typingLi);

    $.ajax({
        url: "/Chatbot/GetResponse",
        type: "GET",
        data: { message: message },
        success: function (response) {
            var typingIndicator = document.getElementById("typingIndicator");
            if (typingIndicator) {
                typingIndicator.remove();
            }

            var botLi = document.createElement("li");
            botLi.className = "bot-message";
            botLi.style.whiteSpace = "pre-wrap";
            botLi.innerText = "Chatbot: " + response;
            document.getElementById("messagesList").appendChild(botLi);

            document.getElementById("sendButton").disabled = false;

            var chatArea = document.getElementById("chatArea");
            chatArea.scrollTop = chatArea.scrollHeight;
        },
        error: function (xhr, status, error) {
            console.error(error);

            document.getElementById("sendButton").disabled = false;

            var typingIndicator = document.getElementById("typingIndicator");
            if (typingIndicator) {
                typingIndicator.remove();
            }
        }
    });

    document.getElementById("messageInput").value = "";
}
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

document.getElementById("new-chat").addEventListener("click", async function (event) {
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
    var chatArea = document.getElementById("chatArea");
    chatArea.removeAttribute("data-value");
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
        document.getElementById("btn-json").disabled = true;


        var typingLi = document.createElement("div");
        typingLi.id = "typingIndicator";
        typingLi.className = "loader";
        document.getElementById("messagesList").appendChild(typingLi);

        var chatArea = document.getElementById("chatArea");
        var convoId = chatArea.getAttribute("data-value");

        if (message.toLowerCase() === "give me json") {
                $.ajax({
                    url: "/Chatbot/GetJsonResponse",
                    type: "GET",
                    data: { message: message, convoId: convoId },
                    success: function (response) {
                        var typingIndicator = document.getElementById("typingIndicator");
                        if (typingIndicator) {
                            typingIndicator.remove();
                        }

                        var botLi = document.createElement("li");
                        botLi.className = "bot-message";
                        botLi.style.whiteSpace = "pre-wrap";
                        botLi.innerHTML = "Chatbot: Is this information correct?";


                        var table = document.createElement("table");
                        table.className = "greenhouse-table";
                        table.style.border = "1px solid black"; // Add border to the table
                        table.style.borderCollapse = "collapse"; // Collapse the borders

                        // Add table rows for each property of GreenhouseController
                        for (var prop in response) {
                            var row = document.createElement("tr");

                            var propCell = document.createElement("td");
                            propCell.innerText = " " + prop;
                            propCell.style.border = "1px solid black"; // Add border to the cells
                            row.appendChild(propCell);

                            var valueCell = document.createElement("td");
                            valueCell.innerText = " " + response[prop] + " ";
                            valueCell.style.border = "1px solid black"; // Add border to the cells
                            row.appendChild(valueCell);
                            
                            table.appendChild(row);
                        }

                        document.getElementById("messagesList").appendChild(botLi);

                        var botLi = document.createElement("li");
                        botLi.className = "bot-message";
                        botLi.style.whiteSpace = "pre-wrap";

                        botLi.appendChild(table);
                        document.getElementById("messagesList").appendChild(botLi);


                        var botLi = document.createElement("li");
                        botLi.className = "bot-message";
                        botLi.style.whiteSpace = "pre-wrap";
                        var sendButton = document.createElement("button");
                        sendButton.textContent = "Send to Greenhouse?";
                        sendButton.className = "btn btn-success";

                        botLi.appendChild(sendButton);
                        document.getElementById("messagesList").appendChild(botLi);

                        document.getElementById("sendButton").disabled = false;
                        document.getElementById("btn-json").disabled = false;

                        var chatArea = document.getElementById("chatArea");
                        chatArea.setAttribute("data-value", response.id);
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
        } else {
            $.ajax({
                url: "/Chatbot/GetResponse",
                type: "GET",
                data: { message: message, convoId: convoId },
                success: function (response) {
                    var typingIndicator = document.getElementById("typingIndicator");
                    if (typingIndicator) {
                        typingIndicator.remove();
                    }

                    var botLi = document.createElement("li");
                    botLi.className = "bot-message";
                    botLi.style.whiteSpace = "pre-wrap";
                    botLi.innerText = "Chatbot: " + response.response;
                    document.getElementById("messagesList").appendChild(botLi);

                    document.getElementById("sendButton").disabled = false;
                    document.getElementById("btn-json").disabled = false;

                    var chatArea = document.getElementById("chatArea");
                    chatArea.setAttribute("data-value", response.id);
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
        }

        document.getElementById("messageInput").value = "";
    }
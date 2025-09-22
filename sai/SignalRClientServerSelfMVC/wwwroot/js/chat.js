"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});


//connection.on("OpcUaReceiveMessage", function (message) {   
//    console.log(message);
//    var receivedData = JSON.parse(message);

//    console.log(receivedData);
//    var tagName = receivedData.TagName;
//    var tagValue = receivedData.TagValue;
//    console.log(tagName);
//    console.log(tagValue);

//    var element = document.getElementById(tagName);
//    element.classList.remove("red_box");
//    element.classList.remove("green_box");
//    element.classList.remove("yellow_box");

//    if (tagValue == 0) {
//        element.classList.add("yellow_box");
//    } else if (tagValue == 1) {
//        element.classList.add("green_box");
//    } else {
//        element.classList.add("red_box");
//    }
    
//});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
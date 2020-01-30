"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();// Для взаимодействия с хабом ChatHub с помощью метода build() объекта HubConnectionBuilder 
                                                                                // создается объект connection - объект подключения. Метод withUrl устанавливает адрес, 
                                                                                //по котору приложение будет обращаться к хабу.

//Выключаем кнопку, пока не будет установлено соединение с хабом
document.getElementById("sendButton").disabled = true;

// Устанавливаем метод на стороне клиента, он будет получать данные от нашего хаба, в данном случае метод нашего хаба называется "Send"
// и фактически он представляет функцию, которая передается в качестве второго параметра
connection.on("Receive", function (Messages)
{
    
    for (var i = 0; i < Messages.length; ++i)// Итерируем список объектов
    {
        for (var Messagekey in Messages[i])// Итерируем свойства конкретного объекта
        {
            if (Messagekey === "message")
            {
                var msg = Messages[i][Messagekey].replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
            }
            if (Messagekey === "name")
            {
                var name = Messages[i][Messagekey]
            }
        }
        var encodedMsg = name + " сказал " + msg;
        var li = document.createElement("li");
        li.textContent = encodedMsg;
        document.getElementById("messagesList").appendChild(li);// Просто добавляем в наш список на cshtml страничке, элемент <li>
    }
});
connection.on("ReceiveOne", function (Message) {


    for (var Messagekey in Message)// Итерируем свойства конкретного объекта
    {
       if (Messagekey === "message") {
                var msg = Message[Messagekey].replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
       }
       if (Messagekey === "name") {
                var name = Message[Messagekey]
       }
    }
        var encodedMsg = name + " сказал " + msg;
        var li = document.createElement("li");
        li.textContent = encodedMsg;
        document.getElementById("messagesList").appendChild(li);// Просто добавляем в наш список на cshtml страничке, элемент <li>
    
});
connection.on("Notify", function (ipMessage)
{
    var userLi = document.createElement("li");
    userLi.textContent = ipMessage;
    document.getElementById("messagesList").appendChild(userLi);
});

connection.start().then(function () // Для начала соединения с сервером (тоесть с нашим хабом) вызывается метод "start()"
{
    document.getElementById("sendButton").disabled = false;// Если соединение было установлено, то делаем кнопку активной
}).catch(function (err)
{
    return console.error(err.toString());// Если соединение не установлено, то пишем в консоль браузера ошибку
});

document.getElementById("sendButton").addEventListener("click", function (event)//Обработчик для кнопки, который вызывается при её нажатии
{
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    // Для отправки ведённых данных, хабу на сервер, вызывается метод "connection.invoke() с тремя параметрами"
    // первый параметр представляет название метода хаба, обрабатывающее данный запрос, второй и третий параметры - данные отправляемые хабу.
    connection.invoke("SendMessage", user, message).catch(function (err)
    {
        return console.error(err.toString());//если не получилось, пишем в консоль браузера ошибку
    });

    event.preventDefault();
});
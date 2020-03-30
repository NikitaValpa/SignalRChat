"use strict";

let connection = new signalR.HubConnectionBuilder().withUrl("/Chat/Chater")
    .withAutomaticReconnect()
    //.configureLogging(signalR.LogLevel.Trace)
    .build();// Для взаимодействия с хабом ChatHub с помощью метода build() объекта HubConnectionBuilder 
    // создается объект connection - объект подключения. Метод withUrl устанавливает адрес, //по котору приложение будет обращаться к хабу. //Выключаем кнопку, пока не будет установлено соединение с хабом

let UserNames = [];
document.getElementById("sendButton").disabled = true;

// Устанавливаем метод на стороне клиента, он будет получать данные от нашего хаба, в данном случае метод нашего хаба называется "Send"
// и фактически он представляет функцию, которая передается в качестве второго параметра
connection.on("Receive", function (Messages)
{
    
    for (let i = 0; i < Messages.length; ++i)// Итерируем список объектов
    {
        for (let Messagekey in Messages[i])// Итерируем свойства конкретного объекта
        {
            if (Messagekey === "message")
            {
                var msg = Messages[i][Messagekey].replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
            }
            if (Messagekey === "name")
            {
                var name = Messages[i][Messagekey]
            }
            if (Messagekey === "sendDate") {
                var date = Messages[i][Messagekey]
            }
        }
        let encodedMsg = date.slice(0, 10) + " " + date.slice(11, 19) +": "+ name + " сказал " + msg;
        let li = document.createElement("li");
        li.textContent = encodedMsg;
        document.getElementById("messagesList").appendChild(li);// Просто добавляем в наш список на cshtml страничке, элемент <li>
    }
});
connection.on("ReceiveOne", function (Message) {


    for (let Messagekey in Message)// Итерируем свойства конкретного объекта
    {
       if (Messagekey === "message") {
            var msg = Message[Messagekey].replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
       }
       if (Messagekey === "name") {
            var name = Message[Messagekey]
       }
       if (Messagekey === "sendDate") {
            var date = Message[Messagekey]
       }
    }
    let encodedMsg = date.slice(0, 10) + " " + date.slice(11, 19) + ": " + name + " сказал " + msg;
    let li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);// Просто добавляем в наш список на cshtml страничке, элемент <li>
    
});
connection.on("Notify", function (UserNames)
{
    let list = document.getElementById("Users");
    while (list.firstChild) {//Чистим список, перед отрисовкой
        list.removeChild(list.firstChild);
    }
    for (let Uservalue of UserNames) {//Отрисовываем список, перебирая наш массив
        let userLi = document.createElement("li");
        userLi.textContent = Uservalue;
        document.getElementById("Users").appendChild(userLi);
    }
});

document.getElementById("sendButton").addEventListener("click", function (event)//Обработчик для кнопки, который вызывается при её нажатии
{
    let user = document.getElementById("userInput").value;
    let message = document.getElementById("messageInput").value;
    // Для отправки ведённых данных, хабу на сервер, вызывается метод "connection.invoke() с тремя параметрами"
    // первый параметр представляет название метода хаба, обрабатывающее данный запрос, второй и третий параметры - данные отправляемые хабу.
    connection.invoke("SendMessage", user, message).catch(function (err)
    {
        return console.error(err.toString());//если не получилось, пишем в консоль браузера ошибку
    });

    event.preventDefault();
});

connection.start().then(function () // Для начала соединения с сервером (тоесть с нашим хабом) вызывается метод "start()"
{
    console.log("Соединение установлено: ID " + connection.connectionId);
    document.getElementById("sendButton").disabled = false;// Если соединение было установлено, то делаем кнопку активной
}).catch(function (err) {
    return console.error(err.toString());// Если соединение не установлено, то пишем в консоль браузера ошибку
});

connection.onreconnecting((error) => {// Обработка ошибок переподключения
    console.assert(connection.state === signalR.HubConnectionState.Reconnecting);
    console.log(`Соединение потеряно из-за ошибки: "${error}". реконнект.`);
});

connection.onreconnected((connectionId) => {// Обработка подключения
    console.assert(connection.state === signalR.HubConnectionState.Connected);
    console.log(`Соединение восстановлено. Связано с идентификатором соединения: "${connectionId}".`);
});

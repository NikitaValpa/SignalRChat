/*Данный скрипт управляет другими скриптами на странице и асинхронно загружает страницы по ссылкам, делая при этом плавный переход*/
"use strict";
let linkLocation;
$(document).ready(function () {//Срабатывает 1 раз при загрузке страницы с нуля
	/*Создаём объекты наших скриптов для управления ими*/
	/*Весь код который находится ниже выполняется только при начальной загрузке страницы или при её обновлении*/

	let ScriptChat = new ChatScript();
	let url = new URL(window.location.href);


	if (url.pathname == "/Chat") {
		ScriptChat.startQuery();
	}
	/*Вешаем обработчики на все ссылки на странице 1 раз при загрузке страницы, потом уже вещаем обработчики только на ссылки которые находяться в мейне*/
	$("a").click(function (event) {
		event.preventDefault();//Отключаем стандартный обработчик для нажатия по ссылке
		linkLocation = this.href;//Запоминаем url на который хотим перейти

		if (window.location.pathname == "/Chat") {
			ScriptChat.stopQuery();
		}
		console.debug("Нажал на ссылку");
		history.pushState(null, null, linkLocation);
		loadTitleAsync();
		$("div#maincontainer").fadeOut(1000, loadContentAsync);//Плавно затеняем контент и асинхронно загружаем новый по новому url
	});

	/*Вся байда ниже выполняется только при клике на любую ссылку на странице*/
	function addListenersOnLinks() { //Функция которая вещает обработчик события клик на все ссылки на странице
		$("main a").click(function (event) {
			event.preventDefault();//Отключаем стандартный обработчик для нажатия по ссылке
			linkLocation = this.href;//Запоминаем url на который хотим перейти

			if (window.location.pathname == "/Chat") {
				ScriptChat.stopQuery();
			}
			console.debug("Нажал на ссылку");
			history.pushState(null, null, linkLocation);
			loadTitleAsync();
			$("div#maincontainer").fadeOut(1000, loadContentAsync);//Плавно затеняем контент и асинхронно загружаем новый по новому url
		});
	}

	function loadContentAsync() {
		$("div#maincontainer").load(linkLocation + " " + "main", function (response, status, xhr) {
			if (status == "error") {
				var msg = "Извините, но произошла ошибка: ";
				$("div#maincontainer").html(msg + xhr.status + " " + xhr.statusText);
			}
			$("div#maincontainer").fadeIn(1000);
			console.debug("Загрузил страницу");
			let path = window.location.pathname;

			if (path == "/Chat") {//Если новая страница успешно загрузилась, то исходя из её пути мы запускаем на исполнение нужный скрипт
				ScriptChat.startQuery();
			}
			addListenersOnLinks();//Вызываем повторно, чтобы повесить обработчик собыйтий на те ссылки, которые появились при загрузке страницы
		});
	}
	function loadTitleAsync() {//Функция которая делает get запрос к серверу, получает ответ, потом парсит ответ и меняет title на новый
		$.get(linkLocation, function (data) {

			let firstIndex = data.indexOf("<title>") + 7;
			let lastIndex = data.indexOf("</title>");
			let titleValue = data.slice(firstIndex, lastIndex);
			document.title = $("title").html(titleValue).text();
		})
	}
});
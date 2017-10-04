function block_smiles(){
	display = document.getElementById('smiles').style.display;
	
	if(display=='none')
		document.getElementById('smiles').style.display='block';
	else
		document.getElementById('smiles').style.display='none';
	}


function smile_textarea(smile){
	$("textarea").val($("textarea").val() + smile);
	}

	
$(function(){
	//Переменная отвечает за id последнего пришедшего сообщения
	var mid = 0;
	//Функция обновления сообщений чата
	function get_message_chat(){
		//Генерируем Ajax запрос
		$.ajaxSetup({url: "messages.php",global: true,type: "POST",data: "event=get&id="+mid+"&t="+(new Date).getTime()});
		//Отправляем запрос
		$.ajax({
			//Если все удачно
			success: function(msg_j){
				//Если есть сообщения в принятых данных
				/* если все сообщения в чате есть, то сервер присылает не пустую строку, а "[]". т.к. ответ генерируется в JSON. */
				if(msg_j.length > 2){
					//Парсим JSON
					var obj = JSON.parse(msg_j);
					
					//Проганяем циклом по всем принятым сообщениям
					for(var i=0; i < obj.length; i ++){
						//Присваиваем переменной ID сообщения
						mid = obj[i].id;
						//Добавляем в чат сообщение
						$("#messages").append('<p class="info_text inline_text">'+
													'['+
											  '</p>'+
											  '<p class="error_text inline_text">'+
													obj[i].login+
											  '</p>'+
											  '<p class="info_text inline_text">'+
													'|'+
													obj[i].datetime+
													']> '+
											  '</p>'+
											  '<p class="inline_text">'+
													obj[i].mess+
											  '</p>'+
											  '<br>');
						}
					//Прокручиваем чат до самого конца
					$("#messages").scrollTo("100%");
					}
				}
			});
		}
 
	//Первый запрос к серверу. Принимаем сообщения
	get_message_chat();
 
	//Обновляем чат
	$("#messages").everyTime(1000, 'refresh', function() {
		get_message_chat();
		});
	//setInterval('get_message_chat()',1000);
	});


$(function(){
	//Переменная отвечает за id последнего пришедшего сообщения
	var mid = 0;
	//Функция обновления сообщений чата
	function get_message_chat(){
		//Генерируем Ajax запрос
		$.ajaxSetup({url: "messages.php",global: true,type: "POST",data: "event=get&id="+mid+"&t="+(new Date).getTime()});
		//Отправляем запрос
		$.ajax({
			//Если все удачно
			success: function(msg_j){
				//Если есть сообщения в принятых данных
				/* если все сообщения в чате есть, то сервер присылает не пустую строку, а "[]". т.к. ответ генерируется в JSON. */
				if(msg_j.length > 2){
					//Парсим JSON
					var obj = JSON.parse(msg_j);
					
					//Проганяем циклом по всем принятым сообщениям
					for(var i=0; i < obj.length; i ++){
						//Присваиваем переменной ID сообщения
						mid = obj[i].id;
						//Добавляем в чат сообщение
						$("#amessages").append('<form class="xxx" action="form_delete_mess.php" method="post" style="display: inline;">'+
													'<input type="submit" value="X">'+
													'<input type="hidden" name="id_mess" value="'+obj[i].id+'">'+
											  '</form>'+
											  '<p class="info_text inline_text">'+
													'['+
											  '</p>'+
											  '<p class="error_text inline_text">'+
													obj[i].login+
											  '</p>'+
											  '<p class="info_text inline_text">'+
													'|'+
													obj[i].datetime+
													']> '+
											  '</p>'+
											  '<p class="inline_text">'+
													obj[i].mess+
											  '</p>'+
											  '<br>');
						}
					//Прокручиваем чат до самого конца
					$("#amessages").scrollTo("100%");
					}
				}
			});
		}
 
	//Первый запрос к серверу. Принимаем сообщения
	get_message_chat();
 
	//Обновляем чат
	$("#amessages").everyTime(1000, 'refresh', function() {
		get_message_chat();
		});
	//setInterval('get_message_chat()',1000);
	});

	
/*
function show(){
	$("#messages").load("messages.php");
	$("#messages").scrollTo("100%");
	}
	
$(document).ready(function(){
	show();
	setInterval('show()',1000);
	});
*/


function online_update(){
	$("#online").load("online.php");
	}
	
$(document).ready(function(){
	online_update();
	setInterval('online_update()',1000);
	});

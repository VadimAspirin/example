<?php
	$db = mysqli_connect ("localhost", "", "", "chat_db");
	mysqli_query ($db, "SET NAMES 'utf8';");
	mysqli_query ($db, "SET CHARACTER SET 'utf8';");
	mysqli_query ($db, "SET SESSION collation_connection = 'utf8_general_ci';");
	
	// удаляем из онлайна протухших пользователей
	mysqli_query ($db, "DELETE FROM session WHERE putdate < SYSDATE() -  INTERVAL '5' MINUTE");
	
	$count_users = mysqli_query ($db, "SELECT count(login) as cl FROM session");
	while ($row = mysqli_fetch_array ($count_users)) {
		print_r ('<p class="info_text inline_text">Сейчас в чате: (</p><p class="error_text inline_text">'.$row['cl'].'</p><p class="info_text inline_text">)</p><br>');
		}

	$result = mysqli_query ($db, "SELECT login FROM session");
	while ($row = mysqli_fetch_array ($result)) {
		print_r ('<p class="error_text inline_text">'.$row['login'].'</p><br>');
		}
?>

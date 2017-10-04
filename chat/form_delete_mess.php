<?php
	$db = mysqli_connect ("localhost", "", "", "chat_db");
	mysqli_query ($db, "SET NAMES 'utf8';");
	mysqli_query ($db, "SET CHARACTER SET 'utf8';");
	mysqli_query ($db, "SET SESSION collation_connection = 'utf8_general_ci';");
	
	mysqli_query ($db, "DELETE FROM messages WHERE id = '$_POST[id_mess]'");
	
	header ('Location: index.php');
?>

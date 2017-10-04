<html>
	<head>
		<meta charset="utf-8">
		<title>Чат - Вход</title>
		<link rel="stylesheet" type="text/css" href="css/main.css">
	</head>
	<body>
		<div id="block_login">
			<?php
				session_start ();
				
				$db = mysqli_connect ("localhost", "", "", "chat_db");
				mysqli_query ($db, "SET NAMES 'utf8';");
				mysqli_query ($db, "SET CHARACTER SET 'utf8';");
				mysqli_query ($db, "SET SESSION collation_connection = 'utf8_general_ci';");
			?>
			<h1 class="header_text">Чат</h1>
			<form method="post">
				<input type="text" name="login" placeholder="Логин"><br>
				<input type="password" name="passwd" placeholder="Пароль"><br>
				<input type="submit" name="send" value="Войти">
			</form>
			<form method="link" action="registration.php">
				<input type="submit" value="Зарегистрироваться">
			</form>
			<?php
				$result = mysqli_query ($db, "SELECT fname, sname, login, passwd, role FROM registration");
				if (isset($_POST['send'])) {
					while ($row = mysqli_fetch_array ($result)) {
						if ($row['login'] == $_POST['login'] && password_verify ($_POST['passwd'], $row['passwd'])) {
							$_SESSION['fname'] = $row['fname'];
							$_SESSION['sname'] = $row['sname'];
							$_SESSION['login'] = $row['login'];
							$_SESSION['role'] = $row['role'];
							header ('Location: index.php');
							exit (0);
							}
						}
						mysqli_free_result ($result);
						?><p class="table_td size center error_text">Неверный логин или<br>пароль!</p><?php
						exit (1);
					}
				else {
					?><p class="table_td size center info_text">Войдите или зарегистрируйтесь, что бы начать общение</p><?php
					}
					
				mysqli_close ($db);
			?>
		</div>
	</body>
</html>

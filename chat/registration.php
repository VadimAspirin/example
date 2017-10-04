<html>
	<head>
		<meta charset="utf-8">
		<title>Чат - Регистрация</title>
		<link rel="stylesheet" type="text/css" href="css/main.css">
	</head>
	<body>
		<div id="block_registration">
			<?php
				session_start ();
				
				$db = mysqli_connect ("localhost", "", "", "chat_db");
				mysqli_query ($db, "SET NAMES 'utf8';");
				mysqli_query ($db, "SET CHARACTER SET 'utf8';");
				mysqli_query ($db, "SET SESSION collation_connection = 'utf8_general_ci';");
			?>
			<h1 class="header_text">Регистрация</h1>
			<form method="post">
				<input type="text" name="fname" maxlength="20" required placeholder="Имя"><br>
				<input type="text" name="sname" maxlength="20" required placeholder="Фамилия"><br>
				<input type="text" name="login" maxlength="20" required placeholder="Логин"><br>
				<input type="password" name="passwd" required placeholder="Пароль"><br>
				<input type="password" name="rpasswd" required placeholder="Повторите пароль"><br>
				<input type="submit" name="send" value="Зарегистрироваться">
			</form>			
			<?php
				if (isset ($_POST['send'])) {
					$result = mysqli_query ($db, "SELECT login FROM registration");
					
					while ($row = mysqli_fetch_array ($result)) {
						if ($row['login'] == $_POST['login']) {
							?><p class="table_td size center error_text">Пользователь с таким логином уже существует!</p><?php
							exit (1);
							}
						}
					
					mysqli_free_result ($result);
					
					if ($_POST['passwd'] != $_POST['rpasswd']) {
						?><p class="table_td size center error_text">Пароли не совпадают!</p><?php
						exit (1);
						}
					mysqli_query ($db, "INSERT INTO registration (fname, sname, login, passwd)
									    VALUES ('".$_POST['fname']."', '".$_POST['sname']."', '"
												  .$_POST['login']."', '".password_hash($_POST['passwd'],PASSWORD_DEFAULT)."')");
					$_SESSION['fname'] = $_POST['fname'];
					$_SESSION['sname'] = $_POST['sname'];
					$_SESSION['login'] = $_POST['login'];
					
					header ('Location: index.php');
					}
				else {
					?><p class="table_td size center info_text">Заполните все поля</p><?php
					}
				
				mysqli_close ($db);
			?>
		</div>
	</body>
</html>

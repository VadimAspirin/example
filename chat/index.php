<html>
	<head>
		<meta charset="utf-8">
		<title>Чат</title>
		<script type="text/javascript" src="js/jquery.js"></script>
		<script type="text/javascript" src="js/jquery.scrollTo-min.js"></script>
		<script type="text/javascript" src="js/jquery.timers.js"></script>
		<script type="text/javascript" src="js/myscript.js"></script>
		<link rel="stylesheet" type="text/css" href="css/main.css">
	</head>
	<body>
		<?php
			session_start();
			
			if (!array_key_exists ('login', $_SESSION)) {
				header ('Location: login.php');
				exit (0);
				}
			
			$id_session = session_id();
			
			$db = mysqli_connect ("localhost", "", "", "chat_db");
			mysqli_query ($db, "SET NAMES 'utf8';");
			mysqli_query ($db, "SET CHARACTER SET 'utf8';");
			mysqli_query ($db, "SET SESSION collation_connection = 'utf8_general_ci';");
			
			$ses = mysqli_query ($db, "SELECT * FROM session WHERE idsession = '$id_session'");
			// обновить онлайн
			if (mysqli_num_rows ($ses) > 0) {
				mysqli_query ($db, "UPDATE session SET putdate = SYSDATE(), login = '$_SESSION[login]' WHERE idsession = '$id_session'");
				}
			// пользователь только зашёл
			else {
				$buf = mysqli_query ($db, "SELECT * FROM session WHERE login = '$_SESSION[login]'");
				if (mysqli_num_rows ($buf) > 0) {
					mysqli_query ($db, "DELETE FROM session WHERE login = '$_SESSION[login]'");
					}
				mysqli_query ($db, "INSERT INTO session VALUES ('$id_session', SYSDATE(), '$_SESSION[login]')");
				}
			
		?>
			<div id="block_top">
				<div id="top_left">
					<p class="info_text">Вы вошли, как <i class="underline"><?php print_r ($_SESSION['fname'] . ' ' . $_SESSION['sname']) ?></i></p>
				</div>
				<div id="top_right">
					<form method="post">
						<input id="submit_exit" type="submit" value="Выйти" name="s_exit">
					</form>
				</div>
			</div>
		<?php
			if (isset ($_POST['s_exit'])) {
				//убираем из онлайн
				mysqli_query ($db, "DELETE FROM session WHERE idsession = '$id_session'");
				header ('Location: login.php');
				session_destroy ();
				exit (0);
				}
		?>	
			<div id="block_center">
				<?php
				if ($_SESSION['role'] == 'admin') {
					?>
					<div id="amessages">
						<!-- messages.php -->
					</div>
					<?php
					}
				else {
					?>
					<div id="messages">
						<!-- messages.php -->
					</div>
					<?php
					}
				?>
				<div id="smiles" style="display: none;">
					<img class="smile" src="smiles/1.png" alt=":-)" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/2.png" alt=":-D" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/3.png" alt=";-)" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/4.png" alt="xD" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/5.png" alt=";-P" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/6.png" alt=":-p" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/7.png" alt="8-)" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/8.png" alt="B-)" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/9.png" alt=":-(" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/10.png" alt=";-]" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/11.png" alt="3(" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/12.png" alt=":*(" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/13.png" alt=":_(" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/14.png" alt=":((" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/15.png" alt=":O" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/16.png" alt=":|" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/17.png" alt="3-)" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/18.png" alt="O:)" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/19.png" alt=";o" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/20.png" alt="8o" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/21.png" alt="8|" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/22.png" alt=":X" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/23.png" alt="<3" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/24.png" alt=":-*" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/25.png" alt=">(" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/26.png" alt=">|(" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/27.png" alt=":-]" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/28.png" alt="}:)" onclick="smile_textarea(this.alt);">
					<img class="smile" src="smiles/29.png" alt=":like:" onclick="smile_textarea(this.alt);">
				</div>
				<div id="online">
					<!-- online.php -->
				</div>
			</div>

			<div id="block_footer">
				<button class="submit_footer" onclick="block_smiles();">Смайлы</button>
				<form method="post" style="display: inline;">
					<input class="submit_footer" type="submit" name="send" value="Отправить">
					<textarea id="textaria_mess" type="text" name="mess" placeholder="Ваше сообщение" maxlength="255" autofocus></textarea>
				</form>
			</div>
		<?php
			if (isset ($_POST['send'])) {
				if (iconv_strlen($_POST['mess']) != 0) {
					//print_r (iconv_strlen ($_POST['mess']));
					//DATE_FORMAT(SYSDATE(),'%Y-%m-%d %H:%i:%s')
					mysqli_query ($db, "INSERT INTO messages (datetime, login, mess) VALUES (SYSDATE(), '".$_SESSION['login']."', '".$_POST['mess']."')");
					header('location:'.$_SERVER['HTTP_REFERER']);
					exit (0);
					}
				}
		?>
	</body>
</html>

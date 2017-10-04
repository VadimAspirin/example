<?php
	function getQuery ($db, $query) {
		$res = mysqli_query ($db, $query);
		$row = mysqli_fetch_row ($res);
		$var = $row[0];
		return $var;
		}
	
	$db = mysqli_connect ("localhost", "", "", "chat_db");
	mysqli_query ($db, "SET NAMES 'utf8';");
	mysqli_query ($db, "SET CHARACTER SET 'utf8';");
	mysqli_query ($db, "SET SESSION collation_connection = 'utf8_general_ci';");
	
	if (isset ($_POST['event'])) {
		
		$max_message = 50;
		
		$last_id_message_server = getQuery ($db, "SELECT MAX(id) FROM messages");
		$last_id_message_client = $_POST['id'];
		// Сколько сообщений нехватает клиенту
		if ($last_id_message_client == 0) {
			$last_id_message_client = $last_id_message_server - $max_message;}
		if ($last_id_message_client < 0) {
			$last_id_message_client = 0;
			}
		$msg = array();
		
		if ($last_id_message_client < $last_id_message_server) {
			
			$query = "SELECT * FROM `chat` WHERE `id`>".$last_id_message_client." AND `id`<=".$last_id_message_server." ORDER BY `id` ";
			$result = mysqli_query ($db, "SELECT * FROM messages WHERE id > ".$last_id_message_client." AND id <= ".$last_id_message_server." ORDER BY id");
			while ($row = mysqli_fetch_array ($result)) {
				/* https://vk.com/emoji_vk */
				$mess = str_replace (
					array (
						":-)",
						":-D",
						";-)",
						"xD",
						";-P",
						":-p",
						"8-)",
						"B-)",
						":-(",
						";-]",
						"3(",
						":*(",
						":_(",
						":((",
						":O",
						":|",
						"3-)",
						"O:)",
						";o",
						"8o",
						"8|",
						":X",
						"<3",
						":-*",
						">(",
						">|(",
						":-]",
						"}:)",
						":like:",
						":dislike:",
						":up:",
						":v:",
						":ok:"
							), array (
								'<img class="smile" src="smiles/1.png">',
								'<img class="smile" src="smiles/2.png">',
								'<img class="smile" src="smiles/3.png">',
								'<img class="smile" src="smiles/4.png">',
								'<img class="smile" src="smiles/5.png">',
								'<img class="smile" src="smiles/6.png">',
								'<img class="smile" src="smiles/7.png">',
								'<img class="smile" src="smiles/8.png">',
								'<img class="smile" src="smiles/9.png">',
								'<img class="smile" src="smiles/10.png">',
								'<img class="smile" src="smiles/11.png">',
								'<img class="smile" src="smiles/12.png">',
								'<img class="smile" src="smiles/13.png">',
								'<img class="smile" src="smiles/14.png">',
								'<img class="smile" src="smiles/15.png">',
								'<img class="smile" src="smiles/16.png">',
								'<img class="smile" src="smiles/17.png">',
								'<img class="smile" src="smiles/18.png">',
								'<img class="smile" src="smiles/19.png">',
								'<img class="smile" src="smiles/20.png">',
								'<img class="smile" src="smiles/21.png">',
								'<img class="smile" src="smiles/22.png">',
								'<img class="smile" src="smiles/23.png">',
								'<img class="smile" src="smiles/24.png">',
								'<img class="smile" src="smiles/25.png">',
								'<img class="smile" src="smiles/26.png">',
								'<img class="smile" src="smiles/27.png">',
								'<img class="smile" src="smiles/28.png">',
								'<img class="smile" src="smiles/29.png">',
								'<img class="smile" src="smiles/30.png">',
								'<img class="smile" src="smiles/31.png">',
								'<img class="smile" src="smiles/32.png">',
								'<img class="smile" src="smiles/33.png">'
									), $row['mess']);
				$msg[] = array("id"=>$row['id'], "login"=>$row['login'], "datetime"=>$row['datetime'], "mess"=>$mess);
				}
			}
		echo json_encode ($msg, JSON_UNESCAPED_UNICODE);
		}
		
	//mysqli_free_result ($result);
	//mysqli_close ($db);
?>

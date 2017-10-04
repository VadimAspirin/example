<?php

function curl_get_url($url)
{
    $curl = curl_init();
    curl_setopt($curl, CURLOPT_URL, $url);
    curl_setopt($curl, CURLOPT_HEADER, 0);
    curl_setopt($curl, CURLOPT_TIMEOUT, 1);
    curl_setopt($curl, CURLOPT_FOLLOWLOCATION, 0);
    curl_setopt($curl, CURLOPT_RETURNTRANSFER, 1);
    curl_exec($curl);
    curl_close($curl);
}

function sendMessageTelegramBot($token, $chat_id, $msg)
{
    curl_get_url("https://api.telegram.org/bot".$token."/sendmessage?chat_id=".$chat_id."&text=".urlencode($msg));
}


ignore_user_abort(1);
require_once './avito_parse/index.php';

$token = "your_token";
$chat_id = "your_chat_id";
$msg = "";

if(file_exists("./stop.txt"))
{
    sendMessageTelegramBot($token, $chat_id, "[Avito парсер остановлен!]");
    exit;
}

/**** PARSE PAGES ****/
$msg .= avito_parse("https://www.avito.ru/chelyabinskaya_oblast/tovary_dlya_kompyutera/komplektuyuschie/videokarty", "GPU");
$msg .= avito_parse("https://www.avito.ru/chelyabinskaya_oblast/tovary_dlya_kompyutera/komplektuyuschie/protsessory", "CPU");
$msg .= avito_parse("https://www.avito.ru/chelyabinskaya_oblast/tovary_dlya_kompyutera/komplektuyuschie/materinskie_platy", "MB");
$msg = str_replace("<br>", "\n", $msg);

if(strlen($msg))
    sendMessageTelegramBot($token, $chat_id, $msg);

/**** TIMER ****/
sleep(420);

curl_get_url("http://www.v96asp69.beget.tech/AvitoParseTelegramBot/index.php");
exit;

?>

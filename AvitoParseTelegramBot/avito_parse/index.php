<?php

function avito_parse($url, $specification)
{
    require_once 'simple_html_dom.php';

    $page = file_get_html($url);
    
    $products = $page->find('div[data-type="1"]');
    
    $msg = "";
    foreach($products as $product)
    {
                if(!mb_substr_count($product->find('div.data',0), "your_city")
                    continue;
        		$msg .= trim($product->find('a.item-description-title-link',0)->plaintext); // заголовок
        		$msg .= "<br>";
        		$msg .= 'https://www.avito.ru' . trim($product->find('a.item-description-title-link',0)->href); // ссылка
        		$msg .= "<br>";
        		$msg .= trim($product->find('div.about',0)->plaintext); // цена
        		$msg .= "<br><br>";
        		$msg .= "\n";
    }
    $msg = str_replace("   &#160;&nbsp;", " (СНИЖЕНА)", $msg);
    file_put_contents("./avito_parse/parsed/".$specification."-products.txt",$msg);
    $msg_buf = $msg;
    $msg = "";
    
    if(file_exists("./avito_parse/parsed/".$specification."-last_products.txt"))
    {
        $last_products = explode("\n",file_get_contents("./avito_parse/parsed/".$specification."-last_products.txt"));
        $products = explode("\n",file_get_contents("./avito_parse/parsed/".$specification."-products.txt"));
        if($last_products != $products)
        {
            $new_products = array_diff($products, $last_products);
        	foreach($new_products as $product)
        		$msg .= $product;
        }
    }
    
    file_put_contents("./avito_parse/parsed/".$specification."-last_products.txt",$msg_buf);
    
    if(strlen($msg))
    	{
    	    $msg = "***** " . $specification . " *****<br>" . $msg;
    	    return $msg;
    	}
}

?>

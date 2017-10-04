<?php
require_once 'simple_html_dom.php';

$urls = [
/*'http://www.lineaflex.ru/good/28',
'http://www.lineaflex.ru/good/81',
'http://www.lineaflex.ru/good/26',
'http://www.lineaflex.ru/good/84',
'http://www.lineaflex.ru/good/25',
'http://www.lineaflex.ru/good/24',
'http://www.lineaflex.ru/good/82',
'http://www.lineaflex.ru/good/83',
'http://www.lineaflex.ru/good/23',
'http://www.lineaflex.ru/good/19',
'http://www.lineaflex.ru/good/22',
'http://www.lineaflex.ru/good/20',
'http://www.lineaflex.ru/good/21',
'http://www.lineaflex.ru/good/29',
'http://www.lineaflex.ru/good/30',
'http://www.lineaflex.ru/good/32',
'http://www.lineaflex.ru/good/33',
'http://www.lineaflex.ru/good/71',
'http://www.lineaflex.ru/good/35',
'http://www.lineaflex.ru/good/36',
'http://www.lineaflex.ru/good/38',
'http://www.lineaflex.ru/good/34',
'http://www.lineaflex.ru/good/37',
'http://www.lineaflex.ru/good/39',
'http://www.lineaflex.ru/good/78',
'http://www.lineaflex.ru/good/31',
'http://www.lineaflex.ru/good/114',
'http://www.lineaflex.ru/good/111',
'http://www.lineaflex.ru/good/115',
'http://www.lineaflex.ru/good/113',
'http://www.lineaflex.ru/good/112',
'http://www.lineaflex.ru/good/110',
'http://www.lineaflex.ru/good/121',
'http://www.lineaflex.ru/good/118',
'http://www.lineaflex.ru/good/116',
'http://www.lineaflex.ru/good/122',
'http://www.lineaflex.ru/good/119',
'http://www.lineaflex.ru/good/120',
'http://www.lineaflex.ru/good/123',
'http://www.lineaflex.ru/good/117',
'http://www.lineaflex.ru/good/124',
'http://www.lineaflex.ru/good/41',
'http://www.lineaflex.ru/good/45',
'http://www.lineaflex.ru/good/49',
'http://www.lineaflex.ru/good/46',
'http://www.lineaflex.ru/good/43',
'http://www.lineaflex.ru/good/42',
'http://www.lineaflex.ru/good/44',
'http://www.lineaflex.ru/good/66',
'http://www.lineaflex.ru/good/48',
'http://www.lineaflex.ru/good/65',
'http://www.lineaflex.ru/good/52',
'http://www.lineaflex.ru/good/64',
'http://www.lineaflex.ru/good/50',
'http://www.lineaflex.ru/good/51',
'http://www.lineaflex.ru/good/62',
'http://www.lineaflex.ru/good/163',
'http://www.lineaflex.ru/good/161',
'http://www.lineaflex.ru/good/162',
'http://www.lineaflex.ru/good/165',
'http://www.lineaflex.ru/good/164',
'http://www.lineaflex.ru/good/166',
'http://www.lineaflex.ru/good/167', */
'http://www.lineaflex.ru/good/53',
'http://www.lineaflex.ru/good/54',
'http://www.lineaflex.ru/good/55',
'http://www.lineaflex.ru/good/56',
'http://www.lineaflex.ru/good/57',
'http://www.lineaflex.ru/good/58',
'http://www.lineaflex.ru/good/59',
'http://www.lineaflex.ru/good/60'
		];

$file = 'test.csv';
file_put_contents ($file, 'id@title@content@regular price@height-product@load@rigidity-product@set-of-springs@size-product@image@categories' . "\n", FILE_APPEND);

foreach ($urls as $url) {
	$page = file_get_html($url);
	$page = iconv('CP1251','UTF-8',$page);
	$page = str_get_html($page);

	
	$id = substr($url, strrpos ($url, '/') + 1);
	
	$title = $page->find('title',0)->plaintext;
	$title = substr ($title, strpos ($title, ' ') + 1);

	$content = '';
	foreach ($page->find('div[id=description]') as $p)
		$content .= $p;
	$buf = '';
	if ($page->find('div[id=features] table tbody tr td table tbody'))
		foreach ($page->find('div[id=features] table tbody tr td table tbody') as $tr) {
			foreach ($tr->find('td img') as $img)
				$img->setAttribute('src', 'http://www.lineaflex.ru' . $img->getAttribute('src'));
			$buf .= $tr;
			}
	else
		foreach ($page->find('div[id=features] table tbody') as $tr) {
			foreach ($tr->find('td img') as $img)
				$img->setAttribute('src', 'http://www.lineaflex.ru' . $img->getAttribute('src'));
			$buf .= $tr;
			}
	$content .= $buf;

	$size_price = '';
	foreach ($page->find('select[class=field w-100 size mr-16] option') as $option)
		$size_price[] = array ($option->plaintext, $option->getAttribute('data-price'));

	$height = $page->find('div[id=features] p', 0)->plaintext;
	$height = substr ($height, strpos ($height, ': ') + 2);

	$springs = $page->find('div[id=features] p', 1)->plaintext;
	$springs = substr ($springs, strpos ($springs, ': ') + 2);

	$load = $page->find('div[id=features] p', 2)->plaintext;
	$load = substr ($load, strpos ($load, ': ') + 2);

	$rigidity = $page->find('div[id=features] p', 3)->plaintext;
	$rigidity = substr ($rigidity, strpos ($rigidity, ': ') + 2);

	$image = '';
	if (count($page->find('ul[class=s list js-list] li a'))) {
		foreach ($page->find('ul[class=s list js-list] li a') as $li)
			$image .= '|http://www.lineaflex.ru' . $li->getAttribute('data-big-img');
		$image = substr ($image, 1);
		}
	else
		$image = 'http://www.lineaflex.ru' . $page->find('a[class=link jqzoom-tall]', 0)->getAttribute('href');

	$categories = $page->find('ul[class=s crumbs] li',2)->plaintext . '>' . $page->find('ul[class=s crumbs] li',3)->plaintext;

	file_put_contents ($file, $id . '@' . $title . '@' . $content . '@' . '' . '@' .  $height . '@' . $load . '@' . $rigidity . '@' . $springs . '@' . '' . '@' . $image . '@' . $categories . "\n", FILE_APPEND);

	foreach ($size_price as $key => $i)
		if ($key != 0)
			file_put_contents ($file, $id . '@' . $title . '@' . '' . '@' . $i[1] . '@' . '' . '@' .  '' . '@' . '' . '@' . '' . '@' . $i[0] . '@' . '' . '@' . '' . "\n", FILE_APPEND);
		

	$page->clear();
	unset($page);
	}


?>

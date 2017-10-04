<?php
require_once 'simple_html_dom.php';

$css_table = '<style type="text/css"> table, tr, td, th { border: none !important; } </style>';

$urls = [
		'http://www.lineaflex.ru/good/92',
		'http://www.lineaflex.ru/good/130',
		'http://www.lineaflex.ru/good/131',
		'http://www.lineaflex.ru/good/97',
		'http://www.lineaflex.ru/good/126',
		'http://www.lineaflex.ru/good/134',
		'http://www.lineaflex.ru/good/96',
		'http://www.lineaflex.ru/good/95',
		'http://www.lineaflex.ru/good/133',
		'http://www.lineaflex.ru/good/132',
		'http://www.lineaflex.ru/good/94',
		'http://www.lineaflex.ru/good/93',
		'http://www.lineaflex.ru/good/135'
		];

$file = 'test.csv';
file_put_contents ($file, 'id@title@content@regular price@height-product@load@rigidity-product@set-of-springs@size-product@image@categories' . "\n", FILE_APPEND);

foreach ($urls as $url) {
	$page = file_get_html($url);
	$page = iconv('CP1251','UTF-8',$page);
	$page = str_get_html($page);

	
	$id = substr($url, strrpos ($url, '/') + 1);

	$title = $page->find('title',0)->plaintext;
	if (strpos ($title, '|'))
		$title = ltrim (substr ($title, strpos ($title, '|') + 1));
	else
		$title = substr ($title, strpos ($title, ' ') + 1);

	$content = '';
	foreach ($page->find('div[id=description]') as $p)
		$content .= $p;
	$buf = '';
	foreach ($page->find('div[id=features]') as $div) {
		foreach ($div->find('li') as &$li)
			$li->outertext = '';
		foreach ($div->find('p[class=mb-3]') as &$p)
			$p->outertext = '';
		foreach ($div->find('p[class=mb-16]') as &$p)
			$p->outertext = '';
		foreach ($div->find('td img') as $img)
			if (!(strstr ($img->getAttribute('src'), 'lineaflex')))
				$img->setAttribute('src', 'http://www.lineaflex.ru' . $img->getAttribute('src'));
		$buf .= $div->innertext;
		}
	$content .= $buf;
	$content .= $css_table;
//	echo $content;
//	file_put_contents ($file, $content);



	$size_price = '';
	foreach ($page->find('select[class=field w-100 size mr-16] option') as $option)
		$size_price[] = array ($option->plaintext, $option->getAttribute('data-price'));

	$height = $page->find('div[id=features] p', 0)->plaintext;
	$height = substr ($height, strpos ($height, ': ') + 2);
	if (strpos ($height, ' см'))
		$height = ltrim (substr ($height, 0, strpos ($height, ' см')));

	$springs = '';
	if ($page->find('div[id=features] p', 1)) {
		$springs = $page->find('div[id=features] p', 1)->plaintext;
		$springs = substr ($springs, strpos ($springs, ': ') + 2);
		}
		
	$load = '';
	if ($page->find('div[id=features] p', 2)) {
		$load = $page->find('div[id=features] p', 2)->plaintext;
		$load = substr ($load, strpos ($load, ': ') + 2);
		}

	$rigidity = '';
	if ($page->find('div[id=features] p', 3)) {
		$rigidity = $page->find('div[id=features] p', 3)->plaintext;
		$rigidity = substr ($rigidity, strpos ($rigidity, ': ') + 2);
		}

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

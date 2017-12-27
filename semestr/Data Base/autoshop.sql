-- phpMyAdmin SQL Dump
-- version 4.5.2
-- http://www.phpmyadmin.net
--
-- Хост: localhost
-- Время создания: Дек 27 2017 г., 12:25
-- Версия сервера: 10.1.16-MariaDB
-- Версия PHP: 5.6.24

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База данных: `autoshop`
--

-- --------------------------------------------------------

--
-- Структура таблицы `s_completed_work`
--

CREATE TABLE `s_completed_work` (
  `n_id_completed_work` int(10) NOT NULL,
  `n_id_workman` int(10) DEFAULT NULL,
  `n_id_product` int(10) DEFAULT NULL,
  `d_date_completion` datetime NOT NULL,
  `n_product_count` int(10) NOT NULL,
  `n_id_type_completed_work` int(10) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `s_completed_work`
--

INSERT INTO `s_completed_work` (`n_id_completed_work`, `n_id_workman`, `n_id_product`, `d_date_completion`, `n_product_count`, `n_id_type_completed_work`) VALUES
(1, 3, 1, '2011-11-14 00:00:00', 13, 3),
(2, 4, 1, '2011-11-14 00:00:00', 3, 2),
(3, 5, 4, '2011-11-14 00:00:00', 30, 3),
(4, 6, 4, '2011-11-14 00:00:00', 11, 1),
(5, 6, 4, '2017-07-07 00:00:00', 77, 1),
(7, 5, 1, '2017-07-07 00:00:00', 77, 3),
(8, 5, 1, '2017-07-07 00:00:00', 71, 2);

-- --------------------------------------------------------

--
-- Структура таблицы `s_journal_visit`
--

CREATE TABLE `s_journal_visit` (
  `d_journal_visit` datetime NOT NULL,
  `n_id_workman` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `s_journal_visit`
--

INSERT INTO `s_journal_visit` (`d_journal_visit`, `n_id_workman`) VALUES
('2011-11-14 00:00:00', 1),
('2011-11-14 00:00:00', 2),
('2011-11-14 00:00:00', 3),
('2011-11-14 00:00:00', 4),
('2011-11-14 00:00:00', 5),
('2011-11-14 00:00:00', 6),
('2017-01-01 00:00:00', 66),
('2017-05-11 00:00:00', 1);

-- --------------------------------------------------------

--
-- Структура таблицы `s_office`
--

CREATE TABLE `s_office` (
  `n_id_office` int(10) NOT NULL,
  `c_address_office` varchar(150) CHARACTER SET utf8 NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Дамп данных таблицы `s_office`
--

INSERT INTO `s_office` (`n_id_office`, `c_address_office`) VALUES
(1, 'г.Москва, ул.Пушкина, д.214'),
(2, 'г.Москва, ул.Ленина, д.124');

-- --------------------------------------------------------

--
-- Структура таблицы `s_post_workman`
--

CREATE TABLE `s_post_workman` (
  `n_id_post_workman` int(10) NOT NULL,
  `c_name_post_workman` varchar(50) CHARACTER SET utf8 NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Дамп данных таблицы `s_post_workman`
--

INSERT INTO `s_post_workman` (`n_id_post_workman`, `c_name_post_workman`) VALUES
(2, 'Грузчик'),
(3, 'Менеджер'),
(1, 'Продавец');

-- --------------------------------------------------------

--
-- Структура таблицы `s_product`
--

CREATE TABLE `s_product` (
  `n_id_product` int(10) NOT NULL,
  `c_name_product` varchar(100) NOT NULL,
  `n_product_count` int(10) DEFAULT NULL,
  `n_product_price` double(12,2) NOT NULL,
  `n_id_type_product` int(10) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `s_product`
--

INSERT INTO `s_product` (`n_id_product`, `c_name_product`, `n_product_count`, `n_product_price`, `n_id_type_product`) VALUES
(1, 'Чехол для руля', 6, 99.99, 2),
(2, 'Чехлы для сидений', 0, 2199.00, 2),
(3, 'Колёса круглые', 0, 10199.00, 1),
(4, 'моторное масло', 0, 6230.60, 1);

-- --------------------------------------------------------

--
-- Структура таблицы `s_solved_problems`
--

CREATE TABLE `s_solved_problems` (
  `n_id_solved_problems` int(10) NOT NULL,
  `c_description_solved_problems` varchar(400) NOT NULL,
  `n_id_workman` int(10) DEFAULT NULL,
  `d_date_completion` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `s_solved_problems`
--

INSERT INTO `s_solved_problems` (`n_id_solved_problems`, `c_description_solved_problems`, `n_id_workman`, `d_date_completion`) VALUES
(1, 'оптимизировал рабочий процесс', 1, '2011-11-14 00:00:00'),
(2, 'улучшил график продаж', 2, '2011-11-14 00:00:00'),
(3, 'бла-бла-бла', 1, '2017-07-07 00:00:00');

-- --------------------------------------------------------

--
-- Структура таблицы `s_type_completed_work`
--

CREATE TABLE `s_type_completed_work` (
  `n_id_type_completed_work` int(10) NOT NULL,
  `c_name_completed_work` varchar(100) NOT NULL,
  `n_id_post_workman` int(10) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `s_type_completed_work`
--

INSERT INTO `s_type_completed_work` (`n_id_type_completed_work`, `c_name_completed_work`, `n_id_post_workman`) VALUES
(1, 'Продано (самовызов)', 1),
(2, 'Продано (доставка)', 1),
(3, 'Отгружено', 2);

-- --------------------------------------------------------

--
-- Структура таблицы `s_type_product`
--

CREATE TABLE `s_type_product` (
  `n_id_type_product` int(10) NOT NULL,
  `c_name_type_product` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `s_type_product`
--

INSERT INTO `s_type_product` (`n_id_type_product`, `c_name_type_product`) VALUES
(11, 'мыло'),
(2, 'Не расходный материал'),
(1, 'Расходный материал');

-- --------------------------------------------------------

--
-- Структура таблицы `s_workman`
--

CREATE TABLE `s_workman` (
  `n_id_workman` int(10) NOT NULL,
  `c_first_name_workman` varchar(30) CHARACTER SET utf8 NOT NULL,
  `c_second_name_workman` varchar(30) CHARACTER SET utf8 NOT NULL,
  `c_last_name_workman` varchar(30) CHARACTER SET utf8 DEFAULT NULL,
  `n_id_post_workman` int(10) DEFAULT NULL,
  `n_id_chief` int(10) DEFAULT NULL,
  `n_id_office` int(10) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Дамп данных таблицы `s_workman`
--

INSERT INTO `s_workman` (`n_id_workman`, `c_first_name_workman`, `c_second_name_workman`, `c_last_name_workman`, `n_id_post_workman`, `n_id_chief`, `n_id_office`) VALUES
(1, 'пётр', 'петров', 'петросян', 3, NULL, 2),
(2, 'сергей', 'есенин', 'андреевич', 3, NULL, 1),
(3, 'иван', 'иванов', 'иванеско', 2, 1, 2),
(4, 'максим', 'вадимов', 'алексеевич', 1, 1, 2),
(5, 'дмитрий', 'тумба', 'юмба', 2, 2, 1),
(6, 'влад', 'дракула', NULL, 1, 2, 1);

-- --------------------------------------------------------

--
-- Структура таблицы `test`
--

CREATE TABLE `test` (
  `name` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Дамп данных таблицы `test`
--

INSERT INTO `test` (`name`) VALUES
(1),
(777),
(22),
(22),
(222),
(222),
(999);

-- --------------------------------------------------------

--
-- Структура таблицы `users`
--

CREATE TABLE `users` (
  `login` varchar(128) NOT NULL,
  `password` varchar(256) NOT NULL,
  `role` varchar(256) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `users`
--

INSERT INTO `users` (`login`, `password`, `role`) VALUES
('admin', '12345', 'admin');

--
-- Индексы сохранённых таблиц
--

--
-- Индексы таблицы `s_completed_work`
--
ALTER TABLE `s_completed_work`
  ADD PRIMARY KEY (`n_id_completed_work`);

--
-- Индексы таблицы `s_journal_visit`
--
ALTER TABLE `s_journal_visit`
  ADD PRIMARY KEY (`d_journal_visit`,`n_id_workman`);

--
-- Индексы таблицы `s_office`
--
ALTER TABLE `s_office`
  ADD PRIMARY KEY (`n_id_office`);

--
-- Индексы таблицы `s_post_workman`
--
ALTER TABLE `s_post_workman`
  ADD PRIMARY KEY (`n_id_post_workman`),
  ADD UNIQUE KEY `c_name_post_workman` (`c_name_post_workman`);

--
-- Индексы таблицы `s_product`
--
ALTER TABLE `s_product`
  ADD PRIMARY KEY (`n_id_product`),
  ADD UNIQUE KEY `c_name_product` (`c_name_product`);

--
-- Индексы таблицы `s_solved_problems`
--
ALTER TABLE `s_solved_problems`
  ADD PRIMARY KEY (`n_id_solved_problems`);

--
-- Индексы таблицы `s_type_completed_work`
--
ALTER TABLE `s_type_completed_work`
  ADD PRIMARY KEY (`n_id_type_completed_work`),
  ADD UNIQUE KEY `c_name_completed_work` (`c_name_completed_work`);

--
-- Индексы таблицы `s_type_product`
--
ALTER TABLE `s_type_product`
  ADD PRIMARY KEY (`n_id_type_product`),
  ADD UNIQUE KEY `c_name_type_product` (`c_name_type_product`);

--
-- Индексы таблицы `s_workman`
--
ALTER TABLE `s_workman`
  ADD PRIMARY KEY (`n_id_workman`);

--
-- Индексы таблицы `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`login`);

--
-- AUTO_INCREMENT для сохранённых таблиц
--

--
-- AUTO_INCREMENT для таблицы `s_completed_work`
--
ALTER TABLE `s_completed_work`
  MODIFY `n_id_completed_work` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;
--
-- AUTO_INCREMENT для таблицы `s_office`
--
ALTER TABLE `s_office`
  MODIFY `n_id_office` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
--
-- AUTO_INCREMENT для таблицы `s_post_workman`
--
ALTER TABLE `s_post_workman`
  MODIFY `n_id_post_workman` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
--
-- AUTO_INCREMENT для таблицы `s_product`
--
ALTER TABLE `s_product`
  MODIFY `n_id_product` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;
--
-- AUTO_INCREMENT для таблицы `s_solved_problems`
--
ALTER TABLE `s_solved_problems`
  MODIFY `n_id_solved_problems` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
--
-- AUTO_INCREMENT для таблицы `s_type_completed_work`
--
ALTER TABLE `s_type_completed_work`
  MODIFY `n_id_type_completed_work` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
--
-- AUTO_INCREMENT для таблицы `s_type_product`
--
ALTER TABLE `s_type_product`
  MODIFY `n_id_type_product` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;
--
-- AUTO_INCREMENT для таблицы `s_workman`
--
ALTER TABLE `s_workman`
  MODIFY `n_id_workman` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

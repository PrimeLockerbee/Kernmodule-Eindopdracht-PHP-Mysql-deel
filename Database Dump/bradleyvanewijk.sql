-- phpMyAdmin SQL Dump
-- version 4.9.4
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Gegenereerd op: 12 aug 2023 om 12:05
-- Serverversie: 10.6.12-MariaDB
-- PHP-versie: 7.4.6

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `bradleyvanewijk`
--

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `scores`
--

CREATE TABLE `scores` (
  `id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `score` int(11) NOT NULL,
  `server_id` int(11) DEFAULT NULL,
  `game_date` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Gegevens worden geëxporteerd voor tabel `scores`
--

INSERT INTO `scores` (`id`, `user_id`, `score`, `server_id`, `game_date`) VALUES
(10, 1, 500, 1, '2023-08-01'),
(11, 2, 150, 1, '2023-08-01'),
(12, 1, 500, 1, '2023-08-02'),
(13, 3, 180, 1, '2023-08-02'),
(14, 2, 160, 1, '2023-08-03'),
(15, 1, 500, 1, '2023-08-03'),
(16, 0, 190, 1, '2023-02-01'),
(17, 2, 175, 1, '2023-04-05'),
(18, 4, 115, 1, '2023-08-02'),
(21, 16, 666, 1, '2023-08-12'),
(22, 67, 666, 1, '2023-08-12'),
(23, 68, 666, 1, '2023-08-12');

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `servers`
--

CREATE TABLE `servers` (
  `server_id` int(11) NOT NULL,
  `password` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Gegevens worden geëxporteerd voor tabel `servers`
--

INSERT INTO `servers` (`server_id`, `password`) VALUES
(1, 'qwerty123');

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `statistics`
--

CREATE TABLE `statistics` (
  `id` int(11) NOT NULL,
  `winner_id` int(11) NOT NULL,
  `win_count` int(11) DEFAULT 0,
  `game_date` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `users`
--

CREATE TABLE `users` (
  `id` int(99) NOT NULL,
  `email` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `nickname` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Gegevens worden geëxporteerd voor tabel `users`
--

INSERT INTO `users` (`id`, `email`, `password`, `nickname`) VALUES
(0, 'newuser00@example.com', '$2y$10$DFM5gI/AzQGN.1dV4qrK6uRA6RcD3FsiVReqwlZJkvmKePH5z96Wu', 'NewNicknaam00'),
(1, 'newuser11@example.com', '$2y$10$09kJh1/aUBDd6n9CwkpBLOxZuZZWBYjmC.zgPuKp3zn9uj49SvBPO', 'NewNicknaam11'),
(2, 'newuser22@example.com', '$2y$10$7AuvIhlyZiEXOk38oOEakOlOHvysBjUPp6WSE/gFNnZi.HUzRXXf6', 'NewNicknaam22'),
(3, 'newuser33@example.com', '$2y$10$lR04R0zRrFWc4K/706fu3e9oSx9ytqPTvygvRZI7O/u2HiftOE1Mm', 'NewNicknaam33'),
(4, 'user4@example.com', '$2y$10$GXbF985g52SlMkP8PGroY.flNbq1KPC8XxGGAMUDhRVGX6N29T0um', 'User4'),
(10, 'newBroodjeDesem@example.com', '$2y$10$gPEzpFiP8CAubuZuncRIwe35LEnlMpRc1SpicXVK2X34Z6L59YUz2', 'Broodman'),
(16, 'test1@gmail.com', '$2y$10$M66GD5eqYf/CaOHUnwLOUuH0X7o7D18b84LgssN3IOdphY2d/PV3m', 'test1'),
(17, 'test2@gmail.com', '$2y$10$GzAeExAYSXijf6NUSUgkU.UDCFMqGm70iBNapDOSR.k8ST0Py0wtK', 'test2'),
(67, 'test1234@gmail.com', '$2y$10$vXW/gJKBAxqdTVNPEhlM1.XNMT3zzOv6OSklR60wRWAh2DQdkRZRG', 'Tester1234'),
(68, 'test66@example.com', '$2y$10$Ns2C1ENoSsVCGcAsHnAzhe2cAd7d5E59PTWOHTYt1rTEy1Snif3Ye', 'tester66');

--
-- Indexen voor geëxporteerde tabellen
--

--
-- Indexen voor tabel `scores`
--
ALTER TABLE `scores`
  ADD PRIMARY KEY (`id`),
  ADD KEY `user_id` (`user_id`);

--
-- Indexen voor tabel `servers`
--
ALTER TABLE `servers`
  ADD PRIMARY KEY (`server_id`);

--
-- Indexen voor tabel `statistics`
--
ALTER TABLE `statistics`
  ADD PRIMARY KEY (`id`),
  ADD KEY `winner_id` (`winner_id`);

--
-- Indexen voor tabel `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT voor geëxporteerde tabellen
--

--
-- AUTO_INCREMENT voor een tabel `scores`
--
ALTER TABLE `scores`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=24;

--
-- AUTO_INCREMENT voor een tabel `servers`
--
ALTER TABLE `servers`
  MODIFY `server_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT voor een tabel `statistics`
--
ALTER TABLE `statistics`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT voor een tabel `users`
--
ALTER TABLE `users`
  MODIFY `id` int(99) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=69;

--
-- Beperkingen voor geëxporteerde tabellen
--

--
-- Beperkingen voor tabel `scores`
--
ALTER TABLE `scores`
  ADD CONSTRAINT `scores_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`);

--
-- Beperkingen voor tabel `statistics`
--
ALTER TABLE `statistics`
  ADD CONSTRAINT `statistics_ibfk_1` FOREIGN KEY (`winner_id`) REFERENCES `users` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

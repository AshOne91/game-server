CREATE DATABASE  IF NOT EXISTS `logdb2` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `logdb2`;
-- MySQL dump 10.13  Distrib 8.0.29, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: logdb2
-- ------------------------------------------------------
-- Server version	8.0.29

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `table_errorlog`
--

DROP TABLE IF EXISTS `table_errorlog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `table_errorlog` (
  `idx` int NOT NULL,
  `procedure_name` varchar(45) DEFAULT NULL,
  `error_state` varchar(10) DEFAULT NULL,
  `error_no` varchar(10) DEFAULT NULL,
  `error_message` varchar(128) DEFAULT NULL,
  `create_time` datetime DEFAULT CURRENT_TIMESTAMP,
  `param` varchar(4000) NOT NULL,
  PRIMARY KEY (`idx`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `table_errorlog`
--

LOCK TABLES `table_errorlog` WRITE;
/*!40000 ALTER TABLE `table_errorlog` DISABLE KEYS */;
/*!40000 ALTER TABLE `table_errorlog` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'logdb2'
--

--
-- Dumping routines for database 'logdb2'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-07-03 21:10:40
CREATE DATABASE  IF NOT EXISTS `gamedb2` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `gamedb2`;
-- MySQL dump 10.13  Distrib 8.0.29, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: gamedb2
-- ------------------------------------------------------
-- Server version	8.0.29

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `table_auto_guser`
--

DROP TABLE IF EXISTS `table_auto_guser`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `table_auto_guser` (
  `idx` bigint NOT NULL AUTO_INCREMENT,
  `user_db_key` bigint NOT NULL,
  `newbie` bit(1) NOT NULL DEFAULT b'1',
  `encode_account_id` varchar(100) NOT NULL,
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `update_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `gm_level` tinyint NOT NULL DEFAULT '0',
  PRIMARY KEY (`idx`),
  UNIQUE KEY `ix_guser_userdbkey` (`user_db_key`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `table_auto_guser`
--

LOCK TABLES `table_auto_guser` WRITE;
/*!40000 ALTER TABLE `table_auto_guser` DISABLE KEYS */;
/*!40000 ALTER TABLE `table_auto_guser` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `table_auto_player`
--

DROP TABLE IF EXISTS `table_auto_player`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `table_auto_player` (
  `idx` bigint unsigned NOT NULL AUTO_INCREMENT,
  `player_db_key` bigint unsigned NOT NULL,
  `user_db_key` bigint unsigned NOT NULL,
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `update_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `login_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `logout_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `is_login` bit(1) NOT NULL DEFAULT b'0',
  `newbie` bit(1) NOT NULL DEFAULT b'1',
  `serial_allocator` bigint NOT NULL DEFAULT '0',
  `player_name` varchar(50) NOT NULL,
  `level` smallint NOT NULL DEFAULT '1',
  `exp` bigint NOT NULL DEFAULT '0',
  PRIMARY KEY (`idx`),
  UNIQUE KEY `ix_player_playerdbkey` (`player_db_key`) /*!80000 INVISIBLE */,
  UNIQUE KEY `ix_player_playername` (`player_name`) /*!80000 INVISIBLE */,
  KEY `ix_player_islogin` (`is_login`) /*!80000 INVISIBLE */,
  KEY `ix_player_level` (`level`) /*!80000 INVISIBLE */,
  KEY `ix_player_logintime` (`login_time`) /*!80000 INVISIBLE */,
  KEY `ix_player_userdbkey` (`user_db_key`) /*!80000 INVISIBLE */
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `table_auto_player`
--

LOCK TABLES `table_auto_player` WRITE;
/*!40000 ALTER TABLE `table_auto_player` DISABLE KEYS */;
/*!40000 ALTER TABLE `table_auto_player` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `table_errorlog`
--

DROP TABLE IF EXISTS `table_errorlog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `table_errorlog` (
  `idx` int NOT NULL,
  `procedure_name` varchar(45) DEFAULT NULL,
  `error_state` varchar(10) DEFAULT NULL,
  `error_no` varchar(10) DEFAULT NULL,
  `error_message` varchar(128) DEFAULT NULL,
  `create_time` datetime DEFAULT CURRENT_TIMESTAMP,
  `param` varchar(4000) NOT NULL,
  PRIMARY KEY (`idx`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `table_errorlog`
--

LOCK TABLES `table_errorlog` WRITE;
/*!40000 ALTER TABLE `table_errorlog` DISABLE KEYS */;
/*!40000 ALTER TABLE `table_errorlog` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'gamedb2'
--

--
-- Dumping routines for database 'gamedb2'
--
/*!50003 DROP PROCEDURE IF EXISTS `gp_user_player_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`ubf`@`%` PROCEDURE `gp_user_player_create`(
	IN p_user_db_key BIGINT UNSIGNED,
    IN p_max_player_count TINYINT UNSIGNED,
    IN p_player_db_key BIGINT UNSIGNED,
    IN p_player_name varchar(50),
    IN p_player_level SMALLINT
)
BEGIN
	DECLARE ProcParam varchar(4000);
	DECLARE EXIT HANDLER FOR SQLEXCEPTION
	BEGIN
		GET DIAGNOSTICS @cno = NUMBER;
		GET DIAGNOSTICS CONDITION @cno
		@p_ErrorState = RETURNED_SQLSTATE, @p_ErrorNo = MYSQL_ERRNO, @p_ErrorMessage = MESSAGE_TEXT;
		SET ProcParam = CONCAT(p_user_db_key, ',', p_max_player_count, ',', p_player_db_key, ',',  p_player_name, ',', p_player_level);
		ROLLBACK;
		INSERT INTO table_errorlog (procedure_name, error_state, error_no, error_message, param)
			VALUES ('gp_user_player_create', @p_ErrorState, @p_ErrorNo, @p_ErrorMessage, ProcParam);
		RESIGNAL;
	END;
		
        #플레이어 최대 생성 수 체크
        IF (SELECT
					COUNT(*)
				FROM table_auto_player
                WHERE user_db_key = p_user_db_key) >= p_max_player_count THEN
			SELECT
				1 AS result;
		# 플레이어 이름 중복 체크
        ELSEIF EXISTS (SELECT
					1
				FROM table_auto_player
                WHERE player_name = p_player_name LIMIT 1) THEN
			SELECT
				2 AS result;
		ELSEIF EXISTS (SELECT
					1
				FROM table_auto_player
                WHERE player_db_key = p_player_db_key LIMIT 1) THEN
			SELECT
				3 AS result;
		ELSE 
			INSERT INTO table_auto_player (user_db_key,
            player_db_key,
            player_name,
            level)
				VALUES (p_user_db_key, p_player_db_key, p_player_name, p_player_level);
                
			SELECT
				0 AS result,
                player_db_key,
                player_name,
                level
			FROM table_auto_player
            WHERE player_db_key = p_player_db_key;
		END IF;        
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `gp_user_player_list_load` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`ubf`@`%` PROCEDURE `gp_user_player_list_load`(
	IN p_user_db_key BIGINT UNSIGNED,
	IN p_encode_account_id varchar(50),
    IN p_gm_level TINYINT
)
BEGIN
	DECLARE ProcParam varchar(4000);
	DECLARE EXIT HANDLER FOR SQLEXCEPTION
	BEGIN
		GET DIAGNOSTICS @cno = NUMBER;
		GET DIAGNOSTICS CONDITION @cno
		@p_ErrorState = RETURNED_SQLSTATE, @p_ErrorNo = MYSQL_ERRNO, @p_ErrorMessage = MESSAGE_TEXT;
		SET ProcParam = CONCAT(p_user_db_key, ',', p_encode_account_id, ',', p_gm_level);
		INSERT INTO table_errorlog (procedure_name, error_state, error_no, error_message, create_time)
			VALUES ('gp_user_player_list_load', @p_ErrorState, @p_ErrorNo, @p_ErrorMessage, ProcParam);
		RESIGNAL;
	END;
    
    IF NOT EXISTS (SELECT
				user_db_key
			FROM table_auto_guser
            WHERE user_db_key = p_user_db_key) THEN
		INSERT INTO table_auto_guser (user_db_key, encode_account_id, gm_level)
			VALUES (p_user_db_key, p_encode_account_id, p_gm_level);
	END IF;
    
    SELECT
		player_db_key,
        player_name,
        create_time,
        logout_time,
        level,
        exp
	FROM table_auto_player
    WHERE user_db_key = p_user_db_key;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-07-03 21:10:40
CREATE DATABASE  IF NOT EXISTS `logdb1` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `logdb1`;
-- MySQL dump 10.13  Distrib 8.0.29, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: logdb1
-- ------------------------------------------------------
-- Server version	8.0.29

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `table_errorlog`
--

DROP TABLE IF EXISTS `table_errorlog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `table_errorlog` (
  `idx` int NOT NULL,
  `procedure_name` varchar(45) DEFAULT NULL,
  `error_state` varchar(10) DEFAULT NULL,
  `error_no` varchar(10) DEFAULT NULL,
  `error_message` varchar(128) DEFAULT NULL,
  `create_time` datetime DEFAULT CURRENT_TIMESTAMP,
  `param` varchar(4000) NOT NULL,
  PRIMARY KEY (`idx`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `table_errorlog`
--

LOCK TABLES `table_errorlog` WRITE;
/*!40000 ALTER TABLE `table_errorlog` DISABLE KEYS */;
/*!40000 ALTER TABLE `table_errorlog` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'logdb1'
--

--
-- Dumping routines for database 'logdb1'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-07-03 21:10:40
CREATE DATABASE  IF NOT EXISTS `globaldb` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `globaldb`;
-- MySQL dump 10.13  Distrib 8.0.29, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: globaldb
-- ------------------------------------------------------
-- Server version	8.0.29

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `table_accountid`
--

DROP TABLE IF EXISTS `table_accountid`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `table_accountid` (
  `idx` bigint unsigned NOT NULL AUTO_INCREMENT,
  `platform_type` tinyint NOT NULL,
  `account_id` varchar(100) NOT NULL,
  `account_db_key` bigint unsigned NOT NULL DEFAULT '0',
  `account_status` varchar(15) NOT NULL DEFAULT 'Normal',
  `block_endtime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `is_withdraw` bit(1) NOT NULL DEFAULT b'0',
  `withdraw_cancel_count` tinyint NOT NULL DEFAULT '0',
  `withdraw_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `login_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `logout_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `gamedb_idx` tinyint NOT NULL,
  `logdb_idx` tinyint NOT NULL,
  PRIMARY KEY (`idx`),
  UNIQUE KEY `ix_accountid_plaform_accountid` (`platform_type`,`account_id`) /*!80000 INVISIBLE */,
  KEY `ix_accountid_accountdbkey` (`account_db_key`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `table_accountid`
--

LOCK TABLES `table_accountid` WRITE;
/*!40000 ALTER TABLE `table_accountid` DISABLE KEYS */;
INSERT INTO `table_accountid` VALUES (1,1,'test2',1,'Normal','2022-06-28 17:13:18',_binary '\0',0,'2022-06-28 17:13:18','2022-06-28 17:13:18','2022-06-28 17:13:18','2022-06-28 17:13:18',0,0);
/*!40000 ALTER TABLE `table_accountid` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `table_aplayer`
--

DROP TABLE IF EXISTS `table_aplayer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `table_aplayer` (
  `idx` bigint unsigned NOT NULL AUTO_INCREMENT,
  `account_db_key` bigint unsigned NOT NULL,
  `user_db_key` bigint unsigned NOT NULL,
  `player_db_key` bigint NOT NULL,
  `server_id` int NOT NULL,
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `login_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `logout_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `player_name` varchar(50) NOT NULL DEFAULT '',
  `player_level` smallint NOT NULL DEFAULT '1',
  `player_class_type` tinyint NOT NULL DEFAULT '0',
  PRIMARY KEY (`idx`),
  UNIQUE KEY `uk_table_aplayer_playername` (`player_db_key`),
  UNIQUE KEY `uk_table_aplayer_playerdbkey` (`player_name`),
  KEY `ix_aplayer_accountdbkey` (`account_db_key`) /*!80000 INVISIBLE */,
  KEY `ix_aplayer_userdbkey` (`user_db_key`) /*!80000 INVISIBLE */
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `table_aplayer`
--

LOCK TABLES `table_aplayer` WRITE;
/*!40000 ALTER TABLE `table_aplayer` DISABLE KEYS */;
INSERT INTO `table_aplayer` VALUES (1,1,1,10001,1,'2022-07-01 13:44:51','2022-07-01 13:44:51','2022-07-01 13:44:51','11',1,0),(2,1,1,10002,1,'2022-07-01 13:45:09','2022-07-01 13:45:09','2022-07-01 13:45:09','111',1,0);
/*!40000 ALTER TABLE `table_aplayer` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `table_auser`
--

DROP TABLE IF EXISTS `table_auser`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `table_auser` (
  `user_db_key` bigint unsigned NOT NULL AUTO_INCREMENT,
  `account_db_key` bigint unsigned NOT NULL,
  `server_id` int NOT NULL,
  `gamedb_idx` smallint NOT NULL,
  `logdb_idx` smallint NOT NULL,
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `login_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `logout_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `block_status` smallint NOT NULL DEFAULT '0',
  `block_end_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `serial_allocator` smallint NOT NULL DEFAULT '0',
  PRIMARY KEY (`user_db_key`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `table_auser`
--

LOCK TABLES `table_auser` WRITE;
/*!40000 ALTER TABLE `table_auser` DISABLE KEYS */;
INSERT INTO `table_auser` VALUES (1,1,1,2,4,'2022-06-29 16:03:48','2022-06-29 16:03:48','2022-06-29 16:03:48',0,'2022-06-29 16:03:48',2),(2,2,2,1,1,'2022-06-29 16:03:58','2022-06-29 16:03:58','2022-06-29 16:03:58',0,'2022-06-29 16:03:58',0),(3,3,3,1,1,'2022-06-29 16:06:09','2022-06-29 16:06:09','2022-06-29 16:06:09',0,'2022-06-29 16:06:09',0),(4,9,1,2,3,'2022-06-29 16:07:36','2022-06-29 16:07:36','2022-06-29 16:07:36',0,'2022-06-29 16:07:36',0);
/*!40000 ALTER TABLE `table_auser` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `table_db_list`
--

DROP TABLE IF EXISTS `table_db_list`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `table_db_list` (
  `idx` tinyint NOT NULL,
  `server_id` int NOT NULL,
  `type` tinyint unsigned NOT NULL,
  `sharding_key` int NOT NULL,
  `name` varchar(30) NOT NULL,
  `id` varchar(30) NOT NULL,
  `pw` varchar(30) NOT NULL,
  `ip` varchar(30) NOT NULL,
  `slave_ip` varchar(30) NOT NULL,
  `port` smallint unsigned NOT NULL,
  `user_count` int NOT NULL DEFAULT '0',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`idx`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `table_db_list`
--

LOCK TABLES `table_db_list` WRITE;
/*!40000 ALTER TABLE `table_db_list` DISABLE KEYS */;
INSERT INTO `table_db_list` VALUES (1,1,2,1,'gamedb1','ubf','qjxjvmffkdl!@#','127.0.0.1','127.0.0.1',3306,6,'2022-06-29 14:56:42'),(2,1,2,2,'gamedb2','ubf','qjxjvmffkdl!@#','127.0.0.1','127.0.0.1',3306,3,'2022-06-29 14:56:42'),(3,1,3,1,'logdb1','ubf','qjxjvmffkdl!@#','127.0.0.1','127.0.0.1',3306,3,'2022-06-29 14:56:42'),(4,1,3,2,'logdb2','ubf','qjxjvmffkdl!@#','127.0.0.1','127.0.0.1',3306,2,'2022-06-29 14:56:42');
/*!40000 ALTER TABLE `table_db_list` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `table_errorlog`
--

DROP TABLE IF EXISTS `table_errorlog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `table_errorlog` (
  `idx` int NOT NULL AUTO_INCREMENT,
  `procedure_name` varchar(45) DEFAULT NULL,
  `error_state` varchar(10) DEFAULT NULL,
  `error_no` varchar(10) DEFAULT NULL,
  `error_message` varchar(128) DEFAULT NULL,
  `create_time` datetime DEFAULT CURRENT_TIMESTAMP,
  `param` varchar(4000) NOT NULL,
  PRIMARY KEY (`idx`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `table_errorlog`
--

LOCK TABLES `table_errorlog` WRITE;
/*!40000 ALTER TABLE `table_errorlog` DISABLE KEYS */;
INSERT INTO `table_errorlog` VALUES (1,'gp_server_platform_auth',NULL,NULL,NULL,'2022-06-28 17:07:12','2,test'),(2,'gp_server_platform_auth',NULL,NULL,NULL,'2022-06-28 17:09:47','1,test77'),(3,'gp_server_get_user',NULL,NULL,NULL,'2022-06-29 15:57:18','1,1'),(4,'gp_server_get_user',NULL,NULL,NULL,'2022-06-29 16:01:36','1,1'),(5,'gp_server_get_user',NULL,NULL,NULL,'2022-06-29 16:03:03','1,1'),(6,'gp_aplayer_get_playerdbkey',NULL,NULL,NULL,'2022-07-01 13:36:50',''),(7,'gp_aplayer_get_playerdbkey',NULL,NULL,NULL,'2022-07-01 13:37:48',''),(8,'gp_aplayer_get_playerdbkey',NULL,NULL,NULL,'2022-07-01 13:44:07','');
/*!40000 ALTER TABLE `table_errorlog` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `table_gm_account`
--

DROP TABLE IF EXISTS `table_gm_account`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `table_gm_account` (
  `idx` bigint unsigned NOT NULL,
  `account_db_key` bigint unsigned NOT NULL,
  `gm_level` tinyint NOT NULL DEFAULT '0',
  PRIMARY KEY (`idx`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `table_gm_account`
--

LOCK TABLES `table_gm_account` WRITE;
/*!40000 ALTER TABLE `table_gm_account` DISABLE KEYS */;
/*!40000 ALTER TABLE `table_gm_account` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'globaldb'
--

--
-- Dumping routines for database 'globaldb'
--
/*!50003 DROP PROCEDURE IF EXISTS `gp_aplayer_get_playerdbkey` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`ubf`@`%` PROCEDURE `gp_aplayer_get_playerdbkey`(
	IN p_user_db_key BIGINT UNSIGNED,
    IN p_player_name VARCHAR(50),
    IN p_server_id INT
)
BEGIN

	DECLARE v_SerialAllocator SMALLINT;
    DECLARE v_AccountDBKey BIGINT;
    DECLARE v_PlayerDBKey BIGINT;
    
	DECLARE ProcParam varchar(4000);
	DECLARE EXIT HANDLER FOR SQLEXCEPTION
	BEGIN
		GET DIAGNOSTICS @cno = NUMBER;
		GET DIAGNOSTICS CONDITION @cno
		@p_ErrorState = RETURNED_SQLSTATE, @p_ErrorNo = MYSQL_ERRNO, @p_ErrorMessage = MESSAGE_TEXT;
		SET ProcParam = CONCAT(p_user_db_key, ',', p_player_name, ',', p_server_id);
		ROLLBACK;
		INSERT INTO table_errorlog (procedure_name, error_state, error_no, error_message, param)
			VALUES ('gp_aplayer_get_playerdbkey', @ErrorState, @ErrorNo, @ErrorMessage, param);
		RESIGNAL;
	END;
    
    SET v_PlayerDBKey = 0;
    
    SELECT
		player_db_key INTO v_PlayerDBKey
	FROM table_aplayer
    WHERE player_name = p_player_name;
    
    IF v_PlayerDBKey > 0 THEN
		SELECT
			0 AS player_db_key;
	ELSE 
    
		START TRANSACTION;
        
			SELECT
				account_db_key INTO v_AccountDBKey
			FROM table_auser
            WHERE user_db_key = p_user_db_key;
            
            UPDATE table_auser
            SET serial_allocator = serial_allocator + 1 
            WHERE user_db_key = p_user_db_key;
            
            SELECT
				serial_allocator INTO v_SerialAllocator
			FROM table_auser
            WHERE user_db_key = p_user_db_key;
            SET v_PlayerDBKey = (p_user_db_key * 10000 + v_SerialAllocator % 10000);
            
            INSERT INTO table_aplayer(account_db_key, user_db_key, player_db_key, player_name, server_id)
				VALUES (v_AccountDBKey, p_user_db_key, v_PlayerDBKey, p_player_name, p_server_id);
                
		COMMIT;
        
        SELECT
			v_PlayerDBKey AS player_db_key;
	END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `gp_auser_login` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`ubf`@`%` PROCEDURE `gp_auser_login`(
	IN p_platform_type INT,
    IN p_encode_account_id varchar(100),
    IN p_user_db_key BIGINT UNSIGNED
)
BEGIN

	DECLARE ProcParam varchar(4000);
	DECLARE EXIT HANDLER FOR SQLEXCEPTION
	BEGIN
		SET ProcParam = CONCAT(p_platform_type, ',', p_encode_account_id, ',', p_user_db_key);
		GET DIAGNOSTICS CONDITION 1 @ErrorState = RETURNED_SQLSTATE, @ErrorNo = MYSQL_ERRNO, @ErrorMessage = MESSAGE_TEXT;
		ROLLBACK;
		INSERT INTO table_errorlog (procedure_name, error_state, error_no, error_message, param)
			VALUES ('gp_auser_login', @ErrorState, @ErrorNo, @ErrorMessage, ProcParam);
		RESIGNAL;
	END;
    
    UPDATE table_accountid
    SET login_time = NOW()
	WHERE platform_type = p_platform_type
    AND account_id = p_encode_account_id;
    
    UPDATE table_accountid
    SET login_time  = NOW()
    WHERE user_db_key = p_user_db_key;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `gp_db_list_load` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`ubf`@`%` PROCEDURE `gp_db_list_load`()
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        DECLARE ProcParam varchar(4000);
        SET ProcParam =CONCAT(p_RealServerID);
        GET DIAGNOSTICS CONDITION 1 @ErrorState = RETURNED_SQLSTATE, @ErrorNo = MYSQL_ERRNO, @ErrorMessage = MESSAGE_TEXT;
        ROLLBACK;
        INSERT INTO table_errorlog(procedure_name, error_state, error_no, error_message, param) VALUES('gp_db_list_load', @ErrorState, @ErrorNo, @ErrorMessage, ProcParam);
        RESIGNAL; 
    END;
    
    SELECT * FROM table_db_list;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `gp_server_get_user` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`ubf`@`%` PROCEDURE `gp_server_get_user`(
	IN p_account_db_key BIGINT UNSIGNED,
    IN p_server_id INT
)
BEGIN
	DECLARE ProcParam varchar(4000);
    DECLARE v_user_db_key BIGINT UNSIGNED DEFAULT 0;
    DECLARE v_login_time DATETIME;
    DECLARE v_logout_time DATETIME;
    DECLARE v_gamedb_idx SMALLINT DEFAULT 0;
    DECLARE v_logdb_idx SMALLINT DEFAULT 0;

	DECLARE EXIT HANDLER FOR SQLEXCEPTION
	BEGIN
		SET ProcParam = CONCAT(p_account_db_key, ',', p_server_id);
		GET DIAGNOSTICS CONDITION 1 @ErrorState = RETURNED_SQLSTATE, @ErrorNo = MYSQL_ERRNO, @ErrorMessage = MESSAGE_TEXT;
		ROLLBACK;
		INSERT INTO table_errorlog (procedure_name, error_state, error_no, error_message, param)
			VALUES ('gp_server_get_user', @ErrorState, @ErrorNo, @ErrorMessage, ProcParam);
		RESIGNAL;
	END;
    
    # 생성된 유저가 존재하는지 찾는다.
    SELECT
			user_db_key INTO v_user_db_key
	FROM table_auser
    WHERE account_db_key = p_account_db_key
    AND server_id = p_server_id;
    
    IF (v_user_db_key = 0) THEN
		SET v_gamedb_idx = 1;
        SELECT
			idx INTO v_gamedb_idx
		FROM table_db_list
        WHERE server_id = p_server_id
        AND type = 2
        ORDER BY user_count ASC LIMIT 1;
        
        UPDATE table_db_list
        SET user_count = user_count + 1
        WHERE idx = v_gamedb_idx;
        
        SET v_logdb_idx = 1;
        SELECT
			idx INTO v_logdb_idx
		FROM table_db_list
        WHERE server_id = p_server_id
        AND type = 3
        ORDER BY user_count ASC LIMIT 1;
        
        UPDATE table_db_list
        SET user_count = user_count + 1
        WHERE idx = v_logdb_idx;
        
        INSERT INTO table_auser (account_db_key, server_id, gamedb_idx, logdb_idx)
			VALUES (p_account_db_key, p_server_id, v_gamedb_idx, v_logdb_idx);
            
		SET v_user_db_key = LAST_INSERT_ID();
	ELSE
		# 유저 제재 상태 갱신
        UPDATE table_auser
        SET block_status = CASE WHEN 
			block_status IN (2, 3) AND block_end_time <= NOW() THEN 0 ELSE block_status END
		WHERE user_db_key = v_user_db_key;
	END IF;
    # 최근 접속 서버 정보를 얻어온다.
    SELECT
		user_db_key,
        login_time,
        logout_time INTO v_user_db_key, v_login_time, v_logout_time
	FROM table_auser
    WHERE account_db_key = p_account_db_key
    ORDER BY login_time DESC LIMIT 1;
	
    SELECT
		user_db_key,
        gamedb_idx,
        logdb_idx,
        block_status,
        block_end_time,
        v_login_time AS recent_login_time,
        v_logout_time AS recent_logout_time,
        IFNULL(t2.gm_level, 0) AS gm_level
	FROM table_auser t1
		LEFT JOIN table_gm_account t2
			ON t2.account_db_key = t1.account_db_key
	WHERE user_db_key = v_user_db_key;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `gp_server_platform_auth` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`ubf`@`%` PROCEDURE `gp_server_platform_auth`(
	IN p_platform_type INT,
    IN p_site_user_id VARCHAR(100)
)
BEGIN
	DECLARE v_account_idx BIGINT UNSIGNED DEFAULT 0;
    DECLARE v_account_db_key BIGINT UNSIGNED DEFAULT 0;
    DECLARE v_encode_account_id VARCHAR(100);
    DECLARE v_gamedb_count INT DEFAULT 0;
    DECLARE v_logdb_count INT DEFAULT 0;
    DECLARE v_gamedb_idx INT DEFAULT 0;
    DECLARE v_logdb_idx INT DEFAULT 0;
    DECLARE v_is_google_link BIT DEFAULT 0;
    DECLARE v_is_apple_link BIT DEFAULT 0;
    DECLARE v_is_facebook_link BIT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        DECLARE ProcParam varchar(4000);
        SET ProcParam = CONCAT(p_platform_type,',',p_site_user_id);
        GET DIAGNOSTICS CONDITION 1 @ErrorState = RETURNED_SQLSTATE, @ErrorNo = MYSQL_ERRNO, @ErrorMessage = MESSAGE_TEXT;
        ROLLBACK;
        INSERT INTO table_errorlog(procedure_name, error_state, error_no, error_message, param) VALUES('gp_server_platform_auth', @ErrorState, @ErrorNo, @ErrorMessage, ProcParam);
        RESIGNAL; 
    END;
    
    START TRANSACTION;
    
    IF p_platform_type = 1 THEN
		IF (p_site_user_id = '') THEN 
			SET v_encode_account_id = REPLACE(UUID(), '-', '');
		ELSE
			SET v_encode_account_id = p_site_user_id;
		END IF;
	ELSEIF p_platform_type IN (2, 3, 4) THEN
		SET v_encode_account_id = p_site_user_id;
	END IF;
    
    SELECT idx, 
		account_db_key INTO v_account_idx, v_account_db_key 
        FROM table_accountid 
        WHERE platform_type = p_platform_type 
        AND account_id = v_encode_account_id;
    
    IF (v_account_idx = 0) THEN
		INSERT INTO table_accountid(platform_type,
		account_id)
        VALUES (p_platform_type, v_encode_account_id);
        -- 계정 생성시 AccountDBKey는 Idx와 일치 시키고, 다른 플랫폼과 연동시 연동되는 플랫폼의 AccountDBKey로 변경된다.
        SET v_account_idx = LAST_INSERT_ID();
        SET v_account_db_key = v_account_idx;
        UPDATE table_accountid 
        SET account_db_key = v_account_idx 
        WHERE idx = v_account_idx;
	ELSE
		UPDATE table_accountid
        SET account_status = CASE WHEN account_status IN ('PeroidBlock', 'TempBlock') AND 
			block_endtime <= NOW() THEN 'Normal' ELSE account_status END
        WHERE idx = v_account_idx;
	END IF;
    
    COMMIT;
    
    SELECT MAX(CASE WHEN platform_type = 2 THEN 1 ELSE 0 END),
		   MAX(CASE WHEN platform_type = 3 THEN 1 ELSE 0 END),
           MAX(CASE WHEN platform_type = 4 THEN 1 ELSE 0 END)
           INTO v_is_google_link, v_is_apple_link, v_is_facebook_link
    FROM table_accountid
    WHERE account_db_key = v_account_db_key
    GROUP BY account_db_key;
    
    SELECT
		v_account_db_key as account_db_key,
        v_encode_account_id as encode_account_id,
        account_status,
        block_endtime,
        is_withdraw,
        withdraw_time,
        withdraw_cancel_count,
        v_is_google_link AS is_google_link,
        v_is_apple_link AS is_apple_link,
        v_is_facebook_link AS is_facebook_link
	FROM table_accountid
    WHERE idx = v_account_idx;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-07-03 21:10:40
CREATE DATABASE  IF NOT EXISTS `gamedb1` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `gamedb1`;
-- MySQL dump 10.13  Distrib 8.0.29, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: gamedb1
-- ------------------------------------------------------
-- Server version	8.0.29

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `table_auto_guser`
--

DROP TABLE IF EXISTS `table_auto_guser`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `table_auto_guser` (
  `idx` bigint NOT NULL AUTO_INCREMENT,
  `user_db_key` bigint NOT NULL,
  `newbie` bit(1) NOT NULL DEFAULT b'1',
  `encode_account_id` varchar(100) NOT NULL,
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `update_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `gm_level` tinyint NOT NULL DEFAULT '0',
  PRIMARY KEY (`idx`),
  UNIQUE KEY `ix_guser_userdbkey` (`user_db_key`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `table_auto_guser`
--

LOCK TABLES `table_auto_guser` WRITE;
/*!40000 ALTER TABLE `table_auto_guser` DISABLE KEYS */;
INSERT INTO `table_auto_guser` VALUES (1,1,_binary '','11','2022-07-01 15:57:01','2022-07-01 15:57:01',0);
/*!40000 ALTER TABLE `table_auto_guser` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `table_auto_player`
--

DROP TABLE IF EXISTS `table_auto_player`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `table_auto_player` (
  `idx` bigint unsigned NOT NULL AUTO_INCREMENT,
  `player_db_key` bigint unsigned NOT NULL,
  `user_db_key` bigint unsigned NOT NULL,
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `update_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `login_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `logout_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `is_login` bit(1) NOT NULL DEFAULT b'0',
  `newbie` bit(1) NOT NULL DEFAULT b'1',
  `serial_allocator` bigint NOT NULL DEFAULT '0',
  `player_name` varchar(50) NOT NULL,
  `level` smallint NOT NULL DEFAULT '1',
  `exp` bigint NOT NULL DEFAULT '0',
  PRIMARY KEY (`idx`),
  UNIQUE KEY `ix_player_playerdbkey` (`player_db_key`) /*!80000 INVISIBLE */,
  UNIQUE KEY `ix_player_playername` (`player_name`) /*!80000 INVISIBLE */,
  KEY `ix_player_islogin` (`is_login`) /*!80000 INVISIBLE */,
  KEY `ix_player_level` (`level`) /*!80000 INVISIBLE */,
  KEY `ix_player_logintime` (`login_time`) /*!80000 INVISIBLE */,
  KEY `ix_player_userdbkey` (`user_db_key`) /*!80000 INVISIBLE */
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `table_auto_player`
--

LOCK TABLES `table_auto_player` WRITE;
/*!40000 ALTER TABLE `table_auto_player` DISABLE KEYS */;
INSERT INTO `table_auto_player` VALUES (1,1,1,'2022-07-01 15:16:23','2022-07-01 15:16:23','2022-07-01 15:16:23','2022-07-01 15:16:23',_binary '\0',_binary '',0,'1',1,0),(2,2,1,'2022-07-01 15:34:02','2022-07-01 15:34:02','2022-07-01 15:34:02','2022-07-01 15:34:02',_binary '\0',_binary '',0,'111',123,0);
/*!40000 ALTER TABLE `table_auto_player` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `table_errorlog`
--

DROP TABLE IF EXISTS `table_errorlog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `table_errorlog` (
  `idx` int NOT NULL AUTO_INCREMENT,
  `procedure_name` varchar(45) DEFAULT NULL,
  `error_state` varchar(10) DEFAULT NULL,
  `error_no` varchar(10) DEFAULT NULL,
  `error_message` varchar(128) DEFAULT NULL,
  `create_time` datetime DEFAULT CURRENT_TIMESTAMP,
  `param` varchar(4000) NOT NULL,
  PRIMARY KEY (`idx`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `table_errorlog`
--

LOCK TABLES `table_errorlog` WRITE;
/*!40000 ALTER TABLE `table_errorlog` DISABLE KEYS */;
INSERT INTO `table_errorlog` VALUES (1,'gp_user_player_create','21S01','1136','Column count doesn\'t match value count at row 1','2022-07-01 15:12:14','1,11,1,1,1');
/*!40000 ALTER TABLE `table_errorlog` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'gamedb1'
--

--
-- Dumping routines for database 'gamedb1'
--
/*!50003 DROP PROCEDURE IF EXISTS `gp_user_player_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`ubf`@`%` PROCEDURE `gp_user_player_create`(
	IN p_user_db_key BIGINT UNSIGNED,
    IN p_max_player_count TINYINT UNSIGNED,
    IN p_player_db_key BIGINT UNSIGNED,
    IN p_player_name varchar(50),
    IN p_player_level SMALLINT
)
BEGIN
	DECLARE ProcParam varchar(4000);
	DECLARE EXIT HANDLER FOR SQLEXCEPTION
	BEGIN
		GET DIAGNOSTICS @cno = NUMBER;
		GET DIAGNOSTICS CONDITION @cno
		@p_ErrorState = RETURNED_SQLSTATE, @p_ErrorNo = MYSQL_ERRNO, @p_ErrorMessage = MESSAGE_TEXT;
		SET ProcParam = CONCAT(p_user_db_key, ',', p_max_player_count, ',', p_player_db_key, ',',  p_player_name, ',', p_player_level);
		ROLLBACK;
		INSERT INTO table_errorlog (procedure_name, error_state, error_no, error_message, param)
			VALUES ('gp_user_player_create', @p_ErrorState, @p_ErrorNo, @p_ErrorMessage, ProcParam);
		RESIGNAL;
	END;
		
        #플레이어 최대 생성 수 체크
        IF (SELECT
					COUNT(*)
				FROM table_auto_player
                WHERE user_db_key = p_user_db_key) >= p_max_player_count THEN
			SELECT
				1 AS result;
		# 플레이어 이름 중복 체크
        ELSEIF EXISTS (SELECT
					1
				FROM table_auto_player
                WHERE player_name = p_player_name LIMIT 1) THEN
			SELECT
				2 AS result;
		ELSEIF EXISTS (SELECT
					1
				FROM table_auto_player
                WHERE player_db_key = p_player_db_key LIMIT 1) THEN
			SELECT
				3 AS result;
		ELSE 
			INSERT INTO table_auto_player (user_db_key,
            player_db_key,
            player_name,
            level)
				VALUES (p_user_db_key, p_player_db_key, p_player_name, p_player_level);
                
			SELECT
				0 AS result,
                player_db_key,
                player_name,
                level
			FROM table_auto_player
            WHERE player_db_key = p_player_db_key;
		END IF;        
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `gp_user_player_list_load` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`ubf`@`%` PROCEDURE `gp_user_player_list_load`(
	IN p_user_db_key BIGINT UNSIGNED,
	IN p_encode_account_id varchar(50),
    IN p_gm_level TINYINT
)
BEGIN
	DECLARE ProcParam varchar(4000);
	DECLARE EXIT HANDLER FOR SQLEXCEPTION
	BEGIN
		GET DIAGNOSTICS @cno = NUMBER;
		GET DIAGNOSTICS CONDITION @cno
		@p_ErrorState = RETURNED_SQLSTATE, @p_ErrorNo = MYSQL_ERRNO, @p_ErrorMessage = MESSAGE_TEXT;
		SET ProcParam = CONCAT(p_user_db_key, ',', p_encode_account_id, ',', p_gm_level);
		INSERT INTO table_errorlog (procedure_name, error_state, error_no, error_message, create_time)
			VALUES ('gp_user_player_list_load', @p_ErrorState, @p_ErrorNo, @p_ErrorMessage, ProcParam);
		RESIGNAL;
	END;
    
    IF NOT EXISTS (SELECT
				user_db_key
			FROM table_auto_guser
            WHERE user_db_key = p_user_db_key) THEN
		INSERT INTO table_auto_guser (user_db_key, encode_account_id, gm_level)
			VALUES (p_user_db_key, p_encode_account_id, p_gm_level);
	END IF;
    
    SELECT
		player_db_key,
        player_name,
        create_time,
        logout_time,
        level,
        exp
	FROM table_auto_player
    WHERE user_db_key = p_user_db_key;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-07-03 21:10:41

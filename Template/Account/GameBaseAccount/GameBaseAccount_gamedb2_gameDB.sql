USE gamedb2;
DROP TABLE if exists player;
CREATE TABLE table_auto_player (	
	idx BIGINT UNSIGNED NOT NULL AUTO_INCREMENT,
	player_db_key BIGINT UNSIGNED NOT NULL DEFAULT 0,
	user_db_key BIGINT UNSIGNED NOT NULL DEFAULT 0,
	create_time DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '생성시간',
	update_time DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '수정시간',
	login_time DATETIME  NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '로그인 시간',
	logout_time DATETIME  NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '로그아웃 시간',
	is_login BIT NOT NULL DEFAULT b'0' COMMENT '접속여부',
	newbie BIT NOT NULL DEFAULT b'0' COMMENT '첫생성자 여부',
	serial_allocator BIGINT NOT NULL DEFAULT 0 COMMENT '시리얼번호 생성기',
	player_name VARCHAR(100) NOT NULL DEFAULT '' COMMENT '플레이어 이름',
	level SMALLINT NOT NULL DEFAULT 0 COMMENT '레벨',
	exp BIGINT NOT NULL DEFAULT 0 COMMENT '경험치',
	PRIMARY KEY(idx)
)
ENGINE = INNODB,
CHARACTER SET utf8mb4,
COLLATE utf8mb4_general_ci;
ALTER TABLE table_auto_player
ADD UNIQUE INDEX ix_player_player_db_key_user_db_key (player_db_key,user_db_key);
DROP PROCEDURE if exists gp_player_player_load;
DELIMITER $$

CREATE PROCEDURE gp_player_player_load(
    IN p_player_db_key BIGINT UNSIGNED,
    IN p_user_db_key BIGINT UNSIGNED
)
BEGIN

	DECLARE ProcParam varchar(4000);

	DECLARE EXIT HANDLER FOR SQLEXCEPTION
	BEGIN
		GET DIAGNOSTICS @cno = NUMBER;
			GET DIAGNOSTICS CONDITION @cno
			@p_ErrorState = RETURNED_SQLSTATE, @p_ErrorNo = MYSQL_ERRNO, @p_ErrorMessage = MESSAGE_TEXT;
		SET ProcParam = CONCAT(p_player_db_key,', ', p_user_db_key);
		INSERT INTO table_errorlog(procedure_name, error_state, error_no, error_message, param) VALUES('gp_player_player_load', @p_ErrorState, @p_ErrorNo, @p_ErrorMessage, ProcParam);
		RESIGNAL;
	END;

    IF NOT EXISTS(SELECT player_db_key,user_db_key FROM table_auto_player WHERE player_db_key = p_player_db_key AND user_db_key = p_user_db_key) THEN
        INSERT INTO table_auto_player(player_db_key,user_db_key)VALUES(p_player_db_key,p_user_db_key);
    END IF;

    SELECT * FROM table_auto_player WHERE player_db_key = p_player_db_key AND user_db_key = p_user_db_key;

END
$$
DELIMITER ;DROP PROCEDURE if exists gp_player_player_save;
DELIMITER $$

CREATE PROCEDURE gp_player_player_save(
    IN p_player_db_key BIGINT UNSIGNED,
    IN p_user_db_key BIGINT UNSIGNED,
    IN p_create_time DATETIME,
    IN p_update_time DATETIME,
    IN p_login_time DATETIME,
    IN p_logout_time DATETIME,
    IN p_is_login BIT,
    IN p_newbie BIT,
    IN p_serial_allocator BIGINT,
    IN p_player_name VARCHAR(100),
    IN p_level SMALLINT,
    IN p_exp BIGINT
)
BEGIN

    DECLARE ProcParam varchar(4000);

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS @cno = NUMBER;
			GET DIAGNOSTICS CONDITION @cno
			@p_ErrorState = RETURNED_SQLSTATE, @p_ErrorNo = MYSQL_ERRNO, @p_ErrorMessage = MESSAGE_TEXT;
		SET ProcParam = CONCAT(p_player_db_key,',',p_user_db_key,',',p_login_time,',',p_logout_time,',',p_is_login,',',p_newbie,',',p_serial_allocator,',',p_player_name,',',p_level,',',p_exp);
		INSERT INTO table_errorlog(procedure_name, error_state, error_no, error_message, param)
			VALUES('gp_player_player_save', @p_ErrorState, @p_ErrorNo, @p_ErrorMessage, ProcParam);
		RESIGNAL;
	END;

	UPDATE table_auto_player
    SET
		player_db_key = p_player_db_key,
		user_db_key = p_user_db_key,
		createTime = p_createTime,
		updateTime = p_updateTime,
		login_time = p_login_time,
		logout_time = p_logout_time,
		is_login = p_is_login,
		newbie = p_newbie,
		serial_allocator = p_serial_allocator,
		player_name = p_player_name,
		level = p_level,
		exp = p_exp
		WHERE player_db_key = p_player_db_key AND
		user_db_key = p_user_db_key;


END
$$
DELIMITER ;
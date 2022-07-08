DELIMITER $$

DROP TABLE if exists gamedb1.DBItemTable;CREATE TABLE gamedb1.table_auto_dbitemtable (	
	idx BIGINT UNSIGNED NOT NULL AUTO_INCREMENT,
	user_db_key BIGINT UNSIGNED NOT NULL DEFAULT 0,
	player_db_key BIGINT UNSIGNED NOT NULL DEFAULT 0,
	slot SMALLINT NOT NULL DEFAULT 0 COMMENT '슬롯 번호',	deleted BIT NOT NULL DEFAULT b'0' COMMENT '삭제 여부',	create_time DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '생성시간',
	update_time DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '수정시간',
	item_type COMMENT '아이템 타입',
	item_id COMMENT '아이템 아이디',
	item_count COMMENT '아이템 카운트',
	remain_charge_timeDATETIME  NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '남은 충전 타입',
	PRIMARY KEY(idx)
)
ENGINE = INNODB,
CHARACTER SET utf8mb4,
COLLATE utf8mb4_general_ci;
DROP PROCEDURE if exists gamedb1.gp_player_dbitemtable_load;CREATE PROCEDURE gamedb1.gp_player_dbitemtable_load(
    IN p_user_db_key BIGINT UNSIGNED,
    IN p_player_db_key BIGINT UNSIGNED
)
BEGIN

	DECLARE ProcParam varchar(4000);

	DECLARE EXIT HANDLER FOR SQLEXCEPTION
	BEGIN
		GET DIAGNOSTICS @cno = NUMBER;
			GET DIAGNOSTICS CONDITION @cno
			@p_ErrorState = RETURNED_SQLSTATE, @p_ErrorNo = MYSQL_ERRNO, @p_ErrorMessage = MESSAGE_TEXT;
		SET ProcParam = CONCAT(p_user_db_key,", "p_player_db_key);
		INSERT INTO table_errorlog(procedure_name, error_state, error_no, error_message, param) VALUES('gp_player_dbitemtable_load', @p_ErrorState, @p_ErrorNo, @p_ErrorMessage, ProcParam);
		RESIGNAL;
	END;

    SELECT * FROM table_auto_dbitemtable WHERE user_db_key = p_user_db_key AND player_db_key = p_player_db_key AND deleted = 0;

END
DROP PROCEDURE if exists gamedb1.gp_player_dbitemtable_save;CREATE PROCEDURE gamedb1.gp_player_dbitemtable_save(
    IN p_user_db_key BIGINT UNSIGNED,
    IN p_player_db_key BIGINT UNSIGNED,
    IN p_slot SMALLINT,
    IN p_deleted BIT,
    IN p_createTime DATETIME,
    IN p_updateTime DATETIME,
    IN p_item_type ,
    IN p_item_id ,
    IN p_item_count ,
    IN p_remain_charge_time DATETIME
)
BEGIN

    DECLARE ProcParam varchar(4000);

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS @cno = NUMBER;
			GET DIAGNOSTICS CONDITION @cno
			@p_ErrorState = RETURNED_SQLSTATE, @p_ErrorNo = MYSQL_ERRNO, @p_ErrorMessage = MESSAGE_TEXT;
		SET ProcParam = CONCAT(p_user_db_key,',',p_player_db_key,',',p_slot, ',', p_deleted, ',', p_create_time, ',', p_update_time, ',', p_item_type,',',p_item_id,',',p_item_count,',',p_remain_charge_time);
		INSERT INTO table_errorlog(procedure_name, error_state, error_no, error_message, param)
			VALUES('gp_player_dbitemtable_save', @p_ErrorState, @p_ErrorNo, @p_ErrorMessage, ProcParam);
		RESIGNAL;
	END;

	INSERT INTO table_auto_dbitemtable (
		user_db_key,
		player_db_key,
		slot,
		deleted,
		update_time,
		item_type,
		item_id,
		item_count,
		remain_charge_time
	)
	VALUES (
		p_user_db_key,
		p_player_db_key,
		p_item_type,
		p_item_id,
		p_item_count,
		p_remain_charge_time
	)
	ON DUPLICATE KEY
	UPDATE
		create_time = CASE WHEN deleted = 1 AND p_deleted = 0 THEN CURRENT_TIMESTAMP() ELSE create_time END,
		deleted = p_deleted,
		update_time = p_update_time,
		item_type = p_item_type,
		item_id = p_item_id,
		item_count = p_item_count,
		remain_charge_time = p_remain_charge_time;

END

DELIMITER ;
USE gamedb2;
DROP TABLE if exists table_auto_dbshoptable;
CREATE TABLE table_auto_dbshoptable (	
	idx BIGINT UNSIGNED NOT NULL AUTO_INCREMENT,
	player_db_key BIGINT UNSIGNED NOT NULL DEFAULT 0,
	slot SMALLINT NOT NULL DEFAULT 0 COMMENT '슬롯 번호',
	deleted BIT NOT NULL DEFAULT b'0' COMMENT '삭제 여부',
	create_time DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '생성시간',
	update_time DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '수정시간',
	shop_index INT NOT NULL DEFAULT 0 COMMENT 'Shop인덱스',
	shop_product_index INT NOT NULL DEFAULT 0 COMMENT 'ShopProductList인덱스',
	buy_count INT NOT NULL DEFAULT 0 COMMENT '구매횟수',
	PRIMARY KEY(idx)
)
ENGINE = INNODB,
CHARACTER SET utf8mb4,
COLLATE utf8mb4_general_ci;
ALTER TABLE table_auto_dbshoptable
ADD UNIQUE INDEX ix_dbshoptable_player_db_key_slot (player_db_key,slot);
DROP PROCEDURE if exists gp_player_dbshoptable_load;
DELIMITER $$

CREATE PROCEDURE gp_player_dbshoptable_load(
    IN p_player_db_key BIGINT UNSIGNED)
BEGIN

	DECLARE ProcParam varchar(4000);

	DECLARE EXIT HANDLER FOR SQLEXCEPTION
	BEGIN
		GET DIAGNOSTICS @cno = NUMBER;
			GET DIAGNOSTICS CONDITION @cno
			@p_ErrorState = RETURNED_SQLSTATE, @p_ErrorNo = MYSQL_ERRNO, @p_ErrorMessage = MESSAGE_TEXT;
		SET ProcParam = CONCAT(p_player_db_key);
		INSERT INTO table_errorlog(procedure_name, error_state, error_no, error_message, param) VALUES('gp_player_dbshoptable_load', @p_ErrorState, @p_ErrorNo, @p_ErrorMessage, ProcParam);
		RESIGNAL;
	END;

    SELECT * FROM table_auto_dbshoptable WHERE player_db_key = p_player_db_key AND deleted = 0;

END
$$
DELIMITER ;DROP PROCEDURE if exists gp_player_dbshoptable_save;
DELIMITER $$

CREATE PROCEDURE gp_player_dbshoptable_save(
    IN p_player_db_key BIGINT UNSIGNED,
    IN p_slot SMALLINT,
    IN p_deleted BIT,
    IN p_create_time DATETIME,
    IN p_update_time DATETIME,
    IN p_shop_index INT,
    IN p_shop_product_index INT,
    IN p_buy_count INT
)
BEGIN

    DECLARE ProcParam varchar(4000);

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS @cno = NUMBER;
			GET DIAGNOSTICS CONDITION @cno
			@p_ErrorState = RETURNED_SQLSTATE, @p_ErrorNo = MYSQL_ERRNO, @p_ErrorMessage = MESSAGE_TEXT;
		SET ProcParam = CONCAT(p_player_db_key,',',p_slot, ',', p_deleted, ',', p_create_time, ',', p_update_time, ',', p_shop_index,',',p_shop_product_index,',',p_buy_count);
		INSERT INTO table_errorlog(procedure_name, error_state, error_no, error_message, param)
			VALUES('gp_player_dbshoptable_save', @p_ErrorState, @p_ErrorNo, @p_ErrorMessage, ProcParam);
		RESIGNAL;
	END;

	INSERT INTO table_auto_dbshoptable (
		player_db_key,
		slot,
		deleted,
		update_time,
		shop_index,
		shop_product_index,
		buy_count
	)
	VALUES (
		p_player_db_key,
		p_slot,
		p_deleted,
		p_update_time,
		p_shop_index,
		p_shop_product_index,
		p_buy_count
	)
	ON DUPLICATE KEY
	UPDATE
		create_time = CASE WHEN deleted = 1 AND p_deleted = 0 THEN CURRENT_TIMESTAMP() ELSE create_time END,
		deleted = p_deleted,
		update_time = p_update_time,
		shop_index = p_shop_index,
		shop_product_index = p_shop_product_index,
		buy_count = p_buy_count;

END
$$
DELIMITER ;
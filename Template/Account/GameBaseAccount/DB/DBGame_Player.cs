using System;
using System.Collections.Generic;
using System.Text;
using Service.DB;
using Service.Net;
using GameBase.Template.GameBase.Common;
using GameBase.Template.Account.GameBaseAccount.Common;
using System.Data.Odbc;

namespace GameBase.Template.GameBase
{
    public class DBGame_PlayerList_LoadAll : QueryBaseValidate
    {
        //IN
        public string _encode_account_id;
        public byte _gm_level;

        //Out
        public List<PlayerInfo> _playerInfos = new List<PlayerInfo>();

        public DBGame_PlayerList_LoadAll(UserObject obj) : base(obj) { }

        public override void vRun(AdoDB adoDB)
        {
            try
            {
                QueryBuilder query = new QueryBuilder("call gp_user_player_list_load(?,?)");
                query.SetInputParam("@p_user_db_key", GetCallerUserDBKey());
                query.SetInputParam("@p_encode_account_id", _encode_account_id);
                query.SetInputParam("@p_gm_level", _gm_level);

                adoDB.Execute(query);

                if (adoDB.RecordNotEOF())
                {
                    PlayerInfo player = new PlayerInfo();

                    player.PlayerDBKey = adoDB.RecordGetValue("player_db_key");
                    player.PlayerName = adoDB.RecordGetStrValue("player_name");

                    _playerInfos.Add(player);
                }
                else
                {
                    _strResult = "[gp_user_player_list_load] No Result!";
                }
                adoDB.RecordEnd();
            }
            catch (OdbcException e)
            {
                adoDB.RecordEnd();
                _strResult = "[gp_user_player_list_load] " + e.Message;
            }
        }

        public override string vGetName()
        {
            return "gp_user_player_list_load";
        }
    }

    public class DBGame_Player_Create: QueryBaseValidate
    {
        public Byte _max_player_count;
        public ulong _player_db_key;
        public string _player_name;
        public short _player_level;
        public long _player_exp;

        public enum EResult { Success, MaxCountOver, DuplicateName, DuplicatePlayerDBKey };
        public EResult _result;
        public PlayerInfo _player = new PlayerInfo();
        public DBGame_Player_Create(UserObject obj) : base(obj) { }

        public override void vRun(AdoDB adoDB)
        {
            try
            {
                QueryBuilder query = new QueryBuilder("call gp_user_player_create(?,?,?,?)");
                query.SetInputParam("@p_user_db_key", GetCallerUserDBKey());
                query.SetInputParam("@p_max_player_count", _max_player_count);
                query.SetInputParam("@p_player_db_key", _player_db_key);
                query.SetInputParam("@p_player_name", _player_name);
                query.SetInputParam("@p_player_level", _player_level);
                query.SetInputParam("@p_player_exp", _player_exp);

                adoDB.Execute(query);

                if (adoDB.RecordNotEOF())
                {
                    _result = (EResult)(int)adoDB.RecordGetValue("result");
                    if (_result == EResult.Success)
                    {
                        _player.PlayerDBKey = adoDB.RecordGetValue("player_db_key");
                        _player.PlayerName = adoDB.RecordGetStrValue("player_name");
                    }
                }
                else
                {
                    _strResult = "[gp_user_player_create] No Result!";
                }
                adoDB.RecordEnd();
            }
            catch (OdbcException e)
            {
                adoDB.RecordEnd();
                _strResult = "[gp_user_player_create] " + e.Message;
            }
        }

        public override string vGetName()
        {
            return "gp_user_player_create";
        }
    }
}

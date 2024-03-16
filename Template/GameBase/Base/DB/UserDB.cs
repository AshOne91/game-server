using System;
using System.Collections.Generic;
using Service.DB;

namespace GameBase.Template.GameBase
{
    public partial class UserDB
    {
        Dictionary<ETemplateType, GameBaseUserDB> _userDBs = new Dictionary<ETemplateType, GameBaseUserDB>();
        public bool _IsLoaded = false;
        public bool _IsDBChanged = false;

        public bool AddUserDB(ETemplateType key, GameBaseUserDB baseUserDB)
        {
            if (_userDBs.ContainsKey(key) == true)
            {
                return false;
            }
            _userDBs.Add(key, baseUserDB);
            return true;
        }

        public T GetReadUserDB<T>(ETemplateType key) where T : GameBaseUserDB
        {
            GameBaseUserDB value;
            if (_userDBs.TryGetValue(key, out value) == false)
            {
                return null;
            }
            return value as T;
        }

        public T GetWriteUserDB<T>(ETemplateType key) where T : GameBaseUserDB
        {
            GameBaseUserDB value;
            if (_userDBs.TryGetValue(key, out value) == false)
            {
                return null;
            }
            _IsDBChanged = true;
            return value as T;
        }

        public void Copy(UserDB userSrc, bool isChanged)
        {
            foreach(var keyValue in _userDBs)
            {
                keyValue.Value.Copy(userSrc, isChanged);
            }
        }

        public void LoadRun(AdoDB adoDB, UInt64 user_db_key, UInt64 player_db_key)
        {
            foreach(var keyValue in _userDBs)
            {
                keyValue.Value.LoadRun(adoDB, user_db_key, player_db_key);
            }
        }

        public void SaveRun(AdoDB adoDB, UInt64 user_db_key, UInt64 player_db_key)
        {
            foreach(var keyValue in _userDBs)
            {
                keyValue.Value.SaveRun(adoDB, user_db_key, player_db_key);
            }
        }
    }
}

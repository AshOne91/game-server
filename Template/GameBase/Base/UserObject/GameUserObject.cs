using System;
using System.Collections.Generic;
using System.Text;
using GameBase.Template.GameBase.Common;
using Service.Core;
using Service.Net;

namespace GameBase.Template.GameBase
{
    public partial class GameUserObject : ImplObject
    {
        private UserSessionData _sessionData = new UserSessionData();
        private TimeCounter _updateSessionTick = new TimeCounter();
        private bool _isLogin = false;
        public bool IsLogin
        {
            get { return _isLogin; }
            set { _isLogin = value; }
        }
        private TimeCounter _updateDBTick = new TimeCounter();
        private bool _isDBLoaded = false;
        public bool IsDBLoaded
        {
            get { return _isDBLoaded; }
            set { _isDBLoaded = value; }
        }
        private int _updateSessionInterval = 60000;
        private int _updateDBInterval = 250;
        public UserSessionData SessionData 
        {
            get => _sessionData; 
            set => _sessionData = value;
        }
        private int _saveDBWaitCount = 0;
        public int SaveDBWaitCount
        {
            get { return _saveDBWaitCount; }
        }


        public GameUserObject()
        {
            _objectID = (int)ObjectType.User;
            _updateSessionTick.Start(_updateSessionInterval);
            _updateDBTick.Start(_updateDBInterval);
        }

        public void DBSaveAll(Action completeCallback = null)
        {
            if (IsDBLoaded == false)
            {
                return;
            }

            if (UserDB._IsDBChanged == false)
            {
                return;
            }

            UserDB._IsDBChanged = false;
            _saveDBWaitCount++;

            DBGameUserSave query = new DBGameUserSave();
            query._isConnected = this.GetSession().IsConnected();
            query._user_db_key = this.UserDBKey;
            query._player_db_key = this.PlayerDBKey;
            query._uid = GetSession().GetUid();
            query._userDB.Copy(UserDB, true);
            GameBaseTemplateContext.GetDBManager().PushQueryGame(UserDBKey, GameDBIdx, query, () =>
            {
                GameUserObject user = GameBaseTemplateContext.FindUserObj<GameUserObject>(query._uid);
                if (user != null)
                {
                    OnDBSaveComplete();
                }
                else
                {
                    Logger.Default.Log(ELogLevel.Err, "Not Found User {0}", query._uid);
                }

                if (completeCallback != null)
                {
                    completeCallback();
                }
            });
        }

        public void OnDBSaveComplete()
        {
            _saveDBWaitCount--;
        }

        public override void OnUpdate(float dt)
        {
            if (_updateSessionTick.IsFinished())
            {
                if (IsLogin == true)
                {
                    GameBaseTemplateContext.Account.UpdateSessionInfo(this);
                }
                _updateSessionTick.Start(_updateSessionInterval);
            }

            if (_updateDBTick.IsFinished())
            {
                DBSaveAll();
                _updateDBTick.Start(_updateDBInterval);
            }
        }
    }
}

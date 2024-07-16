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

            DBGameUserSave query = new DBGameUserSave();
            query._isConnected = this.GetSession().IsConnected();
            query._user_db_key = this.UserDBKey;
            query._player_db_key = this.PlayerDBKey;
            query._userDB.Copy(UserDB, true);
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

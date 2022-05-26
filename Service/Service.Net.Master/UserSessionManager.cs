using GameBase.Template.GameBase;
using Service.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Net.Master
{
    public class UserSessionManager
    {
        private Dictionary<string, UserSessionData> _sessionMap = new Dictionary<string, UserSessionData>();
        private PriorityLock _userSessionManagerLock = new PriorityLock((int)ELockPriority.SessionManager);
        private double _sessionTimeout = 60 * 7;

        public bool AddSession(UserSessionData s)
        {
            try
            {
                _userSessionManagerLock.Enter("AddSession");

                UserSessionData existSession = null;
                bool result = _sessionMap.TryGetValue(s.SiteUserId, out existSession);

                if (result == true && existSession != null)
                {
                    Logger.Default.Log(ELogLevel.Err, "UserSessionManager::AddSession Failed! Exist = {0}", existSession.GetLog());
                    Logger.Default.Log(ELogLevel.Err, "UserSessionManager::AddSession Failed! New = {0}", s.GetLog());
                    return false;
                }

                s.LastUpdateTime = DateTime.Now;
                _sessionMap.Add(s.SiteUserId, s);
                return true;
            }
            finally
            {
                _userSessionManagerLock.Exit();
            }
        }

        public bool UpdateSession(UserSessionData s)
        {
            try
            {
                _userSessionManagerLock.Enter("UpdateSession");
                UserSessionData existSession = null;
                bool result = _sessionMap.TryGetValue(s.SiteUserId, out existSession);
                if (result == false && existSession == null)
                {
                    Logger.Default.Log(ELogLevel.Err, "UserSessionManager::UpdateSession Failed! New = {0}", s.GetLog());
                    return false;
                }

                s.LastUpdateTime = DateTime.Now;
                _sessionMap[s.SiteUserId] = s;
                Logger.Default.Log(ELogLevel.Trace, "UserSessionManager::UpdateSession Success! New = {0}", s.GetLog());
                return true;

            }
            finally
            {
                _userSessionManagerLock.Exit();
            }
        }

        public bool RemoveSession(string siteUserId)
        {
            try
            {
                _userSessionManagerLock.Enter("RemoveSession");
                UserSessionData existSession = null;
                bool result = _sessionMap.TryGetValue(siteUserId, out existSession);
                if (result == true && existSession != null)
                {
                    result = _sessionMap.Remove(siteUserId);
                    if (result)
                    {
                        Logger.Default.Log(ELogLevel.Trace, "UserSessionManager::RemoveSession Success! New = {0}", existSession.GetLog());
                        return true;
                    }
                }
                Logger.Default.Log(ELogLevel.Trace, "UserSessionManager::RemoveSession Failed! New = {0}", siteUserId);
                return false;

            }
            finally
            {
                _userSessionManagerLock.Exit();
            }
        }
        public bool GetUserSession(string siteUserId, out UserSessionData info)
        {
            _userSessionManagerLock.Enter("AddSession");
            try
            {
                bool result = _sessionMap.TryGetValue(siteUserId, out info);
                if (result == true)
                {
                    Logger.Default.Log(ELogLevel.Trace, "SessionManager.GetUserSession {0}", siteUserId);
                    return true;
                }
            }
            finally
            {
                _userSessionManagerLock.Exit();
            }

            return false;
        }

        public void DebugPrintSessions()
        {
            _userSessionManagerLock.Enter("DebugPrintSessions");
            try
            {

                Logger.Default.Log(ELogLevel.Trace, "-- DebugPrintSessions ({0})------------", _sessionMap.Count);
                foreach (KeyValuePair<string, UserSessionData> kv in _sessionMap)
                {
                    Logger.Default.Log(ELogLevel.Trace, "Idx:{0} SvrId:{1} Name:{2} RoomIdx:{3} State:{4} RIP:{5} Date:{6} ",
                        kv.Value.PlayerIdx, kv.Value.ServerIdx, kv.Value.PlayerName, kv.Value.RoomIdx, kv.Value.State, kv.Value.RemoteIP, kv.Value.LastUpdateTime);
                }
            }
            finally
            {
                _userSessionManagerLock.Exit();
            }
        }

        public bool RemoveSessionByServerID(int serverId)
        {
            try
            {
                _userSessionManagerLock.Enter("RemoveSessionByServerID");

                DateTime now = DateTime.Now;
                List<string> deleteSessions = new List<string>();

                foreach (KeyValuePair<string, UserSessionData> kv in _sessionMap)
                {
                    if (kv.Value.ServerIdx == serverId)
                    {
                        deleteSessions.Add(kv.Key);
                    }
                }

                foreach (string key in deleteSessions)
                {
                    _sessionMap.Remove(key);
                    Logger.Default.Log(ELogLevel.Trace, "SessionManager.RemoveSessionByServerID {0}", key);
                }

            }
            finally
            {
                _userSessionManagerLock.Exit();
            }

            return true;
        }

        public bool RemoveSessionByTimeout()
        {
            try
            {
                _userSessionManagerLock.Enter("RemoveSessionByTimeout");
                DateTime now = DateTime.Now;
                List<string> deleteSessions = new List<string>();

                foreach (KeyValuePair<string, UserSessionData> kv in _sessionMap)
                {
                    double diff = (now - kv.Value.LastUpdateTime).TotalSeconds;
                    if (diff > _sessionTimeout)
                    {
                        deleteSessions.Add(kv.Key);
                    }
                }

                foreach (string key in deleteSessions)
                {
                    _sessionMap.Remove(key);
                    Logger.Default.Log(ELogLevel.Trace, "SessionManager.RemoveSessionByTimeout {0}", key);
                }
            }
            finally
            {
                _userSessionManagerLock.Exit();
            }

            return true;
        }

    }
}

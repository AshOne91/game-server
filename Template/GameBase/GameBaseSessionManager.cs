using System;
using System.Collections.Generic;
using System.Text;
using Service.Core;
using GameBase.Template.GameBase.Common;


namespace GameBase.Template.GameBase
{
    public sealed class GameBaseSessionManager
    {
        private Dictionary<string, UserSessionData> _SessionMap = new Dictionary<string, UserSessionData>();
        private double _SessionTimeout = 60 * 7;

        public bool AddSession(UserSessionData s)
        {
            UserSessionData existSession = null;
            bool result = _SessionMap.TryGetValue(s.SiteUserId, out existSession);

            if (result == true && existSession != null)
            {
                Logger.Default.Log(ELogLevel.Err, "SessionManager::AddSession Failed! Exist = {0}", existSession.GetLog());
                Logger.Default.Log(ELogLevel.Err, "SessionManager::AddSession Failed! New = {0}", s.GetLog());
                return false;
            }

            s.LastUpdateTime = DateTime.UtcNow;
            _SessionMap.Add(s.SiteUserId, s);
            return true;
        }
        public bool UpdateSession(UserSessionData s)
        {
            UserSessionData existSession = null;
            bool result = _SessionMap.TryGetValue(s.SiteUserId, out existSession);
            if (result == false && existSession == null)
            {
                Logger.Default.Log(ELogLevel.Err, "SessionManager::UpdateSession Failed! New = {0}", s.GetLog());
                return false;
            }

            s.LastUpdateTime = DateTime.UtcNow;
            _SessionMap[s.SiteUserId] = s;
            Logger.Default.Log(ELogLevel.Trace, "SessionManager::UpdateSession Success! New = {0}", s.GetLog());
            return true;
        }
        public bool RemoveSession(string siteUserId)
        {
            UserSessionData existSession = null;
            bool result = _SessionMap.TryGetValue(siteUserId, out existSession);
            if (result == true && existSession != null)
            {
                result = _SessionMap.Remove(siteUserId);
                if (result)
                {
                    Logger.Default.Log(ELogLevel.Trace, "SessionManager::RemoveSession Success! New = {0}", existSession.GetLog());
                    return true;
                }
            }
            Logger.Default.Log(ELogLevel.Trace, "SessionManager::RemoveSession Failed! New = {0}", siteUserId);
            return false;
        }
        public bool GetUserSession(string siteUserId, out UserSessionData info)
        {
            bool result = _SessionMap.TryGetValue(siteUserId, out info);
            if (result == true)
            {
                Logger.Default.Log(ELogLevel.Trace, "SessionManager.GetUserSession {0}", siteUserId);
                return true;
            }
            return false;
        }

        public bool RemoveSessionByServerID(int serverId)
        {
            DateTime now = DateTime.UtcNow;
            List<string> deleteSessionList = new List<string>();

            foreach (var pair in _SessionMap)
            {
                if (pair.Value.ServerIdx == serverId)
                {
                    deleteSessionList.Add(pair.Key);
                }
            }

            foreach (var key in deleteSessionList)
            {
                _SessionMap.Remove(key);
                Logger.Default.Log(ELogLevel.Trace, "SessionManager.RemoveSessionByServerID {0}", key);
            }
            return true;
        }

        public bool RemoveSessionByTimeout()
        {
            DateTime now = DateTime.Now;
            List<string> deleteSessionList = new List<string>();

            foreach (var pair in _SessionMap)
            {
                double diff = (now - pair.Value.LastUpdateTime).TotalSeconds;
                if (diff > _SessionTimeout)
                {
                    deleteSessionList.Add(pair.Key);
                }
            }

            foreach (string key in deleteSessionList)
            {
                _SessionMap.Remove(key);
                Logger.Default.Log(ELogLevel.Trace, "SessionManager.RemoveSessionByTimeout {0}", key);
            }

            return true;
        }
    }
}

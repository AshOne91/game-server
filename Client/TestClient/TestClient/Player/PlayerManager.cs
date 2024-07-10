using Service.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestClient.TestClient.Player
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        private Dictionary<ulong /*playerDBKey*/, Player> _playerByDBKey = new Dictionary<ulong , Player>();
        public Dictionary<ulong, Player> PlayerByDBKey
        {
            get { return _playerByDBKey; }
        }

        Player _selectPlayer = null;


        public void CreatePlayer(ulong playerDBKey, string siteUserId, string playerName, string version, string matchToken)
        {
            Player createPlayer = new Player();
            createPlayer.PlayerDBKey = playerDBKey;
            createPlayer.SiteUserId = siteUserId;
            createPlayer.PlayerName = playerName;
            createPlayer.Version = version;
            createPlayer.MatchToken = matchToken;
            _playerByDBKey.Add(playerDBKey, createPlayer);
        }

        public void SelectPlayer(ulong playerDBKey)
        {
            _selectPlayer = _playerByDBKey[playerDBKey];
        }

        public Player GetSelectPlayer()
        {
            return _selectPlayer;
        }

        public void Clear()
        {
            _playerByDBKey.Clear();
            _selectPlayer = null;
        }

        private int _cur = 0;
        public int Cur
        {
            get { return _cur; }
        }
        public void Seek(int off)
        {
            if (_playerByDBKey.Count == 0)
            {
                _cur = 0;
                return;
            }

            _cur = _cur + off;
            _cur = Math.Min(_cur, _playerByDBKey.Count - 1);
            _cur = Math.Max(_cur, 0);
        }
        public Player GetSeekPlayer()
        {
            if (_playerByDBKey.Count == 0)
            {
                return null;
            }

            return _playerByDBKey.ElementAt(Cur).Value;
        }


    }
}

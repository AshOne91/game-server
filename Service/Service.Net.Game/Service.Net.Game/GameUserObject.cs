using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Net.Game
{
    public class GameServerConfig
    {
        public GameServerConfig()
        {
            //_Ini = new NwIni("../datas/game_config.ini");
            LoadConfig();
        }

        public void LoadConfig()
        {
            _Ver = _Ini.GetValue("version", "server", "1.0.0");

            _MasterIP = _Ini.GetValue("master_ip", "server", "127.0.0.1");
            _MasterPort = ushort.Parse(_Ini.GetValue("master_port", "server", "17000"));

            _MatchingIP = _Ini.GetValue("matching_ip", "server", "127.0.0.1");
            _MatchingPort = ushort.Parse(_Ini.GetValue("matching_port", "server", "14000"));

            _DediPoolIP = _Ini.GetValue("dedipool_ip", "server", "127.0.0.1");
            _DediPoolPort = ushort.Parse(_Ini.GetValue("dedipool_port", "server", "19500"));

            _ClientIP = _Ini.GetValue("client_ip", "server", "127.0.0.1");
            _ClientPort = ushort.Parse(_Ini.GetValue("client_port", "server", "11000"));
            _DediPort = ushort.Parse(_Ini.GetValue("dedi_port", "server", "18000"));

            _ForceStartTick = int.Parse(_Ini.GetValue("force_start_tick", "match", "1000"));

            _ShardConnString = _Ini.GetValue("shard_conn_string", "db", "Server=133.186.139.97;Database=shardingdb;Uid=now_server;Pwd=Tlqktls^^321@#;");
            _GlobalConnString = _Ini.GetValue("global_conn_string", "db", "Server=133.186.139.97;Database=globaldb;Uid=now_server;Pwd=Tlqktls^^321@#;");
            _CrashReportURL = _Ini.GetValue("crash_report_url", "log_server", string.Format("http://{0}:{1}/upload_server_crash_report", "133.186.139.97", "10004"));
        }

        public string _Ver;
        public int _ForceStartTick;
        public string _MasterIP;
        public ushort _MasterPort;
        public string _MatchingIP;
        public ushort _MatchingPort;
        public string _DediPoolIP;
        public ushort _DediPoolPort;
        public string _ClientIP;
        public ushort _ClientPort;
        public ushort _DediPort;

        //public NwIni _Ini;
        public string _ShardConnString;
        public string _GlobalConnString;
        public string _CrashReportURL;
    }
    class GameUserObject
    {
    }
}

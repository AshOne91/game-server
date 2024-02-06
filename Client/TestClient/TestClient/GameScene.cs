using GameBase.Template.Account.GameBaseAccount;
using Service.Net;
using System;
using System.Collections.Generic;
using System.Text;
using TestClient.FrameWork;

namespace TestClient.TestClient
{
    public class GameScene : AppBaseScene<GameScene>
    {
        enum GamePage
        {
            Entry,
            Connecting,
            ConnectError,
            Disconnect,
            Connection,
            AuthComplete,
        }

        enum GameSequence
        {
            ConnectGameServer,
            PlayerListRequest,
        }
        private string _mainLogo = string.Empty;
        private GameSequence _gameStep = GameSequence.ConnectGameServer;
        private GamePage _gamePage = GamePage.Entry;
        private GameUserObject _userObject = null;
        protected sealed override void PreInit()
        {

        }
        protected sealed override void OnInit()
        {

        }
        protected sealed override void OnRelease()
        {

        }
        protected sealed override void Update()
        {
            if (Console.KeyAvailable)
            {

            }
            GameUI();
        }
        protected sealed override void OnEnter()
        {
            EventManager.Instance.AddEvent("Connectting", this);
            EventManager.Instance.AddEvent("ConnectionError", this);
            EventManager.Instance.AddEvent("Connection", this);
            EventManager.Instance.AddEvent("Disconnect", this);
            EventManager.Instance.AddEvent("AuthComplete", this);
        }
        protected sealed override void OnExit()
        {
            EventManager.Instance.RemoveEvent("Connectting", this);
            EventManager.Instance.RemoveEvent("ConnectionError", this);
            EventManager.Instance.RemoveEvent("Connection", this);
            EventManager.Instance.RemoveEvent("Disconnect", this);
            EventManager.Instance.RemoveEvent("AuthComplete", this);
            _mainLogo = string.Empty;
            ConsoleManager.Instance.ConsoleClear();
        }
        protected sealed override bool ReciveMessage(Message message)
        {
            switch (message.EventType)
            {
                case "Connectting":
                    _gamePage = GamePage.Connecting;
                    break;
                case "ConnectionError":
                    _gamePage = GamePage.ConnectError;
                    break;
                case "Connection":
                    _gamePage = GamePage.Connection;
                    break;
                case "Disconnect":
                    _gamePage = GamePage.Disconnect;
                    break;
                case "AuthComplete":
                    _gamePage = GamePage.AuthComplete;
                    break;
            }
            return true;
        }

        private void GameStep(GameSequence step)
        {
            if (_gameStep == step)
            {
                return;
            }
            _gameStep = step;
            switch (_gameStep) 
            {
                case GameSequence.ConnectGameServer:
                    {
                        _userObject = NetworkManager.Instance.GetUserObject(NetworkManager.Instance.AuthUID);
                        NetworkManager.Instance.GameConnect(_userObject.GetAccountImpl<GameBaseAccountClientImpl>()._IP
                            , _userObject.GetAccountImpl<GameBaseAccountClientImpl>()._Port
                            , NetworkManager.Instance.AuthUID);
                    }
                    break;
                case GameSequence.PlayerListRequest:
                    break;
            }
        }

        private void GameUI()
        {
            _mainLogo = string.Empty;
            _mainLogo += "======================================================================\n";
            _mainLogo += "======================          게임 서버        =====================\n";
            _mainLogo += "======================================================================\n";
            _mainLogo += "                                                                      \n";
            switch (_gamePage)
            {
                case GamePage.Entry:
                    _mainLogo += "1. 게임 서버 접속하기                                               \n";
                    break;
                case GamePage.Connecting:
                    _mainLogo += "접 속 중 ...                                                          \n";
                    break;
                case GamePage.ConnectError:
                    _mainLogo += "접 속 실 패                                                           \n";
                    break;
                case GamePage.Disconnect:
                    _mainLogo += "접 속 종 료                                                           \n";
                    break;
                case GamePage.Connection:
                    _mainLogo += "접 속 완 료                                                             \n";
                    break;
                case GamePage.AuthComplete:
                    _mainLogo += "인 증 완 료                                                          \n";
                    break;
            }
            ConsoleManager.Instance.SetBuffer(_mainLogo);
        }
    }
}

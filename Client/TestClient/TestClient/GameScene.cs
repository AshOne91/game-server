using GameBase.Template.Account.GameBaseAccount;
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
            Main
        }

        enum GameSequence
        {
            ConnectGameServerReady,
            ConnectLoginServer,
            AuthReady,
            AuthComplete
        }
        private string _mainLogo = string.Empty;
        private GameSequence _gameStep = GameSequence.ConnectGameServerReady;
        private GamePage _gamePage = GamePage.Entry;
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
        }
        protected sealed override void OnExit()
        {
            EventManager.Instance.RemoveEvent("Connectting", this);
            EventManager.Instance.RemoveEvent("ConnectionError", this);
            EventManager.Instance.RemoveEvent("Connection", this);
            EventManager.Instance.RemoveEvent("Disconnect", this);
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
                    _gamePage = GamePage.Main;
                    break;
                case "Disconnect":
                    _gamePage = GamePage.Disconnect;
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
                case GameSequence.ConnectLoginServer:
                    {
                        GameUserObject userObject = NetworkManager.Instance.GetUserObject(NetworkManager.Instance.AuthUID);
                        NetworkManager.Instance.GameConnect(userObject.GetAccountImpl<GameBaseAccountClientImpl>()._IP
                            , userObject.GetAccountImpl<GameBaseAccountClientImpl>()._Port
                            , NetworkManager.Instance.AuthUID);
                    }
                    break;
                case GameSequence.AuthReady: 
                    { 
                    }
                    break;
                case GameSequence.AuthComplete:
                    {

                    }
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
                case GamePage.Main:
                    _mainLogo += "인증 완료                                                             \n";
                    break;
            }
            ConsoleManager.Instance.SetBuffer(_mainLogo);
        }
    }
}

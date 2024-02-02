using GameBase.Template.Account.GameBaseAccount;
using GameBase.Template.GameBase;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using TestClient.FrameWork;

namespace TestClient.TestClient
{
    public class LoginScene : AppBaseScene<LoginScene>
    {
        enum LoginSequence
        {
            ConnectLoginServerReady,
            ConnectLoginServer,
            AuthReady,
            AuthComplete
        }
        enum LoginPageSequnce
        {
            Entry,
            Connecting,
            ConnectError,
            Disconnect,
            Main
        }
        private string _mainLogo = string.Empty;
        private LoginSequence _loginStep = LoginSequence.ConnectLoginServerReady;
        private LoginPageSequnce _loginPage = LoginPageSequnce.Entry;
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
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        {
                            LoginStep(LoginSequence.ConnectLoginServer);
                        }
                        break;
                }
            }

            LoginUI();
        }

        private void LoginStep(LoginSequence step)
        {
            if (_loginStep == step)
            {
                return;
            }

            _loginStep = step;

            switch (_loginStep)
            {
                case LoginSequence.ConnectLoginServer:
                    {
                        NetworkManager.Instance.LoginConnect("127.0.0.1", 10000);
                    }
                    break;
                case LoginSequence.AuthReady:
                    {

                    }
                    break;
                case LoginSequence.AuthComplete:
                    {
                        TestApp.Instance.LoadScene<GameScene>();
                    }
                    break;
            }
        }

        protected sealed override void OnEnter()
        {
            EventManager.Instance.AddEvent("Connecting", this);
            EventManager.Instance.AddEvent("ConnectionError", this);
            EventManager.Instance.AddEvent("Connection", this);
            EventManager.Instance.AddEvent("Disconnect", this);
        }
        protected sealed override void OnExit()
        {
            EventManager.Instance.RemoveEvent("Connecting", this);
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
                case "Connecting":
                    _loginPage = LoginPageSequnce.Connecting;
                    break;
                case "ConnectionError":
                    _loginPage = LoginPageSequnce.ConnectError;
                    break;
                case "Connection":
                    _loginPage = LoginPageSequnce.Main;
                    break;
                case "Disconnect":
                    _loginPage = LoginPageSequnce.Disconnect;
                    GameUserObject userObject = (GameUserObject)message.ExtraInfo;
                    if (userObject.GetAccountImpl<GameBaseAccountClientImpl>()._LoginAuth == true)
                    {
                        _loginStep = LoginSequence.AuthComplete;
                    }
                    break;
            }
            return true;
        }
        private void LoginUI()
        {
            _mainLogo = string.Empty;
            _mainLogo += "======================================================================\n";
            _mainLogo += "======================          로그인 서버      =====================\n";
            _mainLogo += "======================================================================\n";
            _mainLogo += "                                                                      \n";
            switch (_loginPage)
            {
                case LoginPageSequnce.Entry:
                    _mainLogo += "1. 로그인 서버 접속하기                                               \n";
                    break;
                case LoginPageSequnce.Connecting:
                    _mainLogo += "접 속 중 ...                                                          \n";
                    break;
                case LoginPageSequnce.ConnectError:
                    _mainLogo += "접 속 실 패                                                           \n";
                    break;
                case LoginPageSequnce.Disconnect:
                    _mainLogo += "접 속 종 료                                                           \n";
                    break;
                case LoginPageSequnce.Main:
                    _mainLogo += "인증 완료                                                             \n";
                    break;
            }
            ConsoleManager.Instance.SetBuffer(_mainLogo);
        }
    }
}

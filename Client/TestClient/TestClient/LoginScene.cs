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
        enum LoginPageSequnce
        {
            None,
            Entry,
            Connecting,
            ConnectError,
            Connection,
            Disconnect,
            AuthComplete
        }

        private string _mainLogo = string.Empty;
        private LoginPageSequnce _loginPage = LoginPageSequnce.None;
        private LoginPageSequnce LoginPage
        {
            get 
            {
                return _loginPage;
            }
            set
            {
                if (_loginPage == value) return;
                OnLeaveUI(_loginPage);
                _loginPage = value;
                OnEnterUI(_loginPage);
            }
        }

        private void OnLeaveUI(LoginPageSequnce loginPage)
        {
            InputManager.Instance.Clear();
        }

        private void OnEnterUI(LoginPageSequnce loginPage)
        {
            switch (loginPage) 
            {
                case LoginPageSequnce.Entry:
                {
                        InputManager.Instance.AddInputMap(0, ConsoleKey.D1, () =>
                        {
                            NetworkManager.Instance.LoginConnect(NetworkManager.Instance.AppConfig.clientConfig.loginServerIP
    , NetworkManager.Instance.AppConfig.clientConfig.loginServerPort);
                        });
                }
                break;
                case LoginPageSequnce.AuthComplete:
                {
                        InputManager.Instance.AddInputMap(0, ConsoleKey.D1, () =>
                        {
                            TestApp.Instance.LoadScene<GameScene>();
                        });
                }
                break;
            }
        }

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
            LoginUI();
        }

        protected sealed override void OnEnter()
        {
            EventManager.Instance.AddEvent("Connecting", this);
            EventManager.Instance.AddEvent("ConnectionError", this);
            EventManager.Instance.AddEvent("Connection", this);
            EventManager.Instance.AddEvent("Disconnect", this);
            EventManager.Instance.AddEvent("AuthComplete", this);
            LoginPage = LoginPageSequnce.Entry;
        }
        protected sealed override void OnExit()
        {
            EventManager.Instance.RemoveEvent("Connecting", this);
            EventManager.Instance.RemoveEvent("ConnectionError", this);
            EventManager.Instance.RemoveEvent("Connection", this);
            EventManager.Instance.RemoveEvent("Disconnect", this);
            EventManager.Instance.RemoveEvent("AuthComplete", this);
            _mainLogo = string.Empty;
            ConsoleManager.Instance.ConsoleClear();
            InputManager.Instance.Clear();
        }
        protected sealed override bool ReciveMessage(Message message)
        {
            switch (message.EventType)
            {
                case "Connecting":
                    LoginPage = LoginPageSequnce.Connecting;
                    break;
                case "ConnectionError":
                    LoginPage = LoginPageSequnce.ConnectError;
                    break;
                case "Connection":
                    LoginPage = LoginPageSequnce.Connection;
                    break;
                case "Disconnect":
                    LoginPage = LoginPageSequnce.Disconnect;
                    GameUserObject userObject = (GameUserObject)message.ExtraInfo;
                    if (userObject.GetAccountImpl<GameBaseAccountClientImpl>()._LoginAuth == true)
                    {
                        LoginPage = LoginPageSequnce.AuthComplete;
                    }
                    break;
                case "AuthComplete":
                    LoginPage = LoginPageSequnce.AuthComplete;
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
                case LoginPageSequnce.AuthComplete:
                    _mainLogo += "인증 완료                                                             \n";
                    _mainLogo += "1. 게임씬 이동하기                                                    \n";
                    break;
            }
            ConsoleManager.Instance.SetBuffer(_mainLogo);
        }
    }
}

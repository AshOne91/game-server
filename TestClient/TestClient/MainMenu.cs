using System;
using System.Collections.Generic;
using System.Text;
using TestClient.FrameWork;

namespace TestClient.TestClient
{
    public class MainMenu : AppBaseScene<MainMenu>
    {
        private string _mainLogo = string.Empty;
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

        }
        protected sealed override void OnEnter()
        {
            _mainLogo += "======================================================================\n";
            _mainLogo += "======================          메인 메뉴        =====================\n";
            _mainLogo += "======================================================================\n";
            _mainLogo += "                                                                      \n";
            _mainLogo += "1. 단일 명령 모드                                                     \n";
            ConsoleManager.Instance.SetBuffer(_mainLogo);
        }
        protected sealed override void OnExit()
        {
            _mainLogo = string.Empty;
            ConsoleManager.Instance.ConsoleClear();
        }
        protected sealed override bool ReciveMessage(Message message)
        {

            return true;
        }
    }
}

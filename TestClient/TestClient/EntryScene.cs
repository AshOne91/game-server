using System;
using System.Collections.Generic;
using System.Text;
using TestClient.FrameWork;

namespace TestClient.TestClient
{
    public enum ServerType
    {
        None = -1,
        Login
    }

    public class EntryScene:AppBaseScene<EntryScene>
    {
        private string _entryLogo = string.Empty;
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
            if (Console.KeyAvailable == true)
            {

            }
        }
        protected sealed override void OnEnter()
        {
            _entryLogo += "===================================\n";
            _entryLogo += "====     테스트 클라이언트     ====\n";
            _entryLogo += "===================================\n";
            _entryLogo += "         Create By 권성호\n";
            _entryLogo += "           PressAnyKey\n";
        }
        protected sealed override void OnExit()
        {
            _entryLogo = string.Empty;
            ConsoleManager.Instance.ConsoleClear();
        }
        protected sealed override bool ReciveMessage(Message message)
        {

            return true;
        }
    }
}

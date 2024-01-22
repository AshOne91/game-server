using System;
using System.Collections.Generic;
using System.Text;
using TestClient.FrameWork;

namespace TestClient.TestClient
{
    public class TestApp:Application<TestApp>
    {
        protected sealed override void DoUpdate()
        {

        }
        protected sealed override void OnEnable()
        {

        }
        protected sealed override void OnDisable()
        {

        }
        protected sealed override void PrepareInit()
        {
            AddAppSubSystem<NetworkManager>();
            AddAppSubSystem<ConsoleManager>();
        }
        protected sealed override void OnInit()
        {
            LoadScene<EntryScene>();
        }
        protected sealed override void OnRelease()
        {

        }
    }
}

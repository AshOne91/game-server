using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace TestClient.FrameWork
{
    public interface IUpdatable
    {
        void DoUpdate();
    }

    public interface ISceneController
    {
        void DoUpdateManaged();
    }

    public interface IEventInterface
    {
        bool OnMessage(Message message);
    }
}

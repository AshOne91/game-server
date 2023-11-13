using System;
using System.Collections.Generic;
using System.Text;

namespace TestClient.FramwWork
{
    public interface IUpdatable
    {
        void DoUpdate();
    }

    public interface ISceneController
    {
        void DoUpdateManaged();
    }
}

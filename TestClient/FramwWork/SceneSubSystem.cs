using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TestClient.FramwWork
{
    public abstract class SceneSubSystem<T> : Singleton<T>, IUpdatable where T : BaseObject, new() 
    {
        public abstract void DoUpdate();
    }
}

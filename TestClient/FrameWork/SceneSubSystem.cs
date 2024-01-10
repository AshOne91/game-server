using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TestClient.FrameWork
{
    public abstract class SceneSubSystem<T> : Singleton<T>, IUpdatable where T : BaseObject, new() 
    {
        public sealed override void Enable()
        {
            OnEnable();
        }
        public sealed override void Disable()
        {
            OnDisable();
        }
        public sealed override void Init()
        {
            OnInit();
        }
        public sealed override void Release()
        {
            OnRelease();
        }
        public abstract void DoUpdate();
        public abstract void OnEnable();
        public abstract void OnDisable();
        public abstract void OnInit();
        public abstract void OnRelease();
    }
}

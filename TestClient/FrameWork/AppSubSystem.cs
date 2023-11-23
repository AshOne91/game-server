using System;
using System.Collections.Generic;
using System.Text;

namespace TestClient.FrameWork
{
    public abstract class AppSubSystem<T> : Singleton<T>, IUpdatable where T : BaseObject, new()
    {
        public abstract void DoUpdate();
        public sealed override void Enable()
        {
            base.Enable();
            OnEnable();
        }
        public sealed override void Disable() 
        {
            OnDisable();
            base.Disable();
        }
        public sealed override void Init()
        {
            base.Init();
            OnInit();
        }
        public sealed override void Release()
        {
            OnRelease();
            base.Release();
        }

        public abstract void OnEnable();
        public abstract void OnDisable();
        public abstract void OnInit();
        public abstract void OnRelease();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace TestClient.FramwWork
{
    public abstract class AppSubSystem<T> : Singleton<T>, IUpdatable where T : BaseObject, new()
    {
        public abstract void DoUpdate();
        public override void Enable()
        {
            OnEnable();
        }
        public override void Disable() 
        {
            OnDisable();
        }
        public override void Init()
        {
            OnInit();
        }
        public override void Release()
        {
            OnRelease();
        }

        public abstract void OnEnable();
        public abstract void OnDisable();
        public abstract void OnInit();
        public abstract void OnRelease();
    }
}

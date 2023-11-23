using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace TestClient.FramwWork
{
    public abstract class SceneController<T> : Singleton<T>, ISceneController where T :BaseObject, new()
    {
        private Dictionary<Type, Tuple<BaseObject, IUpdatable>> _sceneSubSystems = new Dictionary<Type, Tuple<BaseObject, IUpdatable>>();
        private List<Tuple<BaseObject, IUpdatable>> _subSystems = new List<Tuple<BaseObject, IUpdatable>>();
        public void DoUpdateManaged()
        {
            foreach(var subSystem in _subSystems) 
            {
                subSystem.Item2.DoUpdate();
            }
            DoUpdate();
        }
        public sealed override void Init()
        {
            base.Init();
            foreach(var subSystem in _subSystems)
            {
                subSystem.Item1.Init();
            }
            OnInit();
        }
        public sealed override void Release()
        {
            OnRelease();
            foreach(var subSystem in _subSystems)
            {
                subSystem.Item1.Release();
                subSystem.Item1.Dispose();
            }
            _subSystems.Clear();
            _sceneSubSystems.Clear();
            base.Release();
        }
        public sealed override void Enable()
        {
            base.Enable();
            foreach(var subSystem in _subSystems)
            {
                subSystem.Item1.Enable();
            }
            Enter();
        }
        public sealed override void Disable() 
        {
            Leave();
            foreach( var subSystem in _subSystems)
            {
                subSystem.Item1.Disable();
            }
            base.Disable();
        }

        protected void AddSceneSubSystem<U>() where U : BaseObject, IUpdatable, new()
        {
            Tuple<BaseObject, IUpdatable> newSubSystem;
            BaseObject first = Singleton<U>.Instance;
            IUpdatable second = Singleton<U>.Instance as IUpdatable;
            newSubSystem = new Tuple<BaseObject, IUpdatable>(first, second);
            Type type = typeof(U);
            _sceneSubSystems.Add(type, newSubSystem);
            _subSystems.Add(newSubSystem);
        }

        protected void CreateSceneSubSystem<U>() where U : BaseObject, IUpdatable, new()
        {
            AddSceneSubSystem<U>();
            BaseObject first = Singleton<U>.Instance;
            first.Init();
        }

        protected U GetSceneSubSystem<U>() where U : BaseObject, IUpdatable, new()
        {
            if (_sceneSubSystems.ContainsKey(typeof(U)) == false)
            {
                return null;
            }
            return _sceneSubSystems[typeof(U)].Item1 as U;
        }

        protected abstract void DoUpdate();
        protected abstract void OnInit();
        protected abstract void OnRelease();
        protected abstract void Enter();
        protected abstract void Leave();

    }
}

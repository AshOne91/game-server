using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TestClient.FrameWork
{
    public abstract class Application<T> : Singleton<T> where T : BaseObject, new()
    {
        private Dictionary<Type, Tuple<BaseObject, IUpdatable>> _appSubSystems = new Dictionary<Type, Tuple<BaseObject, IUpdatable>>();
        private List<Tuple<BaseObject, IUpdatable>> _subSystemContainer = new List<Tuple<BaseObject, IUpdatable>>();
        private Dictionary<Type, Tuple<BaseObject, ISceneController>> _sceneController = new Dictionary<Type, Tuple<BaseObject, ISceneController>>();
        private Tuple<BaseObject, ISceneController> _activeSceneController = null;
        private bool _app_run = false;
        public bool AppRun => _app_run;
        protected Application() 
        { 
        }
        ~Application()
        {
        }
        public sealed override void Enable()
        {
            base.Enable();
            foreach(var subSystem in _subSystemContainer)
            {
                subSystem.Item1.Enable();
            }
            OnEnable();
        }
        public sealed override void Disable()
        {
            OnDisable();
            for (var num = _subSystemContainer.Count - 1; num >= 0; --num)
            {
                _subSystemContainer[num].Item1.Disable();
            }
            base.Disable();
        }

        public sealed override void Init()
        {
            base.Init();
            AddAppSubSystem<TimerManager>();
            AddAppSubSystem<EventManager>();
            AddAppSubSystem<ObjectManager>();
            PrepareInit();
            foreach(var subSystem in _subSystemContainer)
            {
                subSystem.Item1.Init();
            }
            foreach(var sceneController in _sceneController)
            {
                sceneController.Value.Item1.Init();
            }
            OnInit();
        }
        public sealed override void Release()
        {
            OnRelease();
            if (_activeSceneController != null)
            {
                _activeSceneController.Item1.Disable();
                _activeSceneController = null;
            }
            for (var num = _subSystemContainer.Count - 1; num >= 0; --num)
            {
                _subSystemContainer[num].Item1.Release();
            }
            foreach (var sceneController in _sceneController)
            {
                sceneController.Value?.Item1.Release();
            }

            _appSubSystems.Clear();
            _subSystemContainer.Clear();
            _sceneController.Clear();
            base.Disable();
        }
        public void Update()
        {
            while (AppRun == true)
            {
                foreach(var subSystemContainer in _subSystemContainer)
                {
                    subSystemContainer.Item2.DoUpdate();
                    if (AppRun == false)
                    {
                        break;
                    }
                }

                if (_activeSceneController != null) 
                {
                    _activeSceneController.Item2.DoUpdateManaged();
                }
                DoUpdate();
            }
            OnApplicationQuit();
        }
        // 나중에 확인(U where 절)
        void LoadScene<U>() where U : Singleton<U>, ISceneController, new()
        {
            if (_activeSceneController != null)
            {
                _activeSceneController.Item1.Disable();
                _activeSceneController = null;
            }
            Tuple<BaseObject, ISceneController> loadSceneController = new Tuple<BaseObject, ISceneController>
                (Singleton<U>.Instance, Singleton<U>.Instance);
            loadSceneController.Item1.Enable();
        }
        void AddScene<U>() where U : Singleton<U>, ISceneController, new()
        {
            Tuple<BaseObject, ISceneController> newSceneController = new Tuple<BaseObject, ISceneController>
                (Singleton<U>.Instance, Singleton<U>.Instance);
            newSceneController.Item1.Parent = this;
            _sceneController.Add(typeof(U), newSceneController);
        }
        void CreateScene<U>() where U : Singleton<U>, ISceneController, new()
        {
            AddScene<U>();
            U scene = GetScene<U>();
            scene.Init();
        }
        U GetScene<U>() where U : Singleton<U>, ISceneController, new()
        {
            return _sceneController[typeof(U)].Item1 as U;
        }
        void AddAppSubSystem<U>() where U : Singleton<U>, IUpdatable, new()
        {
            Tuple<BaseObject, IUpdatable> newAppSubSystem = new Tuple<BaseObject, IUpdatable>
                (Singleton<U>.Instance, Singleton<U>.Instance);
            newAppSubSystem.Item1.Parent = this;
            _appSubSystems.Add(typeof(U), newAppSubSystem);
        }
        void CreateAppSubSystem<U>() where U : Singleton<U>, IUpdatable, new()
        {
            AddAppSubSystem<U>();
            U subSystem = GetAppSubSystem<U>();
            subSystem.Init();
        }
        
        U GetAppSubSystem<U>() where U : Singleton<U>, IUpdatable, new()
        {
            return _appSubSystems[typeof(U)].Item1 as U;
        }
        void OnApplicationQuit()
        {
            Release();
        }
        protected abstract void OnEnable();
        protected abstract void OnDisable();
        protected abstract void PrepareInit();
        protected abstract void OnInit();
        protected abstract void OnRelease();
        protected abstract void DoUpdate();
    }
}

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace TestClient.FrameWork
{
    public abstract class Component  : BaseObject
    {
        private Dictionary<Type, Component> _componentList = new Dictionary<Type, Component>();

        public virtual void Update()
        {
            //UpdateComponents();
            DoUpdate();
        }
        public virtual void UpdateComponents()
        {
            foreach(var component in _componentList)
            {
                component.Value.Update();
            }
        }
        public abstract void DoUpdate();
        public virtual void ProcessInput(Byte keyState)
        {
            foreach(var component in _componentList)
            {
                component.Value.ProcessInput(keyState);
            }
        }

        public T CreateComponent<T>() where T : Component, new() 
        {
            T component = new T();
            var type = typeof(T);
            _componentList.Add(type, component);
            component.Parent = this;
            component.Init();

            EventManager.Instance.PostNotifycation("CreateComponent", NotifyType.BroadCast, Index, 0, 0.0f, false, component);
            return component;
        }
        public T GetComponent<T>() where T : Component, new()
        {
            var type = typeof (T);
            return _componentList[type] as T;
        }
        public T CreateObject<T>() where T : AppObject, new()
        {
            T gameObject = new T();
            gameObject.Parent = this;

            EventManager.Instance.PostNotifycation("CreateObjectInComponent", NotifyType.BroadCast, Index, 0, 0.0f, false, gameObject);
            return gameObject;
        }
        void DestroyObject(UInt64 index)
        {
            EventManager.Instance.PostNotifycation("DestroyObjectInComponent", NotifyType.BroadCast, Index, 0, 0.0f, false, index);
        }
        public sealed override void Enable()
        {
            base.Enable();
            foreach (var component in _componentList)
            {
                component.Value.Enable();
            }
            OnEnable();
        }
        public sealed override void Disable() 
        {
            OnDisiable();
            foreach (var component in _componentList)
            {
                component.Value.Disable();
            }
            base.Disable();
        }
        public sealed override void Init()
        {
            base.Init();
            foreach (var component in _componentList)
            {
                component.Value.Init();
            }
            OnInit();
        }
        public sealed override void Release()
        {
            OnRelease();
            foreach (var component in _componentList)
            {
                EventManager.Instance.PostNotifycation("ReleaseComponent", NotifyType.BroadCast, Index, 0, 0.0f, false, component);
                component.Value.Release(); 
            }
            _componentList.Clear();
            base.Release();
        }

        public abstract void OnEnable();
        public abstract void OnDisiable();
        public abstract void OnInit();
        public abstract void OnRelease();
    }
}

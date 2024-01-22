using System;
using System.Collections.Generic;
using System.Text;

namespace TestClient.FrameWork
{
    public abstract class AppBaseScene<T> : SceneController<T> where T : BaseObject, new()
    {
        private Dictionary<UInt64, AppObject> _objectList = new Dictionary<UInt64, AppObject>();
        public U CreateObject<U>() where U : AppObject, new()
        {
            U newObj = ObjectManager.Instance.CreateObject<U>();
            _objectList.Add(newObj.Index, newObj);
            return newObj;
        }
        public U DontDestroyObject<U>() where U : AppObject, new()
        {
            return ObjectManager.Instance.CreateObject<U>();
        }
        public void DestroyObject(UInt64 index)
        {
            _objectList.Remove(index);
            ObjectManager.Instance.DestroyObject(index);
        }
        public void AddObject(AppObject obj)
        {
            _objectList.Add(obj.Index, obj);
        }
        public void RemoveObject(UInt64 index)
        {
            _objectList.Remove(index);
        }

        protected sealed override void Enter()
        {
            OnEnter();
        }
        protected sealed override void Leave()
        {
            foreach(var obj in _objectList.Values) 
            {
                ObjectManager.Instance.DestroyObject(obj.Index);
            }
            _objectList.Clear();
            OnExit();
        }
        protected sealed override void DoUpdate()
        {
            Update();
        }
        protected abstract void Update();
        protected abstract void OnEnter();
        protected abstract void OnExit();
        protected abstract bool ReciveMessage(Message message);
        public sealed override bool OnMessage(Message message)
        {
            if (message.EventType == "CreateObjectInComponent")
            {
                AppObject obj = message.ExtraInfo as AppObject;
                if (obj == null) 
                {
                    throw new Exception();
                }
                AddObject(obj); 
                return true;
            }

            if (message.EventType == "DestroyObjectInComponent")
            {
                var index = (UInt64)message.ExtraInfo;
                RemoveObject(index);
                return true;
            }
            return ReciveMessage(message);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace TestClient.FramwWork
{
    public class ObjectManager : AppSubSystem<ObjectManager>
    {
        private Dictionary<UInt64, AppObject> _objectList;
        private List<UInt64>[] _destroyObjectList;
        private int _destroyIndex;

        public ObjectManager()
        {
            _objectList = new Dictionary<ulong, AppObject> ();
            _destroyObjectList = new List<UInt64>[2];
            _destroyObjectList[0] = new List<UInt64>();
            _destroyObjectList[1] = new List<UInt64>();
            _destroyIndex = 0;
        }

        public sealed override void DoUpdate()
        {
            foreach(var obj in _objectList)
            {
                obj.Value.Update();
            }
            if (_destroyObjectList[_destroyIndex].Count > 0)
            {
                int preIndex = _destroyIndex;
                _destroyIndex = (_destroyIndex + 1) % 2;
                foreach(var objIndex in _destroyObjectList[_destroyIndex])
                {
                    DestroyObject(objIndex);
                }
                _destroyObjectList[_destroyIndex].Clear();
            }
        }
        public sealed override void OnEnable()
        {
            
        }
        public sealed override void OnDisable() 
        { 
        }
        public sealed override void OnInit()
        {
            foreach(var obj in _objectList)
            {
                obj.Value.Init();
            }
        }
        public sealed override void OnRelease()
        {
            foreach (var obj in _objectList)
            {
                obj.Value.Release();
            }
            _objectList.Clear();
        }

        public sealed override bool OnMessage(Message message)
        {
            if (message.EventType == "CreateObjectInComponent") 
            {
                AppObject obj = message.ExtraInfo as AppObject;
                obj.Init();
                EventManager.Instance.PostNotifycation("CreateObject", NotifyType.BroadCast, Index, 0, 0.0f, false, obj);
                return true;
            }

            if (message.EventType == "DestroyObjectInComponent")
            {
                UInt64 destroyIndex = (ulong)message.ExtraInfo;
                _destroyObjectList[_destroyIndex].Add(destroyIndex);
            }
            return false;
        }
        public T CreateObject<T>() where T : AppObject, new()
        {
            T obj = new T();
            _objectList.Add(obj.Index, obj);

            EventManager.Instance.PostNotifycation("CreateObject", NotifyType.BroadCast, this.Index, 0, 0.0f, false, obj);
            return obj;
        }
        public T GetObject<T>(UInt64 index) where T : AppObject
        {
            _objectList.TryGetValue(index, out AppObject obj);
            return obj as T;
        }
        public void DestroyObject(UInt64 index)
        {
            _objectList.TryGetValue(index, out AppObject obj);
            if (obj == null)
            {
                return;
            }
            obj.Release();
            EventManager.Instance.PostNotifycation("DestroyObject", NotifyType.BroadCast, this.Index, 0, 0.0f, false, obj);
            _objectList.Remove(obj.Index);
        }
    }
}

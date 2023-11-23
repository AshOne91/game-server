using System;
using System.Collections.Generic;
using System.Text;

namespace TestClient.FrameWork
{
    public class EntityManager : Singleton<EntityManager>
    {
        private Dictionary<UInt64, BaseObject> _entityList = new Dictionary<UInt64, BaseObject>();

        public void RegisterEntity(BaseObject entity)
        {
            _entityList.Add(entity.Index, entity);
        }
        public void RemoveEntity(BaseObject entity)
        {
            _entityList.Remove(entity.Index);
        }
        public BaseObject GetEntityFromID(UInt64 index)
        {
            _entityList.TryGetValue(index, out BaseObject entity);
            return entity;
        }

        public sealed override void Enable()
        {

        }
        public sealed override void Disable()
        {

        }
        public sealed override void Init()
        {

        }
        public sealed override void Release()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core
{
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        // 메모리에서 데이터를 읽어라(캐시 사용 X)
        private static volatile T _instance = null;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                    _instance.Init();
                }
                return _instance;
            }
        }

        protected virtual void Init() { }
    }
}

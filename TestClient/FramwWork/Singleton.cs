using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TestClient.FramwWork
{
    public abstract class Singleton<T> : BaseObject where T : BaseObject, new()
    {
        private bool _disposed = false;
        // 메모리에서 데이터를 읽어라(캐시 사용 X)
        private static volatile T _instance = null;
        public static T Instance
        {
            get { 
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance; 
            }
        }

        protected Singleton()
        {

        }
        ~Singleton() => Dispose(false);

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {

            }

            _disposed = true;
            base.Dispose(disposing);
        }
    }
}

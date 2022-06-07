﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Service.Core
{
    public class AsyncTaskObject : ObjectCounter, IDisposable
    {
        public int _beginTick = Environment.TickCount;
        public MemoryStream _stream;
        public ushort _Cmd;
        public long _Uid;
        public string _Extra;

        public AsyncTaskObject()
        {
            _beginTick = Environment.TickCount;
        }
        ~AsyncTaskObject()
        {
            this.Dispose(false);
        }

        #region IDisposable Members
        private bool disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
                return;

            try
            {
                if (disposing)
                {
                    _stream.Dispose();
                }
            }
            catch
            {

            }
            finally
            {
                this.disposed = true;
            }



            this.disposed = true;
        }
        #endregion

        public void Serialize(object obj)
        {
            _stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(_stream, obj);

        }
        public object Deserialize()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            _stream.Seek(0, SeekOrigin.Begin);
            object objectType = formatter.Deserialize(_stream);
            return objectType;
        }
    }
}

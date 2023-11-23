using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace TestClient.FramwWork
{
    public enum EventType
    {
        None = 0,
    }
    public enum NotifyType
    {
        Mono = 0,
        BroadCast
    }
    public class Message: IComparable<Message> //: IDisposable//, IEnumerable<Message>//IComparable<Message>
    {
        private static readonly float _smallestDelay = 0.25f; 
        private String _eventType = "None";
        private NotifyType _notifyType = NotifyType.Mono;
        private UInt64 _sender = 0;
        private UInt64 _receiver = 0;
        private object _extra_info = null;

        public static float SmallestDelay => _smallestDelay;
        public String EventType{ get => _eventType; set => _eventType = value; }
        public NotifyType NotifyType { get => _notifyType; set => _notifyType = value; }
        public UInt64 Sender { get => _sender; set => _sender = value; }
        public UInt64 Receiver { get => _receiver; set => _receiver = value; }
        public object ExtraInfo { get => _extra_info; set => _extra_info = value; }

        private bool _dispatchRepeat = false;
        private float _dispatchDelay = 0.0f;
        private float _dispatchTime = 0.0f;
        public bool DispatchRepeat { get => _dispatchRepeat; set => _dispatchRepeat = value; }
        public float DispatchDelay { get => _dispatchDelay; set => _dispatchDelay = value; }
        public float DispatchTime { get => _dispatchTime; set => _dispatchTime = value; }

        public int CompareTo(Message other)
        {
            if (this.DispatchTime > other.DispatchTime)
            {
                return 1;
            }
            else if (this.DispatchTime < other.DispatchTime)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
        /*private bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public Message() { }
        ~Message() => Dispose(false);
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing) 
            { 
            }

            _disposed = true;
        }*/

        /*public static bool operator==(Message a, Message b)
        {
            return (Math.Abs(a.DispatchTime - b.DispatchTime) < Message._smallestDelay) &&
                (a.Sender == b.Sender) &&
                (a.Receiver == b.Receiver) &&
                (a.EventType == b.EventType) &&
                (a.NotifyType == b.NotifyType);
        }
        public static bool operator!=(Message a, Message b)
        {
            return !(a == b);
        }
        public static bool operator<(Message a, Message b)
        {
            if (a == b)
            {
                return false;
            }
            else
            {
                return (a.DispatchTime < b.DispatchTime);
            }
        }
        public static bool operator>(Message a, Message b)
        {
            if (a == b)
            {
                return false;
            }
            else
            {
                return (a.DispatchTime > b.DispatchTime);
            }
        }*/
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Transactions;

namespace TestClient.FramwWork
{
    public class EventManager : AppSubSystem<EventManager>
    {
        private Dictionary<string, List<IEventInterface>> _listener = new Dictionary<string, List<IEventInterface>>();
        private SortedSet<Message> _messageQueue = new SortedSet<Message>();
        private List<Message> _repeatList = new List<Message>();

        public void AddEvent(string eventType, IEventInterface eventObject)
        {
            if (_listener.ContainsKey(eventType) == false)
            {
                _listener.Add(eventType, new List<IEventInterface>());
            }
            _listener[eventType].Add(eventObject);
        }
        private void Discharge(Message message)
        {
            List<IEventInterface> eventList;
            if (_listener.TryGetValue(message.EventType, out eventList) == false)
            {
                return;
            }
            if (message.NotifyType == NotifyType.Mono)
            {
                var obj = EntityManager.Instance.GetEntityFromID(message.Receiver);
                if (obj != null)
                {
                    obj.OnMessage(message);
                }
            }
            else
            {
                foreach(var evt in eventList)
                {
                    evt.OnMessage(message);
                }
            }
        }
        public void RemoveEvent(string eventType)
        {
            _listener.Remove(eventType);
        }
        public void RemoveObject(IEventInterface eventObject)
        {
            foreach(var evt in _listener)
            {
                evt.Value.Remove(eventObject);
            }
        }
        public void PostNotifycation(string eventType, NotifyType notifyType, UInt64 sender, UInt64 receiver, float dispatchDelay, bool dispatchRepeat, object extra_info = null)
        {
            Message message = new Message();
            message.EventType = eventType;
            message.NotifyType = notifyType;
            message.Sender = sender;
            message.Receiver = receiver;
            message.ExtraInfo = extra_info;
            message.DispatchDelay = dispatchDelay;
            message.DispatchRepeat = dispatchRepeat;
            message.ExtraInfo = extra_info;
            if (message.DispatchDelay <= 0.0f)
            {
                Discharge(message);
            }
            else
            {
                float currentTime = TimerManager.Instance.DurationTime;
                message.DispatchTime = currentTime + message.DispatchDelay;
                _messageQueue.Add(message);
            }
        }
        public void DispatchDelayedMessage()
        {
            float currentTime = TimerManager.Instance.DurationTime;
            while (_messageQueue.Count > 0)
            {
                var msg = _messageQueue.First();
                if (msg.DispatchTime > currentTime)
                {
                    return;
                }
                Discharge(msg);
                if (msg.DispatchRepeat == true)
                {
                    _repeatList.Add(msg);
                }
                _messageQueue.Remove(msg);
            }

            foreach(var msg in _repeatList) 
            { 
                msg.DispatchTime = currentTime + msg.DispatchDelay;
                _messageQueue.Add(msg);
            }
        }
        public override void DoUpdate()
        {
            DispatchDelayedMessage();
        }
        public override void OnEnable()
        {

        }
        public override void OnDisable()
        {

        }
        public override void OnInit()
        {

        }
        public override void OnRelease()
        {

        }
    }
}

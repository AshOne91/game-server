using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Net
{
    public delegate void ControllerDelegate(Packet packet);
    class MessageController
    {
        Dictionary<UInt16, ControllerDelegate> _controllers;

        public MessageController()
        {
            _controllers = new Dictionary<UInt16, ControllerDelegate>();
        }

        public bool AddController(UInt16 protocolId, ControllerDelegate callback)
        {
            if (_controllers.ContainsKey(protocolId) == true)
            {
                return false;
            }

            _controllers.Add(protocolId, callback);
            return true;
        }

        public bool AddControllers(Dictionary<UInt16, ControllerDelegate> controllers)
        {
            foreach (var elem in controllers)
            {
                AddController(elem.Key, elem.Value);
            }
            return true;
        }

        public virtual void OnRecevice(UInt16 protocolId, Packet packet)
        {
            ControllerDelegate controllerCallback;
            if (_controllers.TryGetValue(protocolId, out controllerCallback) == false)
            {
                return;
            }

            controllerCallback(packet);
        }
    }
}

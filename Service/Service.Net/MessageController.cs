using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Net
{
    public delegate void ControllerDelegate(UserObject obj, Packet packet);
    public class MessageController
    {
        Dictionary<UInt16, ControllerDelegate> _controllers;
        UserObject _userObject;

        public MessageController(UserObject obj)
        {
            _controllers = new Dictionary<UInt16, ControllerDelegate>();
            _userObject = obj;
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

        public virtual bool OnRecevice(UInt16 protocolId, Packet packet)
        {
            ControllerDelegate controllerCallback;
            if (_controllers.TryGetValue(protocolId, out controllerCallback) == false)
            {
                return false;
            }

            controllerCallback(_userObject, packet);
            return true;
        }
    }
}

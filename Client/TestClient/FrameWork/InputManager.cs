using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestClient.FrameWork
{
    using InputLayer = System.Int32;

    public class InputManager : AppSubSystem<InputManager>
    {

        private SortedDictionary<InputLayer, Dictionary<ConsoleKey, List<Action>>> _InputMap = new SortedDictionary<InputLayer, Dictionary<ConsoleKey, List<Action>>>();
        public void AddInputMap(InputLayer layer, ConsoleKey consoleKey, Action func)
        {
            if (_InputMap.ContainsKey(layer) == false) 
            {
                _InputMap.Add(layer, new Dictionary<ConsoleKey, List<Action>>());
            }
            if (_InputMap[layer].ContainsKey(consoleKey) == false)
            {
                _InputMap[layer].Add(consoleKey, new List<Action>());
            }
            _InputMap[layer][consoleKey].Add(func);
        }

        public bool RemoveLayer(InputLayer layer)
        {
            return _InputMap.Remove(layer);
        }

        public void Clear()
        {
            _InputMap.Clear();
        }

        public override void DoUpdate()
        {
            if (Console.KeyAvailable == false) 
            {
                return;
            }

            if (_InputMap.Count == 0)
            {
                return;
            }

            var lastInputMap = _InputMap.Last();
            var key = Console.ReadKey(true);
            if (lastInputMap.Value.ContainsKey(key.Key) == false)
            {
                return;
            }
            foreach (var func in lastInputMap.Value[key.Key])
            {
                func();
            }
        }

        public override void OnDisable()
        {

        }

        public override void OnEnable()
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

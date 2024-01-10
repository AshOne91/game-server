using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using TestClient.FrameWork;

namespace TestClient.TestClient
{
    public class ConsoleManager : AppSubSystem<ConsoleManager>
    {
        private bool _pause;
        private string[] _buffers;
        private int _activeIndex;
        private int _bufferIndex;
        public ConsoleManager() 
        {
            _pause = false;
            _buffers = new string[2];
            _activeIndex = 0;
            _bufferIndex = 1;
        }
        public sealed override void OnEnable()
        {
        }
        public sealed override void OnDisable() 
        { 
        }
        public sealed override void OnInit()
        {
            Console.WindowWidth = 75;
            Console.WindowHeight = 60;
            Console.CursorVisible = false;
        }
        public sealed override void OnRelease()
        {

        }
        public void MoveCursor(int posX, int posY)
        {
            Console.SetCursorPosition(posX, posY);
        }
        public void BufferDraw()
        {
            Console.Write(_buffers[_activeIndex]);
        }
        public void BufferFlip()
        {
            if (_buffers[_bufferIndex].Length > 0) 
            {
                int tmp = _activeIndex;
                _activeIndex = _bufferIndex;
                _bufferIndex = tmp;
            }
        }
        public void BufferClear()
        {
            _buffers[_activeIndex] = string.Empty;
            MoveCursor(0, 0);
        }
        public sealed override void DoUpdate()
        {
            if (_pause == false)
            {
                BufferClear();
                BufferFlip();
                BufferDraw();
            }
        }
        public void SetBuffer(string console)
        {
            _buffers[_bufferIndex] = console;
        }

        public void ConsoleClear()
        {
            MoveCursor(0, 0);
            _buffers[0] = string.Empty;
            _buffers[1] = string.Empty;
            Console.Clear();
        }

        public void Pause()
        {
            _pause = true;
        }
        public void Resume()
        {
            _pause = false;
        }
    }
}

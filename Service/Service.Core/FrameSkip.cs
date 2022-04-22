using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core
{
    public class FrameSkip
    {
        public FrameSkip()
        {
            Clear();
        }

        public void Clear()
        {
            SetFramePerSec(10.0f);
            Timer = 0.0f;
        }

        public void SetFramePerSec(float fps)
        {
            SecPerFrame = 1.0f / fps;
            Timer = 0.0f;
        }
        public bool Update(float dt)
        {
            Timer += dt;

            if (Timer >= SecPerFrame)
            {
                Timer -= SecPerFrame;
                return true;
            }

            return false;
        }

        public bool IsFrameSkip()
        {
            return (Timer >= 0);
        }

        public float GetFramePerSec()
        {
            return SecPerFrame;
        }

        public void ResetTimer()
        {
            Timer = 0.0f;
        }

        protected float Timer;
        protected float SecPerFrame;
    }
}

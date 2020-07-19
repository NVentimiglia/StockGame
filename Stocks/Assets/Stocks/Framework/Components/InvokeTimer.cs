using System;

namespace Framework
{
    /// <summary>
    /// Pattern for having a custom intervol timer
    /// </summary>
    public class InvokeTimer
    {
        public float Duration;
        public Action Handle;
        public float Delta;

        public InvokeTimer(float duration, Action handle)
        {
            Duration = duration;
            Handle = handle;
        }

        public void Update(float delta)
        {
            Delta += delta;
            if (Delta >= Duration)
            {
                Delta = 0;
                Handle();
            }
        }
    }
}
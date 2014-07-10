using System;

namespace SoundModem.Base
{
    public class EventArgs<T> : EventArgs
    {
        public readonly T Value;

        public EventArgs(T value)
        {
            this.Value = value;
        }

        public static void FireEvent(object sender, EventHandler<EventArgs<T>> handler, T value)
        {
            if (handler != null)
            {
                handler(sender, new EventArgs<T>(value));
            }
        }
    }
}

using System;

namespace Common.PubSub
{
    public interface IMessageConsume<TConsume>
    {
        public event EventHandler<TConsume> OnConsume;
    }
}

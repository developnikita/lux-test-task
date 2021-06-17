using System;

namespace Common.PubSub
{
    public class IMessageConsume<TConsume>
    {
        event EventHandler<TConsume> OnConsume;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Infrastructure.Messaging
{
    public interface IMessageBus
    {
        Task PublishAsync(string queue, object message);
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yape.Domain.Message
{
    public interface IMessageConsumer
    {
        Task ConsumeMessageAsync(CancellationToken cancellationToken);
    }
}

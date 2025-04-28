using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yape.Domain.Entities;

namespace Yape.Domain.Message
{
    public interface IMessageProducer
    {
        Task ProduceMessageAsync(QueueMessage message, CancellationToken cancellationToken);
    }
}

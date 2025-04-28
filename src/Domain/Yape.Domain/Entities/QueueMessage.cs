using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yape.Domain.Entities
{
    public class QueueMessage
    {
        public string Key { get; set; } = default!;
        public string Value { get; set; } = default!;
    }
}

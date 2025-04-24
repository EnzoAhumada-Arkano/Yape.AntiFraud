using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yape.Domain.Entities
{
    public class KafkaMessage
    {
        public string Key { get; set; } = default!;
        public string Value { get; set; } = default!;
    }
}

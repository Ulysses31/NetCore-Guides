using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncApiMinimal.Models
{
    public class BatchProcessItems
    {
        public int Id { get; set; }

        public string? ItemMessage { get; set; }

        public string? RequestId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

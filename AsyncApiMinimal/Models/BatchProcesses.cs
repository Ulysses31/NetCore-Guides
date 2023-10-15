using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncApiMinimal.Models
{
    public class BatchProcesses
    {
        public int Id { get; set; }

        public string? RequestBody { get; set; }

        public string? EstimateCompetionTime { get; set; }

        public string? RequestStatus { get; set; }

        public string? RequestId { get; set; } = Guid.NewGuid().ToString();

        public string? RequestStatusUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncApiMinimal.Dtos
{
    public class BatchProcessStatus
    {
        public string? RequestStatus { get; set; }

        public string? EstimatedCompetionTime { get; set; }

        public string? ResourceUrl { get; set; }

        public string? RequestId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBroker.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? TopicMessage { get; set; }

        public int SubscriptionId { get; set; }

        [Required]
        public DateTime ExpiresAfter { get; set; } = DateTime.Now.AddDays(1);

        public string? MessageStatus { get; set; } = "NEW";
    }
}

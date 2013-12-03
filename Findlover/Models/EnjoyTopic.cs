using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Findlover.Models
{
    public class EnjoyTopic
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public int UserId { get; set; }
        public DateTime ActionTime { get; set; }
        public bool IsRead { get; set; }
    }
}
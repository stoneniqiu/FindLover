using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Findlover.Models
{
    public class VisitLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int VisitorId { get; set; }
        public string VisitorName { get; set; }
        public bool IsRead { get; set; }
        public int Count { get; set; }
        public DateTime ActionTime { get; set; }
    }
}
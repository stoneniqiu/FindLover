using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Findlover.Models;

namespace Findlover.ViewModel
{
    public class TopicUser
    {
        public UninUser UninUser { get; set; }
        public Topic Topic { get; set; }
    }

    public class SimpleTopic
    {
        public int TopId { get; set; }
    }
}
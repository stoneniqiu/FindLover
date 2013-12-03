using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Findlover.Models;

namespace Findlover.ViewModel
{
    public class UninUser
    {
        public User User { get; set; }
        public BaseInfo BaseInfo { get; set; }
        public DetailInfo DetailInfo { get; set; }
        public LoveView LoveView { get; set; }
    }
}
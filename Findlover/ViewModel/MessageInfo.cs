using System;
using Findlover.Models;

namespace Findlover.ViewModel
{
    public class MessageInfo
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string ImgUrl { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public string Message { get; set; }
        public DateTime LastMessageTime { get; set; }
        public int MsgCount { get; set; }
        public string Tag { get; set; }
    }
}
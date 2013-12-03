using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Findlover.ViewModel
{
    public class PersonInfo
    {
        public UninUser Myself { get; set; }
        public double Persent { get; set; }
        public string Info { get; set; }
        public int LoveMeCount { get; set; }
        public int IloveCount { get; set; }
        public int Visitcount { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCodeChems.Objects
{
   public class Profile
    {

        public string nickname { get; set; }
        public int victories { get; set; }
        public byte[] imageProfile { get; set; }
        public int defaults { get; set; }
        public string username { get; set; }
    }
}
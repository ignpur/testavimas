using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class Shot
    {
        public int x { get; set; }
        public int y { get; set; }
        public bool result { get; set; }

        public Shot(int x, int y, bool result)
        {
            this.x = x;
            this.y = y;
            this.result = result;
        }
    }
}

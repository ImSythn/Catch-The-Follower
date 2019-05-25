using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchTheFollower
{
    public class DoublePoint //object only used for storing positions of the two portals
    {
        private Point source;
        private Point target;

        public DoublePoint(Point source, Point target)
        {
            this.source = source;
            this.target = target;
        }

        public Point Source
        {
            get
            {
                return this.source;
            }
        }

        public Point Target
        {
            get
            {
                return this.target;
            }
        }
    }
}

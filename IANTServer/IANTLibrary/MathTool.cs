using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary
{
    public static class MathTool
    {
        public static float Direction2DToRotation2D(double x, double y)
        {
            return (float)(Math.Atan2(y, x) * 180.0 / Math.PI);
        }
    }
}

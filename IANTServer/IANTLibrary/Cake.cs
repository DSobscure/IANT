using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary
{
    public class Cake : Food
    {
        public override Food Duplicate()
        {
            return new Cake();
        }
    }
}

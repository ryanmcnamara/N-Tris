using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionMethods
{
    public static class MyExtensions
    {
        public static int GetHashCode(this BitArray a)
        {
            byte[] x = new byte[(a.Count + 7) / 8];
            a.CopyTo(x, 0);
            int hash = 17;
            for (int i = 0; i < x.Length; i++)
            {
                hash = hash * 23 + x[i].GetHashCode();
            }
            return hash;
        }
    }   
}

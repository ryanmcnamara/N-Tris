using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

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

        public static Color FromArgb(this Color c, int x)
        {
            byte a = (byte)(((x >> 24) & 0xFF));
            byte r = (byte)(((x >> 16) & 0xFF));
            byte g = (byte)(((x >> 8) & 0xFF));
            byte b = (byte)(((x >> 0) & 0xFF));
            Color q = Color.FromArgb(a, r, g, b);
            return q;
        }

        public static int ToArgb(this Color c )
        {
            int x = 0;
            x |= (c.A << 24);
            x |= (c.R << 16);
            x |= (c.G << 8);
            x |= (c.B << 0);

            return x;
        }
    }   
}

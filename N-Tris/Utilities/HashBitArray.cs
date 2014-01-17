using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Tris.Utilities
{
    public class HashBitArray
    {
        private BitArray x;

        public HashBitArray(int p)
        {
            this.x = new BitArray(p);
        }

        public HashBitArray(bool[] x)
        {
            this.x = new BitArray( x.Length );
            int i = 0;
            foreach ( bool b in x )
            {
                this.x[i] = b;
                i++;
            }
        }

        public bool this[int i]
        {
            get { return x[i]; }
            set { x[i] = value; }
        }


        public override int GetHashCode()
        {
            byte[] z = new byte[(x.Count + 7) / 8];
            x.CopyTo(z, 0);
            int hash = 17;
            for (int i = 0; i < z.Length; i++)
            {
                hash = hash * 23 + z[i].GetHashCode();
            }
            return hash;
        }

        public override bool Equals(object obj)
        {
            HashBitArray o = obj as HashBitArray;
            if ( o == null )
            {
                return false;
            }
            byte[] z = new byte[(x.Count + 7) / 8];
            byte[] y = new byte[(o.Count + 7) / 8];
            x.CopyTo(z, 0);
            o.CopyTo(y, 0);
            if (y.Length != z.Length)
            {
                return false;
            }
            for (int i = 0; i < z.Length; i++)
            {
                if (z[i] != y[i])
                {
                    return false;
                }
            }
            return true; 
        }

        private void CopyTo(byte[] y, int p)
        {
            x.CopyTo(y,p);
        }

        public int Count { get { return x.Count; } private set { ; } }
    }
}

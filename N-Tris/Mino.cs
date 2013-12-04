using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Tris
{
    class Mino
    {
        public Vector2 v;
        public readonly Polyomino p;

        public Mino(Vector2 v, Polyomino p)
        {
            this.v = v;
            this.p = p;
        }

        public override int GetHashCode()
        {
            return v.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj as Mino == null)
            {
                return base.Equals(obj);
            }
            Mino o = (Mino)obj;
            return v.Equals(o.v);
        }

    }
}

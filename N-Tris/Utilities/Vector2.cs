using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    struct Vector2
    {
        public readonly int X;
        public readonly int Y;

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override int GetHashCode()
        {
            const int p = 3571;
            int hash = 17;
            hash = hash * p + X;
            hash = hash * p + Y;
            return hash;
        }

        public override bool Equals(object obj)
        {
            if (  obj is Vector2 )
            {
                Vector2 o = (Vector2) obj;
                return this.X == o.X && this.Y == o.Y;
            }
            else
            {
                return false;
            }
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, b.Y + a.Y);
        }

    }
